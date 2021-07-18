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
* libkmaudio_getDevices_linux.cpp
* like libkmaudio_getDevices.cpp, but uses libsoundio under Linux
* to get the device list. Portaudio does not work under Linux because
* it does not support pulseaudio. Therefore the linux functions
* use libsoundio
*
*/

#ifndef WIN32 // Linux

#include "libkmaudio.h"

int scan_devices();

int kmaudio_getDeviceList()
{
    if (soundio == NULL)
    {
        printf("kmaudio_getDeviceList: soundio not initialized\n");
        return -1;
    }

    soundio_flush_events(soundio); // to get actual data

    if (scan_devices() == -1) // read devices
    {
        printf("cannot read audio devices\n");
        return -1;
    }

    io_buildAudioDevString();

    // close stream if a device does not exist any more
    for (int i = 0; i < devanz; i++)
    {
        if (devlist[i].active == 0)
        {
            if (devlist[i].instream != NULL)
            {
                printf("capture device %s disconnected, stop stream\n", devlist[i].name);
                soundio_instream_destroy(devlist[i].instream);
                devlist[i].instream = NULL;
                devlist[i].working = 0;
            }

            if (devlist[i].outstream != NULL)
            {
                printf("playback device %s disconnected, stop stream\n", devlist[i].name);
                soundio_outstream_destroy(devlist[i].outstream);
                devlist[i].outstream = NULL;
                devlist[i].working = 0;
            }
        }
    }

    static int csum = 0;
    int sum = 0;
    uint8_t *p = (uint8_t *)&(devlist[0].index);
    for (int i = 0; i < (int)sizeof(devlist); i++)
        sum += *p++;

    if (csum != sum)
    {
        csum = sum;

        /*printf("====== Linux Devices found: ======\n");
        for (int i = 0; i < devanz; i++)
        {
            printf("Index:  %d\n", devlist[i].index);
            printf("Name: %s\n", devlist[i].name);
            printf("ID: %s\n", devlist[i].id);
            printf("Cap/PB: %d\n", devlist[i].in_out);
            printf("Channels: %d\n", devlist[i].stereo_mono);
            printf("SR 44100: %d\n", devlist[i].supports_44100);
            printf("SR 48000: %d\n", devlist[i].supports_48000);
            printf("is active: %s\n", devlist[i].active ? "yes" : "no");
            printf("--------------------------------------\n");
        }*/
    }
    return 0;
}

static void get_channel_layout(const struct SoundIoChannelLayout *layout)
{
    if (layout->name)
    {
        if (strstr(layout->name, "ereo"))
            devlist[devanz].stereo_mono = 2;
        if (strstr(layout->name, "ono"))
            devlist[devanz].stereo_mono = 1;
    }
}

int getDeviceParameters(int idx, struct SoundIoDevice *device)
{
    strncpy(devlist[idx].id, device->id, sizeof(devlist[0].id) - 1);
    devlist[idx].id[sizeof(devlist[0].id) - 1] = 0;
    strncpy(devlist[idx].name, device->name, sizeof(devlist[0].name) - 1);
    devlist[idx].name[sizeof(devlist[0].name) - 1] = 0;

    for (int i = 0; i < device->layout_count; i++)
        get_channel_layout(&device->layouts[i]);

    int min = 999999, max = 0;
    for (int i = 0; i < device->sample_rate_count; i++)
    {
        struct SoundIoSampleRateRange *range = &device->sample_rates[i];
        if (range->min < min)
            min = range->min;

        if (range->max > max)
            max = range->max;
    }
    if (min <= 44100)
        devlist[idx].supports_44100 = 1;
    if (max >= 48000)
        devlist[idx].supports_48000 = 1;
    if (devlist[idx].supports_44100 == 0 && devlist[idx].supports_48000 == 0)
        return 0;
    return 1;
}

int getDevlistIndex(char *name, char *id)
{
    /* !!! Achtung: das hier filtert Devices raus die auf PB und CAP gleiche Daten haben !!!
       noch nach PB und CAP unterscheiden
    for (int i = 0; i < devanz; i++)
    {
        // check if already exists
        if (!strcmp(devlist[i].id, id) && !strcmp(devlist[i].name, name))
            return i;
    }*/

    int newidx = devanz;
    devanz++;
    //printf("New Dev:%s Idx:%d\n", name, newidx);
    return newidx;
}

int scan_devices()
{
    for (int i = 0; i < devanz; i++)
        devlist[i].active = 0;

    int didx;
    for (int i = 0; i < soundio_input_device_count(soundio); i++)
    {
        struct SoundIoDevice *device = soundio_get_input_device(soundio, i);
        if (device == NULL)
            continue;
        if (strstr(device->name, "onitor"))
            continue;
        if (device->probe_error)
            continue;

        didx = getDevlistIndex(device->name, device->id);
        if (getDeviceParameters(didx, device) == 1)
        {
            printf("%d %d %d ====CAP: name:<%s>\n", i, devanz, didx, device->name);
            devlist[didx].in_out = 0;
            devlist[didx].index = didx;
            devlist[didx].active = 1;
        }
        else
        {
            *devlist[didx].name = 0;
            *devlist[didx].id = 0;
        }
        soundio_device_unref(device);
    }

    for (int i = 0; i < soundio_output_device_count(soundio); i++)
    {
        struct SoundIoDevice *device = soundio_get_output_device(soundio, i);
        if (device == NULL)
            continue;
        if (strstr(device->name, "onitor"))
            continue;
        if (device->probe_error)
            continue;

        didx = getDevlistIndex(device->name, device->id);
        if (getDeviceParameters(didx, device) == 1)
        {
            printf("%d %d %d ====PB : name:<%s>\n", i, devanz, didx, device->name);
            devlist[didx].in_out = 1;
            devlist[didx].index = didx;
            devlist[didx].active = 1;
        }
        else
        {
            *devlist[didx].name = 0;
            *devlist[didx].id = 0;
        }
        soundio_device_unref(device);
    }
    return 0;
}

#endif // ifndef WIN32