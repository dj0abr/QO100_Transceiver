/*
* QO100 Transceiver based on ADALM-PLUTO and Raspi/Odroid... SBCs
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
*/

#include "qo100trx.h"



#ifdef AUDIO
char plutoid[100] = {"ip:192.168.20.25"};
#else
char plutoid[100];
#endif

char gui_ip[20] = {"127.127.0.1"};
int udprxsock = 0;

// fifos to send/receive samples with pluto run thread
int RXfifo;
int TXfifo;
int FFTfifo;

int pbidx, capidx;

void udprxfunc(uint8_t *pdata, int len, struct sockaddr_in* sender)
{
	if(pdata[0] == 0 || pdata[0] == 1)
	{
		// set RX offset
		uint32_t off = pdata[1];
		off <<= 8;
		off |= pdata[2];
		off <<= 8;
		off |= pdata[3];
		off <<= 8;
		off |= pdata[4];

		printf("RX offset: %d\n",off);

		RXoffsetfreq = off;
	}

	if(pdata[0] == 1 || pdata[0] == 2)
	{
		// set TX offset
		// TODO
	}
}

void close_program()
{
    printf("\nCtrl-C pressed\n");
    keeprunning = 0;
	sleep(1);
	close_liquid();
	close_fft();
}

int main ()
{
	printf("=== QO100 Transceiver, by DJ0ABR ===\n");

    install_signal_handler(close_program);

	// find a pluto connected via USB or Ethernet
	if(*plutoid == 'i')
	{
		strcpy(pluto_context_name,plutoid);
	}
	else
	{
		// automatically search a pluto connected via USB
		int res = pluto_get_USB(plutoid);
        if(!res)
        {
            printf("no Pluto found on USB, exit program\n");
            exit(0);
        }
	}

	//  Pluto found, now initialize it
	printf("Pluto IP/USB ID      : <%s>\n",pluto_context_name);
	RXfifo = create_fifo(200, PLUTOBUFSIZE*4);
	TXfifo = create_fifo(200, PLUTOBUFSIZE*4);
	FFTfifo = create_fifo(200, PLUTOBUFSIZE*4);
	pluto_setup();

	// init audio (soundcard)
	#ifdef AUDIO
	if(kmaudio_init() == -1)
	{
		printf("NO AUDIO device\n");
		exit(0);
	}
	kmaudio_getDeviceList();
	pbidx = kmaudio_startPlayback((char *)"USB Advanced Audio Device Analog Stereo", 48000);
	if(pbidx == -1)
	{
		printf("NO AUDIO play device\n");
		exit(0);
	}
	//capidx = kmaudio_startCapture((char *)"USB Advanced Audio Device Analog Stereo", 48000);
	#endif

	// init DSP
	init_liquid();
	
	// pluto RX/TX is now running, samples are available in the fifos
	// start the SSB receiver and transmitter
	init_fft();
	init_rx();
	init_tx();

	// UDP receiver for commands from GUI
	UdpRxInit(&udprxsock, 40821, udprxfunc , &keeprunning);

	printf("initialisation finished. Enter normal operation (press Ctrl+C to cancel)\n");
	while(keeprunning)
	{
		usleep(1000);

	}

	return 0;
}
