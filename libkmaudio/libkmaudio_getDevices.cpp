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
* libkmaudio_getDevices.cpp
* read audio device list via portaudio (which is only used for Windows)
* prepare a device name string which can be read by another program
* in order to present the devices to use user for selection
*
*/

#include "libkmaudio.h"

void io_buildAudioDevString();

// number of detected devices, updated after a call to kmaudio_getDeviceList()
int devanz = 0;

// devlist contains all information for all detected devices
// the list is filled by a call to kmaudio_getDeviceList()
DEVLIST devlist[MAXDEVICES];

double latency = 0.2;   // WASAPI latency in seconds

#ifdef WIN32

/*static double standardSampleRates[] = {
8000.0, 9600.0, 11025.0, 12000.0, 16000.0, 
22050.0, 24000.0, 32000.0, 44100.0, 48000.0, 
88200.0, 96000.0, 192000.0, -1 };*/

static double standardSampleRates[] = {44100.0, 48000.0, -1};

int getDevlistIndex(const char* name, int ichans, int ochans)
{
    for (int i = 0; i < devanz; i++)
    {
        // check if already exists
        if (!strcmp(devlist[i].name, name) &&
            devlist[i].inputParameters.channelCount == ichans &&
            devlist[i].outputParameters.channelCount == ochans)
            return i;
    }

    int newidx = devanz;
    devanz++;
    //printf("New Dev:%s Idx:%d\n", name, newidx);
    return newidx;
}

int kmaudio_getDeviceList()
{
    int numDevices = Pa_GetDeviceCount();
    if (numDevices < 0)
    {
        printf("ERROR: Pa_GetDeviceCount returned 0x%x\n", numDevices);
        return -1;
    }

    //printf("%d Devices found\n", numDevices);

    for (int i = 0; i < devanz; i++)
        devlist[i].active = 0;

    int didx;
    for (int i = 0; i < numDevices; i++)
    {
        const PaDeviceInfo* deviceInfo = Pa_GetDeviceInfo(i);
        const PaHostApiInfo *ai = Pa_GetHostApiInfo(deviceInfo->hostApi);
        
        // Windows: use WASAPI devices only
        if (strstr(ai->name, "WASAPI") != NULL)
        {
            didx = getDevlistIndex(deviceInfo->name, deviceInfo->maxInputChannels, deviceInfo->maxOutputChannels);

            devlist[didx].devnum = i;
            snprintf(devlist[didx].name, MAXDEVNAMELENGTH - 1, "%s", deviceInfo->name);
            //printf("------%s-------\n", deviceInfo->name);

            devlist[didx].inputParameters.device = i;
            devlist[didx].inputParameters.channelCount = deviceInfo->maxInputChannels;
            devlist[didx].inputParameters.sampleFormat = paInt16;
            devlist[didx].inputParameters.suggestedLatency = latency;
            devlist[didx].inputParameters.hostApiSpecificStreamInfo = NULL;

            devlist[didx].outputParameters.device = i;
            devlist[didx].outputParameters.channelCount = deviceInfo->maxOutputChannels;
            devlist[didx].outputParameters.sampleFormat = paInt16;
            devlist[didx].outputParameters.suggestedLatency = latency;
            devlist[didx].outputParameters.hostApiSpecificStreamInfo = NULL;

            if (devlist[didx].inputParameters.channelCount > 0 && devlist[devanz].outputParameters.channelCount > 0)
                devlist[didx].in_out = 2;
            else if (devlist[didx].inputParameters.channelCount > 0)
                devlist[didx].in_out = 0;
            else if (devlist[didx].outputParameters.channelCount > 0)
                devlist[didx].in_out = 1;

            devlist[didx].index = didx;
            devlist[didx].active = 1;

            for (int j = 0; standardSampleRates[j] > 0; j++)
            {
                PaError err = 0;
                //if (devlist[didx].inputParameters.channelCount > 0 && devlist[didx].outputParameters.channelCount > 0)
                  //  err = Pa_IsFormatSupported(&devlist[didx].inputParameters, &devlist[didx].outputParameters, standardSampleRates[j]);
                if (devlist[didx].inputParameters.channelCount > 0)
                    err = Pa_IsFormatSupported(&devlist[didx].inputParameters, NULL, standardSampleRates[j]);
                if (devlist[didx].outputParameters.channelCount > 0)
                    err = Pa_IsFormatSupported(NULL, &devlist[didx].outputParameters, standardSampleRates[j]);

                // portaudio cannot detect if a device was removed, instead it delivers errors
                if (err == paInvalidDevice)
                    devlist[didx].active = 0;
                else if (err == paFormatIsSupported)
                {
                    if (j == 0) devlist[didx].supports_44100 = 1;
                    if (j == 1) devlist[didx].supports_48000 = 1;
                }
            }
        }
    }

    io_buildAudioDevString();

    // close stream if a device does not exist any more
    for (int i = 0; i < devanz; i++)
    {
        if (devlist[i].active == 0)
        {
            if (devlist[i].capStream != NULL)
            {
                printf("capture device %s disconnected, stop stream\n", devlist[i].name);
                Pa_CloseStream(devlist[i].capStream);
                devlist[i].capStream = NULL;
                devlist[i].working = 0;
            }

            if (devlist[i].pbStream != NULL)
            {
                printf("playback device %s disconnected, stop stream\n", devlist[i].name);
                Pa_CloseStream(devlist[i].pbStream);
                devlist[i].pbStream = NULL;
                devlist[i].working = 0;
            }
        }
    }

    static int csum = 0;
    int sum = 0;
    uint8_t* p = (uint8_t*)&(devlist[0].index);
    for (int i = 0; i < (int)sizeof(devlist); i++)
        sum += *p++;

    if (csum != sum)
    {
        csum = sum;

        printf("Windows Devices found:\n");
        for (int i = 0; i < devanz; i++)
        {
            printf("Portaudio ID: %d\n", devlist[i].index);
            printf("Name: %s\n", devlist[i].name);
            printf("Cap/PB: %d\n", devlist[i].in_out);
            printf("Channels: i:%d o:%d\n", devlist[i].inputParameters.channelCount, devlist[i].outputParameters.channelCount);
            printf("SR 44100: %d\n", devlist[i].supports_44100);
            printf("SR 48000: %d\n", devlist[i].supports_48000);
            printf("is active: %s\n", devlist[i].active ? "yes" : "no");
        }
    }

    return 0;
}

#endif //WIN32


// find a device in devlist
// returns: list index or -1 if error
int searchDevice(char* devname, int io)
{
    for (int i = 0; i < devanz; i++)
    {
        if (strcmp(devname, devlist[i].name) == 0 && (devlist[i].in_out == io || devlist[i].in_out == 2))
            return i;
    }
    return -1;
}

// choose physical, real sample rate for a device
// returns: 0=ok, -1=error: no sample rate supported
int getRealSamprate(int idx)
{
    if (devlist[idx].requested_samprate == 44100)
    {
        if (devlist[idx].supports_44100)		devlist[idx].real_samprate = 44100;
        else if (devlist[idx].supports_48000)	devlist[idx].real_samprate = 48000;
        else return -1;
    }

    else if (devlist[idx].requested_samprate == 48000)
    {
        if (devlist[idx].supports_48000)		devlist[idx].real_samprate = 48000;
        else if (devlist[idx].supports_44100)	devlist[idx].real_samprate = 44100;
        else return -1;
    }

    else
    {
        if (devlist[idx].supports_48000)		devlist[idx].real_samprate = 48000;
        else if (devlist[idx].supports_44100)	devlist[idx].real_samprate = 44100;
        else return -1;
    }

    return 0;
}

// build string of audio device name, to be sent to application as response to Broadcast search
// starting with PB devices, sperarator ^, capture devices
// separator between devices: ~
#define MAXDEVSTRLEN (MAXDEVICES * (MAXDEVNAMELENGTH + 2) + 10)
uint8_t io_devstring[MAXDEVSTRLEN];

void io_buildAudioDevString()
{
    memset(io_devstring, 0, sizeof(io_devstring));
    io_devstring[0] = ' ';     // placeholder for other data
    io_devstring[1] = ' ';     
    io_devstring[2] = ' ';
    io_devstring[3] = ' ';
    io_devstring[4] = ' ';

    // playback devices
    for (int i = 0; i < devanz; i++)
    {
        if (devlist[i].active == 0) continue;
        if (strlen((char *)io_devstring) > MAXDEVSTRLEN)
        {
            printf("io_devstring too small:%d / %d. Serious error, abort program\n", MAXDEVSTRLEN, (int)strlen((char*)io_devstring));
            exit(0);
        }
        if (devlist[i].in_out == 1)
        {
            strcat((char*)io_devstring, devlist[i].name);
            strcat((char*)io_devstring, "~");    // audio device separator
        }
    }

    strcat((char*)(io_devstring + 1), "^");    // PB, CAP separator

    // capture devices
    for (int i = 0; i < devanz; i++)
    {
        if (devlist[i].active == 0) continue;
        if (strlen((char*)io_devstring) > MAXDEVSTRLEN)
        {
            printf("io_devstring too small:%d / %d. Serious error, abort program\n", MAXDEVSTRLEN, (int)strlen((char*)io_devstring));
            exit(0);
        }
        if (devlist[i].in_out == 0)
        {
            strcat((char*)io_devstring, devlist[i].name);
            strcat((char*)io_devstring, "~");    // audio device separator
        }
    }

    //printf("<%s>\n", (char *)io_devstring);
}

uint8_t* io_getAudioDevicelist(int* len)
{
    *len = strlen((char*)(io_devstring + 1)) + 1;
    return io_devstring;
}
