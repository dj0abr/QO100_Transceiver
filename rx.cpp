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
* SSB Receiver
* =========================
* 
*/

#include "qo100trx.h"

int RXoffsetfreq = 280000;

void* rx_threadfunction(void* param);

void init_rx()
{
	pthread_t rxthread;
	pthread_create(&rxthread, NULL, rx_threadfunction, NULL);
}

void* rx_threadfunction(void* param) 
{
    pthread_detach(pthread_self());

    printf("entering RX loop\n");
	while(keeprunning)
	{
		uint8_t data[PLUTOBUFSIZE*4];
		if(read_fifo(RXfifo, data, PLUTOBUFSIZE*4))
		{
            // Test: send RX back to TX (Repeater Mode)
			//write_fifo(TXfifo,data,PLUTOBUFSIZE*4);

            // samples received
            // Pluto RX tuned to 10489,47 (minus LNB and RX mixer)
            // the range is 10489,47 - 10490,03 which covers the full NB band

            // convert the raw stream into complex samples
            liquid_float_complex samples[PLUTOBUFSIZE];
            streamToSamples(data, PLUTOBUFSIZE*4, samples);

            // down mix the SSB channel into baseband, demodulate and send to soundcard
            downmix(samples,PLUTOBUFSIZE,RXoffsetfreq);
		}

        usleep(1000);
	}
    printf("exit RX loop\n");
    pthread_exit(NULL); // self terminate this thread
    return NULL;
}