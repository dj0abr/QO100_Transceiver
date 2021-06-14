
#include "libkmaudio.h"

void getMax(int id, float fv);

void close_stream(int id)
{
#ifdef WIN32
	if (devlist[id].capStream != NULL)
		close_capture_stream(id);
	if (devlist[id].pbStream != NULL)
	close_playback_stream(id);
#else
	if (devlist[id].instream != NULL)
		close_capture_stream(id);
	if (devlist[id].outstream != NULL)
		close_playback_stream(id);
#endif
}

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

int kmaudio_readsamples(int id, float* psamp, int len, float volume, int wait)
{
	int e = io_fifo_elems_avail(id);
	if (e < len) return 0;

	if (devlist[id].requested_samprate == devlist[id].real_samprate)
	{
		// data rate is ok, take samples as is
		int nl = io_read_fifo_num(id, psamp, len);
		for (int i = 0; i < nl; i++)
		{
			psamp[i] *= volume;
			getMax(id, psamp[i]);
			//kmaudio_detectDropouts(id);
		}
		return nl;
	}

	// resampling is required
	int num = io_read_fifo_num(id, psamp, len);
	if (num == 0) return 0;
	
	int newlen = 0;
	float *f = resample(id, psamp, len, &newlen);
	for (int i = 0; i < newlen; i++)
	{
		psamp[i] = f[i] * volume;
		getMax(id, psamp[i]);
	}

	return newlen;
}

/*
* plays len samples from psamp to device id
* returns: 0=ok, -1=error
* id ... device id returned by kmaudio_startPlayback
* psamp ... float array of length len with the audio data (mono)
* len ... number of float values in psamp
* volume ... 0.0f..2.0f will be multiplied with the output sample
*/

int kmaudio_playsamples(int id, float* psamp, int len, float volume)
{
	// check if resampling is required
	//printf("%d %d len:%d\n", devlist[id].requested_samprate , devlist[id].real_samprate,len);

	/* Diagnostic: print buffer usage
	static int xxx=0;
	if(++xxx >= 10000)
	{
		xxx = 0;
		printf("Pluto->sndcard fifo usage: %d\n",io_fifo_usedspace(id));
	}*/

	if (devlist[id].requested_samprate == devlist[id].real_samprate)
	{
		// sampling rate is ok, just play samples
		for (int i = 0; i < len; i++)
		{
			io_write_fifo(id, psamp[i] * volume);
			getMax(id, psamp[i] * volume);
		}
		return 0;
	}
	
	// resampling is required
	int newlen = 0;
	
	float *f = resample(id, psamp, len, &newlen);
	for (int i = 0; i < newlen; i++)
	{
		io_write_fifo(id, f[i] * volume);
		getMax(id, f[i] * volume);
	}

	return 0;
}

#define MCHECK 48000	// abt. 1s of samples
float farr[MAXDEVICES][MCHECK];
int farridx[MAXDEVICES];

void init_maxarray()
{
	// initialize arrays
	for (int md = 0; md < MAXDEVICES; md++)
	{
		farridx[md] = 0;
		for (int i = 0; i < MCHECK; i++)
			farr[md][i] = 0;
	}
}

void getMax(int id, float fv)
{
	// put value into array
	farr[id][farridx[id]] = fv;
	if (++farridx[id] == MCHECK)
		farridx[id] = 0;
}

/*
* returns the max level (within 1 second) of this stream in % (0..100)
* if the level >= 100 the signal will get clipped and distorted
*/

uint8_t kmaudio_maxlevel(int id)
{
	float max = 0;
	for (int i = 0; i < MCHECK; i++)
		if (farr[id][i] > max) max = farr[id][i];

	return (uint8_t)(max * 100);
}

void kmaudio_detectDropouts(int id)
{
	int stat = 0;
	int drlen = 0;

	for (int i = 0; i < MCHECK; i++)
	{
		switch (stat)
		{
		case 0:	// search beginning of dropout
			if (farr[id][i] == 0.0f)
				stat = 1;
			break;

		case 1: // count length of dropout
			if (farr[id][i] == 0.0f) drlen++;
			else 
			{
				// end of dropout
				if (drlen > 0)
				{
					printf("Dropout len:%d\n", drlen);
				}
				drlen = 0;
				stat = 0;
			}
			break;
		}
	}
}