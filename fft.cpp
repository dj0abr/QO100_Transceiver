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


fftw_complex *din = NULL;				// input data for  fft, output data from ifft
fftw_complex *cpout = NULL;	            // ouput data from fft, input data to ifft
fftw_plan plan = NULL;

#define FFT_RESOLUTION  25             // Hz per bin
#define FFT_LENGTH (SAMPRATE / FFT_RESOLUTION)  // 560kS/s / 25 = 22400

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

// convert a frequency offset (starting at ,470 and the kHz part only) to the bin index
int FreqToBinIdx(int freq)
{
    // bins: 0.. 22400
    // freq: 0..560000
    return freq * 224 / 5600;
}

void calc_fft(uint8_t *data, int len)
{
static int din_idx = 0;
double real, imag;

    // values for small WF, calculate only once per loop
    int span = FFT_RESOLUTION*1120;   // size of small waterfall in kHz (28kHz, +/-14kHz)
    int start = FreqToBinIdx(RXoffsetfreq - span/2);
    int end = FreqToBinIdx(RXoffsetfreq + span/2);

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
            // the FFT delivers FFT_LENGTH values (44800 bins)
            // but we only use the first half of them, which is the full range with the 
            // requested resolution of 25 Hz per bin (22400 bins)
            int numbins = FFT_LENGTH; // 22400 (22400*25=560k)
            // the FFT generates the upper half folloed by the lower half
            // the tuned freq is in the center

            // if fftspeed > 0 then ignore every n fft line
            // this helps to run this software on slow SBCs
            static int fdel = 0;
    
            if(fdel++ < fftspeed) continue;
            fdel = 0;

            // range centerfreq(,750) - 560k/2 to +560k/2
            // is ,470 ... 1,030

            float bin[numbins];
            int binidx = 0;
            // lower half
            for(int i=numbins/2; i<numbins; i++)
            {
                real = cpout[i][0];
                imag = cpout[i][1];
                // calc magnitude
                double v = sqrt((real * real) + (imag * imag));
                // convert to dB scale
                if(v <=0) v=0.01;
                bin[binidx] = log(v);
                bin[binidx] *= 1000;
                if(bin[binidx] > 32768) printf("reduce multiplicator %f\n",bin[binidx]);
                binidx++;
            }
            // upper half
            for(int i=0; i<numbins/2; i++)
            {
                real = cpout[i][0];
                imag = cpout[i][1];
                // calc magnitude
                double v = sqrt((real * real) + (imag * imag));
                // convert to dB scale
                if(v <=0) v=0.01;
                bin[binidx] = log(v);
                bin[binidx] *= 1000;
                if(bin[binidx] > 32768) printf("reduce multiplicator %f\n",bin[binidx]);
                binidx++;
            }
            // bins are in the range 0..32767 (16 bit)

            // ======== noise level ==========
            // measure the noise level on freq: ,475 to ,490 MHz, just below the lower beacon
            // ,470 is index 0, steps 25 Hz
            // ,475 is index 200 and ,490 is index 800
            // so we calc the mid value if this range
            float gval = 0;
            int gstart = FreqToBinIdx(485000 - 470000);
            int gend = FreqToBinIdx(495000 - 470000);
            for(int g=gstart; g<gend; g++)
                gval += bin[g];
            gval /= (gend-gstart);

            // bring down a little, looks nicer
            gval = gval * 100 / 99;

            // and make the mid value over 10 values
            #define GMIDLEN 10
            static float gmid[GMIDLEN];
            static int gmididx = 0;
            gmid[gmididx] = gval;
            if(++gmididx >= GMIDLEN) gmididx = 0;
            float gvalmid = 0;
            for(int g=0; g<GMIDLEN; g++)
                gvalmid += gmid[g];
            gvalmid /= GMIDLEN;
            uint32_t gvalmid_u = (uint32_t)gvalmid;
            //printf("noise level: %f\n",gvalmid);

            // ======== beacon level ==========
            // measure the max beacon level of BPSK beacon
            // max of 749 - 751
            float mval = 0;
            int mstart = FreqToBinIdx(749000 - 470000);
            int mend = FreqToBinIdx(751000 - 470000);
            for(int m=mstart; m<mend; m++)
                if(bin[m] > mval) mval = bin[m];

            // and make the mid value over 10 values
            #define MMIDLEN 10
            static float mmid[MMIDLEN];
            static int mmididx = 0;
            mmid[mmididx] = mval;
            if(++mmididx >= MMIDLEN) mmididx = 0;
            float mvalmid = 0;
            for(int m=0; m<MMIDLEN; m++)
                mvalmid += mmid[m];
            mvalmid /= MMIDLEN;
            uint32_t mvalmid_u = (uint32_t)mvalmid;
            //printf("max level: %f %f\n",mval,mvalmid);

            uint8_t levels[9];
            levels[0] = 5;
            levels[1] = gvalmid_u >> 24;
            levels[2] = gvalmid_u >> 16;
            levels[3] = gvalmid_u >> 8;
            levels[4] = gvalmid_u & 0xff;
            levels[5] = mvalmid_u >> 24;
            levels[6] = mvalmid_u >> 16;
            levels[7] = mvalmid_u >> 8;
            levels[8] = mvalmid_u & 0xff;
            
            sendUDP(gui_ip, GUI_UDPPORT, levels, 9);

            // ======== BIG Waterfall ========
            // the big waterfall has a screen resulution of 1120 pixel
            // FFT_RESOLUTION=25 Hz/bin
            // the FFT delivers 1,12MS/s / 25 / 2 = 22400 values
            // so the BIG WF resolution is 22400 / 1120 = 20
            static const int bigres = numbins / 1120;
            static const int midlen = 15;
            static uint32_t bigmid[midlen][FFT_LENGTH];
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
            // raw values, no mid val, for waterfall
            sendUDP(gui_ip, GUI_UDPPORT, biglineraw, bigidx);

            // send the big fft bins to the GUI
            // mid vals for spectrum
            sendUDP(gui_ip, GUI_UDPPORT, bigline, bigidx);

            // ======== SMALL Waterfall ========
            // the small waterfall is  a piece of some kHz around the mid RX frequency "RXoffsetfreq"
            // RXoffsetfreq is in kHz above the left margin which is 750-280= 470k
            // the bin-index of RXoffsetfreq is: RXoffsetfreq/10

            static uint32_t smallmid[midlen][FFT_LENGTH];
            static int smididx = 0;
            for(int i=start; i<end; i++)
            {
                if(i>=0 && i<FFT_LENGTH)
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
                if(i>=0 && i<FFT_LENGTH)
                {
                    for(int j=0; j<midlen; j++)
                        uv = uv + smallmid[j][i]+smallmid[j][i-1]+smallmid[j][i+1];
                    uv /= (midlen*3);

                    if((smallidx+1) >= (int)sizeof(smalllineraw)) printf("*********************** %d  %d  %d\n",smallidx, start,end);

                    smalllineraw[smallidx] = smallmid[smididx][i] >> 8;
                    smalllineraw[smallidx+1] = smallmid[smididx][i] & 0xff;

                    smallline[smallidx] = uv >> 8;
                    smallline[smallidx+1] = uv & 0xff;
                }
                else
                {
                    smalllineraw[smallidx] = 0;
                    smalllineraw[smallidx+1] = 0;

                    smallline[smallidx] = 0;
                    smallline[smallidx+1] = 0;
                }

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