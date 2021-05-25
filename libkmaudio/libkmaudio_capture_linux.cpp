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
* libkmaudio_capture.cpp ...
* starts a libsoundio capture stream and a callback routine. Writes the
* received audio samples into the fifo (Linux only)
*/

#ifndef WIN32

#include "libkmaudio.h"

char* getDevID(char* devname, int io, int *pidx)
{
    for (int i = 0; i < devanz; i++)
    {
        //printf("%s: %d %d\n", devlist[i].name, io, devlist[i].in_out);
        if (!strcmp(devname, devlist[i].name) && io == devlist[i].in_out)
        {
            *pidx = i;
            return devlist[i].id;
        }
    }
    return NULL;
}

int min_int(int a, int b)
{
    return (a < b) ? a : b;
}

void read_callback(struct SoundIoInStream* instream, int frame_count_min, int frame_count_max)
{
    int err;
    if (instream == NULL || soundio == NULL) return;
    //printf("cap: %d  %d\n", frame_count_min, frame_count_max);
    //int chans = instream->layout.channel_count;
    //printf("cap:%d\n", instream->sample_rate);
    int idx = *((int *)(instream->userdata));

    struct SoundIoChannelArea* areas;
    // samples are in areas.ptr
    int frames_left = frame_count_max; // take all 
    while (keeprunning)
    {
        int frame_count = frames_left;
        if ((err = soundio_instream_begin_read(instream, &areas, &frame_count)))
        {
            fprintf(stderr, "begin read error: %s", soundio_strerror(err));
            return;
        }
        if (!frame_count)
            break;

        //printf("write %d samples to fifo %d. Channels:%d\n", frame_count, idx, instream->layout.channel_count);
        for (int frame = 0; frame < frame_count; frame += 1)
        {
            for (int ch = 0; ch < instream->layout.channel_count; ch += 1)
            {
                float frxdata;
                memcpy(&frxdata, areas[ch].ptr, instream->bytes_per_sample);
                areas[ch].ptr += areas[ch].step;
                if (ch == 0)
                {
                    io_write_fifo(idx, frxdata);
                }
            }
        }

        //measure_speed_bps(frame_count);

        if ((err = soundio_instream_end_read(instream)))
        {
            printf("end read error: %s", soundio_strerror(err));
            return;
        }

        frames_left -= frame_count;
        if (frames_left <= 0)
            break;
    }
}

void overflow_callback(struct SoundIoInStream* instream)
{
    static int count = 0;
    printf("overflow %d\n", ++count);
}

void close_capture_stream(int idx)
{
    if (devlist[idx].instream != NULL)
    {
        soundio_instream_destroy(devlist[idx].instream);
        devlist[idx].instream = NULL;
    }
}

int kmaudio_startCapture(char* devname, int samprate)
{
    printf("Start request for CAP stream:%s\n", devname);

    if (devname == NULL || strlen(devname) < 3)  // no devices defined yet
    {
        printf("no capture devices specified\n");
        return -1;
    }

    int idx = 0; // index into devlist
    char* capdevid = getDevID(devname, 0, &idx);
    if (capdevid == NULL) return -1;

    // if an old stream is open, close it
    close_capture_stream(idx);

    printf("Starting CAP stream:%s [%d]\n", devname, idx);

    io_fifo_clear(idx);

    devlist[idx].working = 0;

    // define the capture device
    soundio_flush_events(soundio);

    for (int i = 0; i < soundio_input_device_count(soundio); i++)
    {
        devlist[idx].io_cap_device = NULL;
        struct SoundIoDevice* device = soundio_get_input_device(soundio, i);
        if (strcmp(device->id, capdevid) == 0)
        {
            devlist[idx].io_cap_device = device;
            break;
        }
        soundio_device_unref(device);
    }
    if (!devlist[idx].io_cap_device)
    {
        printf("Invalid device id: %s\n", capdevid);
        return -1;
    }

    if (devlist[idx].io_cap_device->probe_error)
    {
        printf("Unable to probe device: %s\n", soundio_strerror(devlist[idx].io_cap_device->probe_error));
        return -1;
    }

    // create capture callback
    devlist[idx].instream = soundio_instream_create(devlist[idx].io_cap_device);
    if (!devlist[idx].instream) {
        printf("capture: out of memory\n");
        return -1;
    }

    devlist[idx].requested_samprate = samprate;
    if (getRealSamprate(idx) == -1)
    {
        printf("Samplerate %d not supported by device:<%s>\n", samprate, devname);
        return -1;
    }

    if (devlist[idx].requested_samprate != devlist[idx].real_samprate)
        resampler_create(idx);

    devlist[idx].instream->format = SoundIoFormatFloat32NE;
    devlist[idx].instream->sample_rate = devlist[idx].real_samprate;
    devlist[idx].instream->software_latency = 0.1f;
    devlist[idx].instream->read_callback = read_callback;
    devlist[idx].instream->overflow_callback = overflow_callback;
    devlist[idx].instream->userdata = &(devlist[idx].index);

    int err = 0;
    if ((err = soundio_instream_open(devlist[idx].instream))) {
        printf("unable to open input stream: %d: %s", err, soundio_strerror(err));
        return -1;
    }

    if ((err = soundio_instream_start(devlist[idx].instream))) {
        printf("unable to start input device: %s", soundio_strerror(err));
        return -1;
    }

    printf("selected CAPTURE device:\nname:%s\nid  :%s\n", devlist[idx].name, capdevid);
    printf("physical capture rate:%d, requested capture rate:%d\n", devlist[idx].real_samprate, devlist[idx].requested_samprate);
    printf("format: %s\n\n", soundio_format_string(devlist[idx].instream->format));

    devlist[idx].working = 1;

    return idx;
}

#endif // #ifndef WIN32
