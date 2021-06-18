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
* runloop for RX and TX
* =========================
* 
*/

#include "../qo100trx.h"

void* plutorun_threadfunction(void* param);

void init_runloop()
{
	pthread_t plutorunthread;
	pthread_create(&plutorunthread, NULL, plutorun_threadfunction, NULL);
}

void* plutorun_threadfunction(void* param) 
{
ssize_t nbytes_rx, nbytes_tx;
uint8_t *p_dat, *p_start;

    pthread_detach(pthread_self());

    printf("entering pluto loop\n");
    
	while(keeprunning)
	{
      	// ====== receive samples from pluto ======
        // Refill RX buffer
        nbytes_rx = iio_buffer_refill(rxbuf);
        if (nbytes_rx < 0) { printf("Error refilling buf %d\n",(int) nbytes_rx); }
        //measure_samplerate(0,nbytes_rx/4,10);

        p_start = (uint8_t *)iio_buffer_first(rxbuf, rx0_i);

        //for(int ix=0; ix<nbytes_rx; ix++)
        //    measure_maxval(p_start[ix]);

        // sample buffer begins at p_start with length (PLUTOBUFSIZE * p_inc) bytes
        write_fifo(RXfifo,p_start,nbytes_rx);
        write_fifo(FFTfifo,p_start,nbytes_rx);
    
        // ====== send samples to pluto ======
        // get samples received via UDP
        static uint8_t pidata[PLUTOBUFSIZE*4];
        int lenfifo = read_fifo(TXfifo, pidata, PLUTOBUFSIZE*4);
        if(lenfifo)
        {
            p_dat = (uint8_t *)iio_buffer_first(txbuf, tx0_i);
            memcpy(p_dat,pidata,PLUTOBUFSIZE*4);

            nbytes_tx = iio_buffer_push(txbuf);
            if (nbytes_tx < 0) { printf("Error pushing buf %d\n", (int) nbytes_tx); }
        }

        usleep(1000);
	}
    printf("exit pluto loop\n");
    pthread_exit(NULL); // self terminate this thread
    return NULL;
}
