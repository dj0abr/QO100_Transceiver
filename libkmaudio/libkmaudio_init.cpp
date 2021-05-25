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
* libkmaudio_init.cpp ... initialize portaudio (Windows only)
*
*/

#include "libkmaudio.h"

void kmaudio_close();

int kmaudio_init()
{
    
    kmaudio_close();
    sleep_ms(100);
    
	printf("libkmaudio_init\n");

    init_pipes();       // init fifo
    init_maxarray();    // init array for maxlevel measurement

#ifdef WIN32
    int err = Pa_Initialize();
    if (err != paNoError)
    {
        printf("ERROR: Pa_Initialize returned 0x%x\n", err);
        return -1;
    }

    printf("PortAudio version: 0x%08X\n", Pa_GetVersion());
#else
    return kmaudio_init_linux();
#endif

	return 0;
}

void kmaudio_close()
{
	printf("libkmaudio_close\n");

#ifdef WIN32
    for (int i = 0; i < devanz; i++)
    {
        if (devlist[i].capStream != NULL)
        {
            Pa_CloseStream(devlist[i].capStream);
            devlist[i].capStream = NULL;
        }
        if (devlist[i].pbStream != NULL)
        {
            Pa_CloseStream(devlist[i].pbStream);
            devlist[i].pbStream = NULL;
        }
    }
    Pa_Terminate();

#else
    kmaudio_close_linux();
#endif
}

// diagonstic routines for development

#define MAXSPDARR   10
int spdarr[MAXSPDARR];
int spdarrbps[MAXSPDARR];

#ifdef _LINUX_
uint64_t getms()
{
    struct timeval  tv;
    gettimeofday(&tv, NULL);
    uint64_t at = tv.tv_sec * 1000000 + tv.tv_usec;
    at = at / 1000;
    return at;
}
#endif

#ifdef WIN32
uint64_t getms()
{
    // get time in 100ns resolution
    FILETIME ft_now;
    GetSystemTimeAsFileTime(&ft_now);

    // convert to full 64 bit time
    uint64_t ll_now = (uint64_t)ft_now.dwLowDateTime + ((uint64_t)(ft_now.dwHighDateTime) << 32LL);

    // convert to Milliseconds
    ll_now /= (10 * 1000);      // still needs 64 bit integer

    return ll_now;
}
#else
uint64_t getms()
{
    struct timeval  tv;
 	gettimeofday(&tv, NULL);
    uint64_t at = tv.tv_sec * 1000000 + tv.tv_usec;
    at = at / 1000;
    return at;
}
#endif

void measure_speed_bps(int len)
{
    static uint64_t lasttim = 0;
    static int elems = 0;

    uint64_t tim = getms();
    int timespan = (int)(tim - lasttim);
    if (timespan < 0)
    {
        lasttim = tim;
        return;
    }

    elems += len;
    if (timespan < 1000) return;

    double dspd = elems;
    dspd = dspd * 1e3 / timespan;
    int speed = (int)dspd;

    // here we have number of elements after 1s
    printf(" ======================= %d bit/s\n", speed);

    elems = 0;
    lasttim = tim;
}

void sleep_ms(int ms)
{
#ifdef WIN32
    Sleep(ms);
#else
    usleep(ms * 1000);
#endif
}
