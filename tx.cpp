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

#include "qo100trx.h"

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


        usleep(1000);
	}
    printf("exit TX loop\n");
    pthread_exit(NULL); // self terminate this thread
    return NULL;
}