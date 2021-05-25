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
*/

#pragma once


#ifdef WIN32
// ignore senseless warnings invented by M$ to confuse developers
#pragma warning( disable : 4091 )
#pragma warning( disable : 4003 )
#endif

#include <stdio.h>
#include <ctype.h>
#include <string.h>
#include <errno.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <signal.h>
#include <stdlib.h>
#include <stdarg.h>
#include <stdint.h>
#include <wchar.h>

#ifdef WIN32
#include "Winsock2.h"
#include "io.h"
#include <Windows.h>
#include <iostream>
#include <process.h>
#include <Tlhelp32.h>
#include <winbase.h>
#include <Shlobj.h>
#define _USE_MATH_DEFINES
#include <math.h>
#include "portaudio.h"
#include "pa_win_wasapi.h"

#pragma comment(lib, "portaudio_x86.lib")
#pragma comment(lib, "libliquid.lib")
#else
#include <sys/socket.h>
#include <unistd.h>
#include <sys/time.h>
#include <sys/ioctl.h>
#include <termios.h>
#include <sys/file.h>
#include <pthread.h>
#include <netinet/in.h>
#include <netdb.h>
#include <arpa/inet.h>
#include <pwd.h>
#include <math.h>
#include "soundio.h"
#endif

#include "liquid.h"

/*
* Sample usage:
* init sound system
* 1. kmaudio_init();
* 2. kmaudio_getDeviceList();
* 
* start a capture and a playback stream 
* 3. kmaudio_startCapture() using a device name returned by io_getAudioDevicelist()
* 4. kmaudio_startPlayback() using a device name returned by io_getAudioDevicelist()
* 
* now everything runs in the background, no more need for the user program
* to handle sound specific data
* now we can read/write sound samples as needed by our application
* as an example: get sound from microphone and send to speaker
* process in a loop:
* 5. kmaudio_readsamples()
* 6. kmaudio_playsamples()
* 
* for a working example see main(), you will need to
* replace the device names with the names in your computer
*/

/*
* initialize the audio library, create required processes
* call only once when your application starts
* returns: 0 = OK, -1 = error
*/
int kmaudio_init();

/*
* closes and frees all resources
* call only when the application exits
*/
void kmaudio_close();

/*
* read a list of all available audio devices into devlist
* the list can then be read by calling io_getAudioDevicelist()
* call any time to find new plugged in/out devices
* returns: 0=OK, -1 if error
*/
int kmaudio_getDeviceList();

/*
* starts a capturing stream from devname with samprate
* returns: id of the capture stream or -1 = error
*/
int kmaudio_startCapture(char* devname, int samprate);

/*
* starts a playback stream to devname with samprate
* returns: id of the playback stream or -1 = error
*/

int kmaudio_startPlayback(char* devname, int samprate);

/* 
* plays len samples from psamp to device id
* returns: 0=ok, -1=error
* id ... device id returned by kmaudio_startPlayback
* psamp ... float array of length len with the audio data (mono)
* len ... number of float values in psamp
* volume ... 0.0f..2.0f will be multiplied with the output sample
*/
int kmaudio_playsamples(int id, float* psamp, int len, float volume);

/*
* reads len samples from device id into psamp
* returns: 0=ok, -1=error
* id ... device id returned by kmaudio_startCapture
* psamp ... float array of length len getting the audio data (mono)
* len ... number of float values to write into psamp
* volume ... 0.0f..2.0f will be multiplied with the input sample
* wait ... 1=wait for data, 0=return if not enough data available (in this case psamp will return 0,0,0...)
*/
int kmaudio_readsamples(int id, float* psamp, int len, float volume, int wait);

/*
* reads the names of detected sound devices
* *len...length of the returned string
* returns: pointer to device string
* Format of the device string:
* first byte = 3 ... ID of this string, followed by pure text:
* Active status of the following device "0" or "1"
* Name of playback devices, followed by ~
* separator ^
* Active status of the following device "0" or "1"
* Name of capture devices, followed by ~
* these names are used for calls to kmaudio_startCapture and kmaudio_startPlayback
* to select the device
*/
uint8_t* io_getAudioDevicelist(int* len);

/*
* returns the max level (within 1 second) of this stream in % (0..100)
* if the level >= 100 the signal will get clipped and distorted
*/
uint8_t kmaudio_maxlevel(int id);

/*
* closes a stream which was started by 
* kmaudio_startCapture or kmaudio_startPlayback
* id ... stream ID which was returned by kmaudio_startCapture or kmaudio_startPlayback
*/
void close_stream(int id);

/*
* handle the FIFO which is used to buffer audio data
* pipenum ... stream ID which was returned by kmaudio_startCapture or kmaudio_startPlayback
* IMPORTANT: this information MUST be used to synchonize the data flow into
* the fifo. The speed is always defined by the audio sample rate
* by checking the fifo an application knows when it has to put more audio samples
* into the fifo
*/
// returns number of remaining elements (audio 16 bit short values) 
int io_fifo_freespace(int pipenum);

// returns number of used elements (audio 16 bit short values) 
int io_fifo_usedspace(int pipenum);

// like before, but returns a number between 0 and 100 %
int io_fifo_usedpercent(int pipenum);

// clear the fifo
void io_fifo_clear(int pipenum);


// -------- functions for internal use only --------

#define MAXDEVICES	200
#define MAXDEVNAMELENGTH	150

typedef struct _DEVLIST_ {
	int index = 0;				// index to this list
	int active = 0;				// 1=device valid, 0=possibly disconencted
	char name[MAXDEVNAMELENGTH] = { 0 };// real name
	int in_out = 0;				// 0=capture device, 1=playback device, 2=both
	int supports_44100 = 0;		// 1 if supported
	int supports_48000 = 0;		// 1 if supported
	int requested_samprate = 0; // sample rate requested by caller
	int real_samprate = 0;		// real sample rate of the device
	int working = 0;			// 0=not running, 1=initialized and working
#ifdef WIN32 // Windows using portaudio
	int devnum = -1;			// port audio device number
	PaStreamParameters inputParameters;
	PaStreamParameters outputParameters;
	PaStream* capStream = NULL;
	PaStream* pbStream = NULL;
#else // Linux using libsoundio
	struct SoundIoDevice* io_pb_device = NULL;
	struct SoundIoDevice* io_cap_device = NULL;
	struct SoundIoInStream* instream = NULL;
	struct SoundIoOutStream* outstream = NULL;
	int    stereo_mono = 2;    // 1=mono, 2=stereo
	char id[1000] = { 0 };
#endif
} DEVLIST;

int searchDevice(char* devname, int io);
void measure_speed_bps(int len);
void sleep_ms(int ms);
void io_write_fifo(int pipenum, float sample);
void io_write_fifo_short(int pipenum, int16_t sample);
void io_fifo_clear(int pipenum);
void init_pipes();
int io_read_fifo_num(int pipenum, float* data, int num);
int io_read_fifo(int pipenum, float* data);
int getRealSamprate(int idx);
int io_fifo_elems_avail(int pipenum);
void sleep_ms(int ms);
void io_buildAudioDevString();
void resampler_create(int devidx);
float* resample(int id, float* psamp, int len, int* pnewlen);
uint64_t getms();
void init_maxarray();
void kmaudio_detectDropouts(int id);
int io_read_fifo_num_short(int pipenum, int16_t* data, int num);
void close_capture_stream(int idx);
void close_playback_stream(int idx);

extern DEVLIST devlist[MAXDEVICES];
extern int devanz;
extern int keeprunning;

#ifndef WIN32 // Linux
int kmaudio_init_linux();
void kmaudio_close_linux();
char* getDevID(char* devname, int io, int* pidx);

extern struct SoundIo* soundio;
#endif // ndef WIN32

