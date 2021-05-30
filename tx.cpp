/*
* Adalm Pluto Driver
* ==================
* Author: DJ0ABR
*
*   (c) DJ0ABR
*   www.dj0abr.de
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
* =========================
* SSB Transmitter
* =========================
* 
*/

/*
* reads len samples from device id into psamp
* returns: number of values written to psamp , -1=error
* id ... device id returned by kmaudio_startCapture
* psamp ... float array of length len getting the audio data (mono)
* len ... number of float values to write into psamp
* volume ... 0.0f..2.0f will be multiplied with the input sample
* wait ... 1=wait for data, 0=return if not enough data available (in this case psamp will return 0,0,0...)
* 
* if resampling is required the number of returned samples may differ from the number of requested samples
* it can be larger, so the buffer psamp should be larger than "len" by factor 1.1
*/

int kmaudio_readsamples(int id, float* psamp, int len, float volume, int wait);


#include "qo100trx.h"

int TXoffsetfreq = 280000;

void* tx_threadfunction(void* param);

void init_tx()
{
	pthread_t txthread;
	pthread_create(&txthread, NULL, tx_threadfunction, NULL);
}

void* tx_threadfunction(void* param) 
{
    pthread_detach(pthread_self());

    printf("entering TX loop\n");
	while(keeprunning)
	{
        float f[4800];
        if(capidx != -1)
        {
            int ret = kmaudio_readsamples(capidx, f, 4800, 1.0f, 0);
            if(ret)
            {
                if(audioloop)
                {
                    // send back to sound output
                    if(pbidx != -1)
                        kmaudio_playsamples(pbidx,f,ret,1.0f);
                }
                else
                {
                    // send to modulator
                    if(ptt)
                        upmix(f,ret,TXoffsetfreq);
                }
            }
            else
                usleep(1000);
        }
        else
            usleep(1000);
	}
    printf("exit TX loop\n");
    pthread_exit(NULL); // self terminate this thread
    return NULL;
}