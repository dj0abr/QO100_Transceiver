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
* Usage Example: see main() in this file
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
* libkmaudio.cpp ... main() for test purposes only
* usually this library is linked to another program, in this case
* comment-out #define LIBTEST
*
*/

#include "libkmaudio.h"

int kmaudio_getDeviceList_test();

int keeprunning = 1;

/*
* main()
* for testing purposes only
* for library generation comment out: LIBTEST
*/


#define LIBTEST

#ifdef LIBTEST
int main()
{
    // initialize sound system
    // must be called once after program start
    // if called during program run, this will reset the sound system, so better don't do it
	kmaudio_init();
	
    // read list of devices
    // call as often as needed
    // if a user pluggs-in an USB device on the fly then the running stream may
    // be redirected by the OS. In this case closing/opening the stream
    // may be required.
	kmaudio_getDeviceList();
    
    // start capture and/or playback streams
    // Parameter: the device name and the sample rate (44100 or 48000 are supported)
    // these function return the fifo-index, which is used to access the data in the
    // coresponding fifo

	int capidx = kmaudio_startCapture((char *)"Line 2 (Virtual Audio Cable)", 48000);
    int pbidx = kmaudio_startPlayback((char *)"Line 2 (Virtual Audio Cable)", 48000);

    float f[1100];
    while (1)
    {
        // make a loop: record from Mic and play to Speaker

        int done = 0;
        // read samples from the capture fifo
        int anz = kmaudio_readsamples(capidx, f, 1000, 1.0f, 0);
        if (anz > 0)
        {
            // if samples are available, send them to playback fifo
            //printf("write %d samples from %d to %d\n", anz, capidx, pbidx);
            kmaudio_playsamples(pbidx, f, anz,1.0f);
            done = 1;
        }
    }

    // free resources. This will never happen in this example
    // but should be done in the final program
	kmaudio_close();
	return 0;
}
#endif // LIBTEST
