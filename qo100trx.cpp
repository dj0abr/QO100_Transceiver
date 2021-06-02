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



char plutoid[100] = {"ip:192.168.20.25"};
//char plutoid[100];

char gui_ip[20] = {"127.127.0.1"};
int udprxsock = 0;
uint8_t audioloop = 0;
uint8_t rfloop = 0;
uint8_t ptt = 0;
uint8_t lastptt = 0;
char pbdevname[101] = {0};
char capdevname[101] = {0};
int newaudiodevs = 0;
int compressor = 0;
int audioagc = 0;
int gotPlutoID = 0;
int rxfilter = 2;
int txfilter = 3;
int rxmute = 0;

// fifos to send/receive samples with pluto run thread
int RXfifo;
int TXfifo;
int FFTfifo;

int pbidx = -1, capidx = -1;

void udprxfunc(uint8_t *pdata, int len, struct sockaddr_in* sender)
{
	//printf("UDP command from GUI: %d\n",pdata[0]);

	if(pdata[0] == 0)
	{
		// set RX offset
		uint32_t off = pdata[1];
		off <<= 8;
		off |= pdata[2];
		off <<= 8;
		off |= pdata[3];
		off <<= 8;
		off |= pdata[4];

		//printf("RX offset: %d\n",off);

		RXoffsetfreq = off;

		// set TX offset
		off = pdata[5];
		off <<= 8;
		off |= pdata[6];
		off <<= 8;
		off |= pdata[7];
		off <<= 8;
		off |= pdata[8];

		//printf("TX offset: %d\n",off);

		TXoffsetfreq = off;
	}

	if(pdata[0] == 3)
		audioloop = pdata[1];

	if(pdata[0] == 4)
	{
		ptt = pdata[1];
		if(ptt && lastptt == 0)
		{
			io_fifo_clear(capidx);
			fifo_clear(TXfifo);
		}
		lastptt = ptt;
	}

	if(pdata[0] == 5)
	{
		rfloop = pdata[1];
		if(rfloop)
		{
			setRXfrequency((long long)TX_FREQ);
		}
		else
		{
			setRXfrequency((long long)RX_FREQ);
		}
	}

	if(pdata[0] == 7)
	{
		memcpy(pbdevname,pdata+1,100);
		pbdevname[99] = 0;
		memcpy(capdevname,pdata+1+100,100);
		capdevname[99] = 0;
		printf("get Audiodevs: <%s><%s>\n",pbdevname,capdevname);
		newaudiodevs = 1;
	}

	if(pdata[0] == 8)
	{
		uint32_t rxbaseqrg;
		rxbaseqrg = pdata[1];
		rxbaseqrg <<= 8;
		rxbaseqrg += pdata[2];
		rxbaseqrg <<= 8;
		rxbaseqrg += pdata[3];
		rxbaseqrg <<= 8;
		rxbaseqrg += pdata[4];

		rxbaseqrg -= 30;

		uint32_t txbaseqrg;
		txbaseqrg = pdata[5];
		txbaseqrg <<= 8;
		txbaseqrg += pdata[6];
		txbaseqrg <<= 8;
		txbaseqrg += pdata[7];
		txbaseqrg <<= 8;
		txbaseqrg += pdata[8];

		txbaseqrg -= 30;

		RX_FREQ = (double)rxbaseqrg * 1000.0f;
		TX_FREQ = (double)txbaseqrg * 1000.0f;
	}

	if(pdata[0] == 9)
		compressor = pdata[1];

	if(pdata[0] == 10)
	{
		if(pdata[1] == 1)
		{
			// Pluto on local USB
			*plutoid = 0;
			printf("Pluto on local USB\n");
		}
		else
		{
			// Pluto on ETH
			memset(plutoid,0,sizeof(plutoid));
			strcpy(plutoid,"ip:");
			memcpy(plutoid+3,pdata+2,len-2);
			printf("Pluto on Ethernet IP: <%s>\n",plutoid);
		}
		gotPlutoID = 1;
	}

	if(pdata[0] == 11)
		audioagc = pdata[1];

	if(pdata[0] == 12)
		rxfilter = pdata[1];

	if(pdata[0] == 13)
		txfilter = pdata[1];

	if(pdata[0] == 14)
		rxmute = pdata[1];
}

void close_program()
{
    printf("\nCtrl-C pressed\n");
    keeprunning = 0;
	sleep(1);
	close_liquid();
	close_liquid_modulator();
	close_fft();
}

int main ()
{
	printf("=== QO100 Transceiver Pluto-Driver, by DJ0ABR ===\n");

	// https://github.com/torvalds/linux/blob/master/include/uapi/asm-generic/errno-base.h
	/*char s1[100];
	iio_strerror(-19, s1, 99);
	printf("<%s>\n",s1);
	exit(0);*/

    install_signal_handler(close_program);

	// Install FIFOs
	RXfifo = create_fifo(200, PLUTOBUFSIZE*4);
	TXfifo = create_fifo(200, PLUTOBUFSIZE*4);
	FFTfifo = create_fifo(200, PLUTOBUFSIZE*4);

	// start audio (soundcard) and get connected devices
	if(kmaudio_init() == -1)
	{
		printf("NO AUDIO device\n");
		exit(0);
	}
	kmaudio_getDeviceList();

	// UDP receiver for commands from GUI
	UdpRxInit(&udprxsock, 40821, udprxfunc , &keeprunning);

	// send audio devices to GUI
	int len;
	uint8_t *s = io_getAudioDevicelist(&len);
	uint8_t ub[len+1];
	ub[0] = 4; // ID for sound device string
	memcpy(ub+1,s,len);
	sendUDP(gui_ip, GUI_UDPPORT, ub, len+1);

	// wait for initial configuration from GUI
	// the GUI sends now:
	// selected Audio Device (udp code 7)
	// Base QRGs (udp code 8)
	// Pluto Address (udp code 10)
	int to=0;
	while(gotPlutoID == 0)
	{
		printf("to:%d\n",to);
		usleep(100000);
		if(++to >= 50)
		{
			printf("no config from GUI, exit program\n");
			exit(0);
		}
	}
	printf("initial config OK, continue\n");

	// find plutoid
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
	pluto_setup();

	// init DSP demodulator
	init_liquid();
	// init DSP modulator
	init_liquid_modulator();
	
	// pluto RX/TX is now running, samples are available in the fifos
	// start the SSB receiver and transmitter
	init_fft();
	init_rx();
	init_tx();

	printf("initialisation finished. Enter normal operation (press Ctrl+C to cancel)\n");
	while(keeprunning)
	{
		if(newaudiodevs)
		{
			if(*pbdevname && *capdevname)
			{
				// audio device names available
				printf("close streams\n");
				if(pbidx!=-1) close_stream(pbidx);
				if(capidx!=-1) close_stream(capidx);
				printf("init streams\n");

				pbidx = kmaudio_startPlayback(pbdevname, 48000);
				if(pbidx == -1)
					printf("NO AUDIO play device: <%s>\n",pbdevname);

				capidx = kmaudio_startCapture(capdevname, 48000);
				if(capidx == -1)
					printf("NO AUDIO record device: <%s>\n",capdevname);

				newaudiodevs = 0;
			}
		}

		usleep(100);
	}
 
	return 0;
}
