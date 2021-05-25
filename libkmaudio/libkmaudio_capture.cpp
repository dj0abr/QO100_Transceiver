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
* starts a portaudio capture stream and a callback routine. Writes the
* received audio samples into the fifo (Windows only)
*/
#include "libkmaudio.h"

#define FRAMES_PER_BUFFER 512

int recordCallback(const void* inputBuffer, void* outputBuffer,
    unsigned long framesPerBuffer,
    const PaStreamCallbackTimeInfo* timeInfo,
    PaStreamCallbackFlags statusFlags,
    void* userData);

void close_capture_stream(int idx)
{
	if (devlist[idx].capStream != NULL)
	{
		Pa_CloseStream(devlist[idx].capStream);
		devlist[idx].capStream = NULL;
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

	int idx = searchDevice(devname, 0);
	if (idx == -1)
	{
		printf("Device:<%s> not found\n", devname);
		return -1;
	}

	devlist[idx].working = 0;

	close_capture_stream(idx);

	printf("Starting CAP stream:%s [%d]\n", devname, idx);

	io_fifo_clear(idx);

	devlist[idx].requested_samprate = samprate;
	if(getRealSamprate(idx) == -1)
	{
		printf("Samplerate %d not supported by device:<%s>\n", samprate, devname);
		return -1;
	}

	if (devlist[idx].requested_samprate != devlist[idx].real_samprate)
		resampler_create(idx);

	struct PaWasapiStreamInfo wasapiInfo;
	memset(&wasapiInfo, 0, sizeof(PaWasapiStreamInfo));
	wasapiInfo.size = sizeof(PaWasapiStreamInfo);
	wasapiInfo.hostApiType = paWASAPI;
	wasapiInfo.version = 1;
	wasapiInfo.flags = (paWinWasapiExclusive | paWinWasapiThreadPriority);
	wasapiInfo.threadPriority = eThreadPriorityProAudio;
	
	devlist[idx].inputParameters.hostApiSpecificStreamInfo = (&wasapiInfo);

	int e = Pa_IsFormatSupported(&devlist[idx].inputParameters, NULL, (double)devlist[idx].real_samprate);
	printf("err:%d device:%d PAdev:%d samprate: %f\n", e,idx, devlist[idx].devnum,(double)devlist[idx].real_samprate);

	devlist[idx].index = idx;

	int err = Pa_OpenStream(
		&devlist[idx].capStream,
		&devlist[idx].inputParameters,
		NULL,                  
		(double)devlist[idx].real_samprate,
		FRAMES_PER_BUFFER,
		paClipOff,      
		recordCallback,
		&(devlist[idx].index));

	if (err != paNoError)
	{
		printf("cannot open capture stream for device:<%s> %d\n", devname,err);
		return -1;
	}

	err = Pa_StartStream(devlist[idx].capStream);
	if (err != paNoError)
	{
		printf("cannot start capture stream for device:<%s>\n", devname);
		return -1;
	}

	printf("Capture started sucessfully\n");
	devlist[idx].working = 1;
	return idx;
}

int recordCallback( const void*                     inputBuffer, 
                    void*                           outputBuffer,
                    unsigned long                   framesPerBuffer,
                    const PaStreamCallbackTimeInfo* timeInfo,
                    PaStreamCallbackFlags           statusFlags,
                    void*                           userData)
{
    const int16_t* rptr = (const int16_t*)inputBuffer;
    int devidx = *((int *)userData);
	int chans = devlist[devidx].inputParameters.channelCount;

	//printf("%ld captured %d frames. Flag: %X\n", (long)rptr,framesPerBuffer, statusFlags);
	//measure_speed_bps(framesPerBuffer);

	//printf("%d %d\n", chans, rptr[0]);
	for (unsigned int i = 0; i < framesPerBuffer; i++)
		io_write_fifo_short(devidx, rptr[i * chans]);

    // Prevent unused variable warnings
    (void)outputBuffer; 
    (void)timeInfo;
    (void)statusFlags;

	if(keeprunning == 1)
		return paContinue;

	return paComplete;
}
