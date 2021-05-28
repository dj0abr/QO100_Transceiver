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
* FFT for Spectrum/Waterfall
* =========================
* 
*/

#include "qo100trx.h"
#include <fftw3.h>

void fftinit();
void calc_fft(uint8_t *data, int len);

void* fft_threadfunction(void* param);

void init_fft()
{
    fftinit();

	pthread_t fftthread;
	pthread_create(&fftthread, NULL, fft_threadfunction, NULL);
}

void* fft_threadfunction(void* param) 
{
int ph = 0;

    pthread_detach(pthread_self());

    printf("entering FFT loop\n");
	while(keeprunning)
	{
		uint8_t data[PLUTOBUFSIZE*4];
		if(read_fifo(FFTfifo, data, PLUTOBUFSIZE*4))
		{
            if(++ph >= 2) // uncomment to reduce cpu load
            {
                ph=0;
                calc_fft(data, PLUTOBUFSIZE*4);
            }
        }

        usleep(1000);
	}
    printf("exit FFT loop\n");
    pthread_exit(NULL); // self terminate this thread
    return NULL;
}

/*
With the samplerate pf 1.12 MS/s we have a spectrum of 560kHz
the FFT is calculated with a resolution of 10 Hz = 56kbins
*/

fftw_complex *din = NULL;				// input data for  fft, output data from ifft
fftw_complex *cpout = NULL;	            // ouput data from fft, input data to ifft
fftw_plan plan = NULL;

#define FFT_RESOLUTION  25             // Hz per bin
#define FFT_LENGTH (SAMPRATE / FFT_RESOLUTION)

void fftinit()
{

    int numofcpus = sysconf(_SC_NPROCESSORS_ONLN); // Get the number of logical CPUs.
    if(numofcpus > 1)
    {
        printf("found %d cores, running FFT in multithreading mode\n",numofcpus);
        fftw_init_threads();
        fftw_plan_with_nthreads(numofcpus);
    }

    fftw_import_wisdom_from_filename("fftcfg");

    din   = (fftw_complex *)fftw_malloc(sizeof(fftw_complex) * FFT_LENGTH);
	cpout = (fftw_complex *)fftw_malloc(sizeof(fftw_complex) * FFT_LENGTH);

    plan = fftw_plan_dft_1d(FFT_LENGTH, din, cpout, FFTW_FORWARD, FFTW_MEASURE);

    fftw_export_wisdom_to_filename("fftcfg");
}

void close_fft()
{
    if(din) fftw_free(din);
    din = NULL;

    if(cpout) fftw_free(cpout);
    cpout = NULL;
}

// convert a pluto stream into fftw3 samples
void streamToFftSamples(uint8_t *stream, int streamlen, liquid_float_complex *samples)
{
int didx = 0;
int16_t xi;
int16_t xq;

    for(int i=0; i<streamlen; i+=4)
    {
        xi = stream[i+1];
        xi <<= 8;
        xi += stream[i];
        
        xq = stream[i+3];
        xq <<= 8;
        xq += stream[i+2];

        samples[didx].real = (float)xi / 32768.0f;
        samples[didx].imag = (float)xq / 32768.0f;

        didx++;
    }
}

void calc_fft(uint8_t *data, int len)
{
static int din_idx = 0;
double real, imag;

    for(int i=0; i<len; i+=4)
    {
        // convert Pluto samples to fftw3 complex samples
        int16_t xi = data[i+1];
        xi <<= 8;
        xi += data[i];
        
        int16_t xq = data[i+3];
        xq <<= 8;
        xq += data[i+2];

        din[din_idx][0] = (double)xi;
		din[din_idx][1] = (double)xq;
        din_idx++;

        if(din_idx >= FFT_LENGTH)
        {

            din_idx = 0;
            // the fft input buffer is full, now lets execute the fft
            fftw_execute(plan);
            // the FFT delivers FFT_LENGTH values (11200 bins)
            // but we only use the first half of them, which is the full range with the 
            // requested resolution of 10 Hz per bin (5600 bins)
            int numbins = FFT_LENGTH/2;

            // calculate the absolute level from I and Q values
            float bin[numbins];
            for(int i=0; i<numbins; i++)
            {
                real = cpout[i][0];
                imag = cpout[i][1];
                // calc magnitude
                double v = sqrt((real * real) + (imag * imag));
                // convert to dB scale
                if(v <=0) v=0.01;
                bin[i] = log(v);
                bin[i] *= 1000;
                if(bin[i] > 32768) printf("reduce multiplicator %f\n",bin[i]);
            }
            // bins are in the range 0..32767 (16 bit)

            // ======== BIG Waterfall ========
            // the big waterfall has a resulution of numbins/5 = 1120 pixel
            // take the maximum value of 5 "bigres" bins
            // and make a mid value with length "midlen" to reduce flickering
            static const int bigres = 500/FFT_RESOLUTION;
            static const int midlen = 15;
            static uint32_t bigmid[midlen][FFT_LENGTH/2];
            static int bmididx = 0;

            int didx = 0;
            for(int i=0; i<numbins; i+=bigres)
            {
                float max = 0;
                for(int j=0; j<bigres; j++)
                {
                    float fbin = bin[i+j];
                    if(fbin > max) max = fbin;
                }
                bigmid[bmididx][didx++] = (uint32_t)max;
                if(max > 32768) printf("16bit overflow %f\n",max);
            }
            
            uint8_t bigline[1 + 2 * (numbins/bigres)];
            uint8_t biglineraw[1 + 2 * (numbins/bigres)];
            int bigidx = 0;
            bigline[bigidx] = 0;  // ID for big spectrum
            biglineraw[bigidx] = 2;  // ID for big waterfall
            bigidx++;
            for(int i=0; i<(numbins/bigres); i++)
            {
                uint32_t uv = 0;
                for(int j=0; j<midlen; j++)
                    uv += bigmid[j][i];
                uv /= midlen;

                if((bmididx+1) >= (int)sizeof(biglineraw)) 
                    printf("+++++++++++++++++++++++ %d  %d\n",bmididx, numbins/bigres);

                biglineraw[bigidx] = bigmid[bmididx][i] >> 8;
                biglineraw[bigidx+1] = bigmid[bmididx][i] & 0xff;

                bigline[bigidx] = uv >> 8;
                bigline[bigidx+1] = uv & 0xff;

                bigidx+=2;
            }

            if(++bmididx >= midlen) bmididx = 0;

            // send the big fft bins to the GUI
            sendUDP(gui_ip, GUI_UDPPORT, biglineraw, bigidx);

            // send the big fft bins to the GUI
            sendUDP(gui_ip, GUI_UDPPORT, bigline, bigidx);

            // ======== SMALL Waterfall ========
            // the small waterfall is  a piece of some kHz around the mid RX frequency "RXoffsetfreq"
            // RXoffsetfreq is in kHz above the tuner qrg
            // the bin-index of RXoffsetfreq is: RXoffsetfreq/10
            int span = FFT_RESOLUTION*1120;   // size of small waterfall in kHz
            int start = RXoffsetfreq/FFT_RESOLUTION - span/2/FFT_RESOLUTION;
            int end = RXoffsetfreq/FFT_RESOLUTION + span/2/FFT_RESOLUTION;
            if(start >= 1 && end < (SAMPRATE/2/FFT_RESOLUTION-1))
            {
                static uint32_t smallmid[midlen][FFT_LENGTH/2];
                static int smididx = 0;
                for(int i=start; i<end; i++)
                {
                    smallmid[smididx][i] =  (uint32_t)bin[i];
                }

                uint8_t smallline[1+(end-start)*2];
                uint8_t smalllineraw[1+(end-start)*2];
                int smallidx = 0;
                smallline[smallidx] = 1;  // ID for small waterfall
                smalllineraw[smallidx] = 3;  // ID for small waterfall
                smallidx++;
                for(int i=start; i<end; i++)
                {
                    uint32_t uv = 0;
                    for(int j=0; j<midlen; j++)
                        uv = uv + smallmid[j][i]+smallmid[j][i-1]+smallmid[j][i+1];
                    uv /= (midlen*3);

                    if((smallidx+1) >= (int)sizeof(smalllineraw)) printf("*********************** %d  %d  %d\n",smallidx, start,end);

                    smalllineraw[smallidx] = smallmid[smididx][i] >> 8;
                    smalllineraw[smallidx+1] = smallmid[smididx][i] & 0xff;

                    smallline[smallidx] = uv >> 8;
                    smallline[smallidx+1] = uv & 0xff;

                    smallidx += 2;
                }

                if(++smididx >= midlen) smididx = 0;

                // send the small fft bins to the GUI
                sendUDP(gui_ip, GUI_UDPPORT, smalllineraw, smallidx);

                // send the small fft bins to the GUI
                sendUDP(gui_ip, GUI_UDPPORT, smallline, smallidx);
            }
        }
    }
}