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
* libkmaudio_resampler.cpp ... converts audio streams
* between 44100 and 48000 samples/s in both directions
* uses the libliquid library
*/
#include "libkmaudio.h"

#define MAXRLEN 10000

resamp_crcf q[MAXDEVICES];
float fresamp[MAXDEVICES][MAXRLEN];

unsigned int h_len = 13;		// filter semi-length (filter delay)
float r = 0.9f;					// resampling rate (output/input)
float bw = 0.45f;				// resampling filter bandwidth
float slsl = 60.0f;			// resampling filter sidelobe suppression level
unsigned int npfb = 32;			// number of filters in bank (timing resolution)

void resampler_create(int devidx)
{
	static int f = 1;
	if(f)
	{
		f = 0;
		for (int i = 0; i < MAXDEVICES; i++)
			q[i] = NULL;
	}

	printf("create resampler %d real %d req %d\n", devidx, devlist[devidx].real_samprate, devlist[devidx].requested_samprate);

	if (q[devidx] != NULL) resamp_crcf_destroy(q[devidx]);

	int src_rate = 0;
	int dst_rate = 0;
	if (devlist[devidx].in_out == 0)
	{
		// capture device:
		src_rate = devlist[devidx].real_samprate;
		dst_rate = devlist[devidx].requested_samprate;
	}
	else
	{
		// playback device:
		src_rate = devlist[devidx].requested_samprate;
		dst_rate = devlist[devidx].real_samprate;
	}

	r = (float)dst_rate / (float)src_rate;

	//printf("%f  %f  %f\n", r, (float)dst_rate, (float)src_rate);

	q[devidx] = resamp_crcf_create(r, h_len, bw, slsl, npfb);
}



float* resample(int id, float*psamp, int len, int *pnewlen)
{
	int didx = 0;

	for (int i = 0; i < len; i++)
	{
		unsigned int num_written = 0;
		liquid_float_complex in;
		liquid_float_complex out[10];
		in.real = psamp[i];
		in.imag = 0;

		
		resamp_crcf_execute(q[id], in, out, &num_written);
		
		for (unsigned int r = 0; r < num_written; r++)
		{
			if (didx < MAXRLEN)
				fresamp[id][didx++] = out[r].real;
			else
				printf("MAXRLEN too small\n");
		}
	}

	*pnewlen = didx;

	return fresamp[id];
}
