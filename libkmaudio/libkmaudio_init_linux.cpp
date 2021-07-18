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
* libkmaudio_init_linux.cpp ... initialize libsoundio (Linux only)
*
*/
#include "libkmaudio.h"

struct SoundIo* soundio = NULL;

#ifndef WIN32 // Linux

int kmaudio_init_linux()
{
    int err;

    // prepare and connect to libsoundio
    soundio = soundio_create();
    if (!soundio) {
        printf("soundio_create: out of memory\n");
        return -1;
    }

    if ((err = soundio_connect(soundio))) {
        printf("soundio_connect: %s\n", soundio_strerror(err));
        return -1;
    }
    printf("linux libkmaudio_init finished\n");

	return 0;
}

void kmaudio_close_linux()
{
    for (int i = 0; i < devanz; i++)
    {
        if (devlist[i].instream) soundio_instream_destroy(devlist[i].instream);
        devlist[i].instream = NULL;

        if (devlist[i].outstream) soundio_outstream_destroy(devlist[i].outstream);
        devlist[i].outstream = NULL;

        if (devlist[i].io_pb_device) soundio_device_unref(devlist[i].io_pb_device);
        devlist[i].io_pb_device = NULL;

        if (devlist[i].io_cap_device) soundio_device_unref(devlist[i].io_cap_device);
        devlist[i].io_cap_device = NULL;
    }

    if (soundio) soundio_destroy(soundio);
    soundio = NULL;
}

#endif // ndef WIN32
