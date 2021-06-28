#include "../qo100trx.h"
#include <fftw3.h>

void createSSBfilter();
void init_beaconlock();
void close_beaconlock();
void exec_beaconlock(liquid_float_complex samp);

// down mixer
nco_crcf dnnco = NULL;      

// SSB Demodulator
float demod_index  = 0.99f;                // modulation index (bandwidth)
ampmodem demod = NULL;

// Low pass
unsigned int lp_order =   4;       // filter order
float        lp_fc    =   0.0050f; // cutoff frequency
float        lp_f0    =   0.0f;    // center frequency
float        lp_Ap    =   1.0f;    // pass-band ripple
float        lp_As    =  40.0f;    // stop-band attenuation
iirfilt_crcf lp_q = NULL;

// Down-Sampler
unsigned int decim_h_len = 13;    // filter semi-length (filter delay)
float decim_r = (float)((double)AUDIOSAMPRATE / (double)SAMPRATE); // resampling rate (output/input)
float decim_bw=0.001f;              // cutoff frequency
float decim_slsl= 60.0f;          // resampling filter sidelobe suppression level
unsigned int decim_npfb=32;       // number of filters in bank (timing resolution)
resamp_crcf decim_q = NULL;

void init_liquid()
{
    printf("init DSP\n");

    // Downmixer
    dnnco = nco_crcf_create(LIQUID_NCO);
    tune_downmixer();

    // SSB Demodulator
    demod = ampmodem_create(demod_index, LIQUID_AMPMODEM_USB, 1);

    // create downsampler
    decim_q = resamp_crcf_create(decim_r,decim_h_len,decim_bw,decim_slsl,decim_npfb);
    if(decim_q == NULL) printf("decimq error\n");

    // low pass
    createSSBfilter();

    init_beaconlock();
}

void close_liquid()
{
    printf("close DSP\n");

    if(dnnco) nco_crcf_destroy(dnnco);
    dnnco = NULL;

    if(demod) ampmodem_destroy(demod);
    demod = NULL;

    if(decim_q) resamp_crcf_destroy(decim_q);
    decim_q = NULL;

    if(lp_q) iirfilt_crcf_destroy(lp_q);
    lp_q = NULL;

    close_beaconlock();
}

void createSSBfilter()
{
static int lastrxfilter = -1;

    if(rxfilter != lastrxfilter)
    {
        printf("create RX SSB filter: %d\n",rxfilter);
        lastrxfilter = rxfilter;
        if(lp_q) iirfilt_crcf_destroy(lp_q);

        switch(rxfilter)
        {
            case 0: lp_fc    =   0.002f; break;
            case 1: lp_fc    =   0.0036f; break;
            case 2: lp_fc    =   0.0044f; break;
            case 3: lp_fc    =   0.0050f; break;
        }


        lp_q = iirfilt_crcf_create_prototype(LIQUID_IIRDES_ELLIP, LIQUID_IIRDES_LOWPASS, LIQUID_IIRDES_SOS,
                                         lp_order, lp_fc, lp_f0, lp_Ap, lp_As);
    }
}

void tune_downmixer()
{
static int lastoffset = -1;

    int newoffset = RXoffsetfreq;
    if(beaconlock) 
        newoffset += bcnoffset;

    if(lastoffset != newoffset)
    {
        lastoffset = newoffset;
        printf("tune RX to %f\n",BASEQRG*1e3 + lastoffset);
        float RADIANS_PER_SAMPLE   = ((2.0f * (float)M_PI * (lastoffset-280000))/(float)SAMPRATE);
        nco_crcf_set_phase(dnnco, 0.0f);
        nco_crcf_set_frequency(dnnco, RADIANS_PER_SAMPLE);
    }
}

// convert a pluto stream into liquid samples
void streamToSamples(uint8_t *stream, int streamlen, liquid_float_complex *samples)
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

        xi <<= 4;
        xq <<= 4;

        samples[didx].real = (float)xi / 32768.0f;
        samples[didx].imag = (float)xq / 32768.0f;

        didx++;
    }
}


void downmix(liquid_float_complex *samples, int len)
{
    if (dnnco == NULL) return;
    if (demod == NULL) return;
    if (decim_q == NULL) return;
    if (lp_q == NULL) return;

    // re-tune if RX grq has been changed
    tune_downmixer();

    // re-set RX filter if it was changed by the user
    createSSBfilter();

    // for each sample
    liquid_float_complex c, cfilt;
    liquid_float_complex soundsamp;
    float z;
    unsigned int num_written;

    for(int i=0; i<len; i++)
    {
        exec_beaconlock(samples[i]);

        // down mix SSB channel into baseband
        nco_crcf_step(dnnco);
        nco_crcf_mix_down(dnnco,samples[i],&c);

        // SSB Filter
        iirfilt_crcf_execute(lp_q, c, &cfilt);

        // resample to 48000S/s for the soundcard
        resamp_crcf_execute(decim_q, cfilt, &soundsamp, &num_written);
        if(num_written == 0) continue;
        if(num_written > 1)
        {
            printf("decimator error, num_written=%d\n",num_written);
            exit(0);
        }

        // the rate here is 48000S/s
        // demodulate
        ampmodem_demodulate(demod, soundsamp, &z);

        // send z to soundcard
        if(audioloop == 0 && pbidx != -1)
        {
            static float playvol = 1.0f;
            if(ptt && rxmute && rfloop == 0) playvol = 0.08f;
            else playvol = 2.0f;
            kmaudio_playsamples(pbidx,&z,1,playvol);
        }
    }
}

/* ================= beaconlock ===================
the beacon lock measures the frequency of the lower CW beacon

1) Frequency
tuner is on x,470000
beacon is on x,500000 - x,500400
take 2kHz from x,499200 to x,501200
Downmix x,499200 to baseband

2) Low pass filter with < 2kHz to eliminate aliasing

3) Sample Rate: Downsampling by 480
input:  1,12MS/s
output: 4000S/s which gives the required 2kHz range 

4) run an FFT with a resolution of bcn_resolution.
this give new FFT bins every 1/bcn_resolution seconds
*/

// FFT
fftw_complex *bcn_din = NULL;				// input data for  fft, output data from ifft
fftw_complex *bcn_cpout = NULL;	            // ouput data from fft, input data to ifft
fftw_plan bcn_plan = NULL;
const int bcn_FFTsamprate = 4000;           // sample rate for FFT (required range * 2)
const int bcn_resolution = 2;               // Hz per bin
const int bcn_fftlength = bcn_FFTsamprate / bcn_resolution; // 4kS/s / 5 = 800

// down mixer
nco_crcf bcn_dnnco = NULL;    
const int startqrg = 499200;  

// Low pass
unsigned int bcn_lp_order =   4;       // filter order
float        bcn_lp_fc    =   0.0018f; // cutoff frequency
float        bcn_lp_f0    =   0.0f;    // center frequency
float        bcn_lp_Ap    =   1.0f;    // pass-band ripple
float        bcn_lp_As    =  40.0f;    // stop-band attenuation
iirfilt_crcf bcn_lp_q = NULL;

// decimator
unsigned int bcn_m_predec = 8;  // filter delay
float bcn_As_predec = 40.0f;    // stop-band att 
const int bcnInterpolfactor = SAMPRATE / bcn_FFTsamprate;
firdecim_crcf bcn_decim = NULL;

int bcnoffset = -1;

void init_beaconlock()
{
    // Downmixer
    bcn_dnnco = nco_crcf_create(LIQUID_NCO);
    int offset = startqrg - 470000;
    float RADIANS_PER_SAMPLE   = ((2.0f * (float)M_PI * (offset-280000))/(float)SAMPRATE);
    nco_crcf_set_phase(bcn_dnnco, 0.0f);
    nco_crcf_set_frequency(bcn_dnnco, RADIANS_PER_SAMPLE);

    // Low pass filter
    bcn_lp_q = iirfilt_crcf_create_prototype(LIQUID_IIRDES_ELLIP, LIQUID_IIRDES_LOWPASS, LIQUID_IIRDES_SOS,
                                         bcn_lp_order, bcn_lp_fc, bcn_lp_f0, bcn_lp_Ap, bcn_lp_As);

    // decimator
    bcn_decim = firdecim_crcf_create_kaiser(bcnInterpolfactor, bcn_m_predec, bcn_As_predec);
    firdecim_crcf_set_scale(bcn_decim, 1.0f/(float)bcnInterpolfactor);

    // FFT
    fftw_import_wisdom_from_filename("bcn_fftcfg");
    bcn_din   = (fftw_complex *)fftw_malloc(sizeof(fftw_complex) * bcn_fftlength);
	bcn_cpout = (fftw_complex *)fftw_malloc(sizeof(fftw_complex) * bcn_fftlength);
    bcn_plan = fftw_plan_dft_1d(bcn_fftlength, bcn_din, bcn_cpout, FFTW_FORWARD, FFTW_MEASURE);
    fftw_export_wisdom_to_filename("bcn_fftcfg");
}

void close_beaconlock()
{
    if(bcn_dnnco) nco_crcf_destroy(bcn_dnnco);
    bcn_dnnco = NULL;

    if(bcn_lp_q) iirfilt_crcf_destroy(bcn_lp_q);
    bcn_lp_q = NULL;

    if(bcn_decim != NULL) firdecim_crcf_destroy(bcn_decim);
    bcn_decim = NULL;

    if(bcn_din) fftw_free(bcn_din);
    bcn_din = NULL;

    if(bcn_cpout) fftw_free(bcn_cpout);
    bcn_cpout = NULL;

}

void exec_beaconlock(liquid_float_complex samp)
{
static liquid_float_complex ccol[bcnInterpolfactor];
static int ccol_idx = 0;
static int bcn_din_idx = 0;

    if(!beaconlock) return;

    if (bcn_dnnco == NULL) return;
    if (bcn_lp_q == NULL) return;
    if (bcn_decim == NULL) return;

    // mix lower beacon into baseband
    liquid_float_complex c;
    nco_crcf_step(bcn_dnnco);
    nco_crcf_mix_down(bcn_dnnco,samp,&c);

    // Filter
    liquid_float_complex cfilt;
    iirfilt_crcf_execute(bcn_lp_q, c, &cfilt);

    // down sampling 1,2MS/s -> 4kS/s
    ccol[ccol_idx++] = cfilt;
    if (ccol_idx < bcnInterpolfactor) return;
    ccol_idx = 0;

    // we have bcnInterpolfactor samples in ccol
    liquid_float_complex y;
    firdecim_crcf_execute(bcn_decim, ccol, &y);
    // the output of the pre decimator is exactly one sample in y
    // the rate here is 4kS/s

    // FFT
    // collect samples until we have bcn_fftlength
    bcn_din[bcn_din_idx][0] = y.real;
    bcn_din[bcn_din_idx][1] = y.imag;
    if(++bcn_din_idx < bcn_fftlength) return;
    bcn_din_idx = 0;

    fftw_execute(bcn_plan);
    int numbins = bcn_fftlength/2;

    // calc absolute values and search max value
    float bin[numbins];
    float real,imag;
    float max = 0;
    for(int i=0; i<numbins; i++)
    {
        real = bcn_cpout[i][0];
        imag = bcn_cpout[i][1];
        bin[i] = sqrt((real * real) + (imag * imag));
        if(bin[i]>max) max=bin[i];
    }

    // from all samples > 1/2 max search the min and max frequency
    int minf=99999, maxf=0;
    for(int i=0; i<numbins; i++)
    {
        if(bin[i] > (max*1/2))
        {
            if(i < minf) minf = i;
            if(i > maxf) maxf = i;
        }
    }

    int minqrg = minf* bcn_resolution+startqrg;
    int maxqrg = maxf* bcn_resolution+startqrg;
    int diff = abs(maxqrg-minqrg);
    //printf("%d  %d  %d\n",minqrg,maxqrg,diff);

    int newoffset = 0;
    if(diff > 380) 
    {
        // we have both frequencies, then measure the beacon mir frequency
        int bcnqrg = minqrg + diff/2;
        int bcnqrgsoll = 500200;    // expected frequency
        bcnoffset = (bcnqrg - bcnqrgsoll);
        //printf("lower beacon %d .. %d: mid QRG: %d kHz. Offset: %d Hz\n",minqrg,maxqrg,bcnqrg,bcnoffset);
        newoffset = 1;
    }
    else
    {
        // we have only one frequency

        return;


        int difflow = minqrg - 500000;
        int diffhigh = maxqrg - 500400;
        if(abs(difflow) < abs(diffhigh))
        {
            //printf("lower beacon low QRG: %d kHz. Offset: %d Hz\n",minqrg,difflow);
            bcnoffset = difflow;
            newoffset = 1;
        }
        else
        {
            //printf("lower beacon hi  QRG: %d kHz. Offset: %d Hz\n",maxqrg,diffhigh);
            bcnoffset = diffhigh;
            newoffset = 1;
        }
    }

    // send offset to GUI
    if(newoffset)
    {
        uint8_t drift[5];
        drift[0] = 6;
        drift[1] = bcnoffset >> 24;
        drift[2] = bcnoffset >> 16;
        drift[3] = bcnoffset >> 8;
        drift[4] = bcnoffset & 0xff;
        
        sendUDP(gui_ip, GUI_UDPPORT, drift, 5);

        tune_downmixer();
    }
}

