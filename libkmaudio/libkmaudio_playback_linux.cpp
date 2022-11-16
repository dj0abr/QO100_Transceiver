/*
* Audio Library for Linux and Windows
* ===================================
* Author: DJ0ABR
*
* Author: Kurt Moraw, Ham radio: DJ0ABR, github: dj0abr
* License: GPL-3
*
* compilation:
* Windows ... Visual Studio
* Linux ... make
*
* Documentation see: libkmaudio.h
*
*   This program is free software; you can redistribute it and/or modify
*   it under the terms of the GNU General Public License as published by
*   the Free Software Foundation; either version 2 of the License, or
*   (at your option) any later version.
*
*   This program is distributed in the hope that it will be useful,
*   but WITHOUT ANY WARRANTY; without even the implied warranty of
*   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*   GNU General Public License for more details.
*
*   You should have received a copy of the GNU General Public License
*   along with this program; if not, write to the Free Software
*   Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
*
* libkmaudio_playback.cpp ...
* starts a libsoundio playback stream and a callback routine. Plays the
* audio samples coming via the fifo (Linux only)
*/
#ifndef WIN32

#include "libkmaudio.h"

// #define SINEWAVETEST

#ifdef SINEWAVETEST
static const double PI = 3.14159265358979323846264338328;
static double seconds_offset = 0.0;
#endif

void write_sample_float32ne(char* ptr, double sample)
{
    float* buf = (float*)ptr;
    *buf = (float)sample;
}

static void write_callback(struct SoundIoOutStream* outstream, int frame_count_min, int frame_count_max)
{
    if (outstream == NULL || soundio == NULL) return;
    //printf("pb: %d  %d\n", frame_count_min, frame_count_max);
    //printf("pb :%d\n", outstream->sample_rate);
    int idx = *((int*)(outstream->userdata));

#ifdef SINEWAVETEST
    double float_sample_rate = outstream->sample_rate;
    double seconds_per_frame = 1.0 / float_sample_rate;
    double pitch = 440.0;
    double radians_per_second = pitch * 2.0 * PI;
#endif
    struct SoundIoChannelArea* areas;
    int err;

    int frames_left = 4800;
    if (frame_count_max < frames_left)
        frames_left = frame_count_max;

    for (;;) {
        int frame_count = frames_left;
        if ((err = soundio_outstream_begin_write(outstream, &areas, &frame_count))) {
            fprintf(stderr, "write_callback unrecoverable soundio_outstream_begin_write error: %s\n", soundio_strerror(err));
            return;
        }

        if (!frame_count)
            break;

        //printf("ck: %d read from fifo:%d\n", frame_count,idx);
        if (frame_count >= 10000)
        {
            printf("frame count >= 1000: %d\n", frame_count); 
            exit(0);
        }

        float f[10000];
        memset(f, 0, sizeof(float) * frame_count);
        if (io_fifo_elems_avail(idx) >= frame_count)
        {
            // if fifo does not have enough data, don't take any
            // this gives the fifo a chance to fill up a bit
            io_read_fifo_num(idx, f, frame_count);
        }

        //measure_speed_bps(frame_count);

        const struct SoundIoChannelLayout* layout = &outstream->layout;

        for (int frame = 0; frame < frame_count; frame += 1)
        {
#ifdef SINEWAVETEST
            double sample = sin((seconds_offset + frame * seconds_per_frame) * radians_per_second);
#endif
            for (int channel = 0; channel < layout->channel_count; channel += 1)
            {
                float ftx = f[frame];
                //getTXMax(f[frame]);
#ifdef SINEWAVETEST
                write_sample_float32ne(areas[channel].ptr, sample); // sine wave test tone
#endif
                write_sample_float32ne(areas[channel].ptr, ftx);
                areas[channel].ptr += areas[channel].step;
            }
        }
#ifdef SINEWAVETEST
        seconds_offset = fmod(seconds_offset + seconds_per_frame * frame_count, 1.0);
#endif

        if ((err = soundio_outstream_end_write(outstream))) {
            if (err == SoundIoErrorUnderflow)
                return;
            fprintf(stderr, "unrecoverable stream error: %s\n", soundio_strerror(err));
            return;
        }

        frames_left -= frame_count;
        if (frames_left <= 0)
            break;
    }
}

void underflow_callback(struct SoundIoOutStream* outstream)
{
    static int count = 0;
    printf("audio output underflow %d\n", count++);
}

void close_playback_stream(int idx)
{
    if (devlist[idx].outstream != NULL)
    {
        soundio_outstream_destroy(devlist[idx].outstream);
        devlist[idx].outstream = NULL;
    }
}

int kmaudio_startPlayback(char* devname, int samprate)
{
    printf("Start request for PB stream:%s\n", devname);

    if (devname == NULL || strlen(devname) < 3)  // no devices defined yet
    {
        printf("no PB devices specified\n");
        return -1;
    }

    int idx = 0; // index into devlist
    char* pbdevid = getDevID(devname, 1, &idx);
    if (pbdevid == NULL) 
    {
        printf("PB: Device ID not found\n");
        return -1;
    }

    close_playback_stream(idx);

    printf("Starting PB stream:%s [%d]\n", devname, idx);

    io_fifo_clear(idx);

    devlist[idx].working = 0;

    // define the capture device
    soundio_flush_events(soundio);

    for (int i = 0; i < soundio_output_device_count(soundio); i++)
    {
        devlist[idx].io_pb_device = NULL;
        struct SoundIoDevice* device = soundio_get_output_device(soundio, i);
        if (strcmp(device->id, pbdevid) == 0)
        {
            devlist[idx].io_pb_device = device;
            break;
        }
        soundio_device_unref(device);
    }
    if (!devlist[idx].io_pb_device)
    {
        printf("Invalid device id: %s\n", pbdevid);
        return -1;
    }

    if (devlist[idx].io_pb_device->probe_error)
    {
        printf("Unable to probe device: %s\n", soundio_strerror(devlist[idx].io_pb_device->probe_error));
        return -1;
    }

    // create playback callback
    devlist[idx].outstream = soundio_outstream_create(devlist[idx].io_pb_device);
    if (!devlist[idx].outstream) {
        printf("soundio_outstream_create: out of memory\n");
        return 0;
    }

    devlist[idx].requested_samprate = samprate;
    if (getRealSamprate(idx) == -1)
    {
        printf("Samplerate %d not supported by device:<%s>\n", samprate, devname);
        return -1;
    }

    //printf("requested SR:%d, real SR:%d\n",devlist[idx].requested_samprate, devlist[idx].real_samprate);
    if (devlist[idx].requested_samprate != devlist[idx].real_samprate)
        resampler_create(idx);

    devlist[idx].outstream->format = SoundIoFormatFloat32NE;
    devlist[idx].outstream->sample_rate = devlist[idx].real_samprate;
    devlist[idx].outstream->software_latency = 0.1f;
    devlist[idx].outstream->write_callback = write_callback;
    devlist[idx].outstream->underflow_callback = underflow_callback;
    devlist[idx].outstream->userdata = &(devlist[idx].index);

    int err = 0;
    if ((err = soundio_outstream_open(devlist[idx].outstream))) {
        printf("unable to open output stream: %s", soundio_strerror(err));
        return -1;
    }

    if ((err = soundio_outstream_start(devlist[idx].outstream))) {
        printf("unable to start output device: %s", soundio_strerror(err));
        return -1;
    }

    printf("selected PLAYBACK device:\nname:%s\nid  :%s\n", devname, pbdevid);
    printf("physical playback rate:%d, requested capture rate:%d\n", devlist[idx].real_samprate, devlist[idx].requested_samprate);
    printf("format: %s\n\n", soundio_format_string(devlist[idx].outstream->format));

    devlist[idx].working = 1;
    return idx;
}

#endif // ndef WIN32
