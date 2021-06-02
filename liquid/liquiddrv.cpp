#include "../qo100trx.h"

void createSSBfilter();

// down mixer
nco_crcf dnnco = NULL;      

// SSB Demodulator
float demod_index  = 0.1f;                // modulation index (bandwidth)
ampmodem demod = NULL;

// Low pass
unsigned int lp_order =   4;       // filter order
float        lp_fc    =   0.0025f;    // cutoff frequency
float        lp_f0    =   0.0f;    // center frequency
float        lp_Ap    =   1.0f;    // pass-band ripple
float        lp_As    =  40.0f;    // stop-band attenuation
unsigned int lp_n     = 128;       // number of samples
iirfilt_crcf lp_q = NULL;

// Down-Sampler
unsigned int decim_h_len = 13;    // filter semi-length (filter delay)
float decim_r = (float)((double)48000 / (double)SAMPRATE); // resampling rate (output/input)
float decim_bw=0.1f;              // cutoff frequency
float decim_slsl= 60.0f;          // resampling filter sidelobe suppression level
unsigned int decim_npfb=32;       // number of filters in bank (timing resolution)
resamp_crcf decim_q = NULL;

void init_liquid()
{
    printf("init DSP\n");

    // Downmixer
    dnnco = nco_crcf_create(LIQUID_NCO);
    tune_downmixer(0);

    // SSB Demodulator
    demod = ampmodem_create(demod_index, LIQUID_AMPMODEM_USB, 1);

    // create downsampler
    decim_q = resamp_crcf_create(decim_r,decim_h_len,decim_bw,decim_slsl,decim_npfb);
    if(decim_q == NULL) printf("decimq error\n");

    // low pass
    createSSBfilter();
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
            case 0: lp_fc    =   0.001f; break;
            case 1: lp_fc    =   0.0018f; break;
            case 2: lp_fc    =   0.0022f; break;
            case 3: lp_fc    =   0.0025f; break;
        }

        lp_q = iirfilt_crcf_create_prototype(LIQUID_IIRDES_ELLIP, LIQUID_IIRDES_LOWPASS, LIQUID_IIRDES_SOS,
                                         lp_order, lp_fc, lp_f0, lp_Ap, lp_As);
    }
}

void tune_downmixer(int offset)
{
static int lastoffset = -1;

    if(lastoffset != offset)
    {
        lastoffset = offset;
        //printf("tune RX to %f\n",BASEQRG*1e6 + offset);
        float RADIANS_PER_SAMPLE   = ((2.0f * (float)M_PI * offset)/(float)SAMPRATE);
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

        samples[didx].real = (float)xi / 32768.0f;
        samples[didx].imag = (float)xq / 32768.0f;

        didx++;
    }
}


void downmix(liquid_float_complex *samples, int len, int offsetfreq)
{
    if (dnnco == NULL) return;
    if (demod == NULL) return;
    if (decim_q == NULL) return;
    if (lp_q == NULL) return;

    // re-tune if RX grq has been changed
    tune_downmixer(offsetfreq);

    // re-set RX filter if it was changed by the user
    createSSBfilter();

    // for each sample
    liquid_float_complex c, cfilt;
    liquid_float_complex soundsamp;
    float z;
    unsigned int num_written;

    for(int i=0; i<len; i++)
    {
         // down mix to SSB channel into baseband
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
            if(ptt && rxmute) playvol = 0.1f;
            else playvol = 1.0f;
            kmaudio_playsamples(pbidx,&z,1,playvol);
        }
    }
}