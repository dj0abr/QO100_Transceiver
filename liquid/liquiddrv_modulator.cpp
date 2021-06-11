#include "../qo100trx.h"

void tune_upmixer(int offset);
void sendToPluto();
void createBandpass();
void measureTxAudioVolume(float f);

// Modulator
float mod_index  = 0.99f;                // modulation index (bandwidth)
ampmodem mod = NULL;

// Up-Sampler
unsigned int interp_h_len = 13;    // filter semi-length (filter delay)
float interp_r = (float)((double)SAMPRATE / (double)AUDIOSAMPRATE); // resampling rate (output/input)
float interp_bw=0.1f;              // cutoff frequency
float interp_slsl= 60.0f;          // resampling filter sidelobe suppression level
unsigned int interp_npfb=32;       // number of filters in bank (timing resolution)
resamp_crcf interp_q = NULL;

// up mixer
nco_crcf upnco = NULL;    
nco_crcf upssb = NULL;    

// ssb band pass
unsigned int tx_lp_order =   8;       // filter order
float        tx_lp_fc    =   0.066f;    // cutoff frequency
float        tx_lp_f0    =   0.09f;    // center frequency
float        tx_lp_Ap    =   1.0f;    // pass-band ripple
float        tx_lp_As    =  80.0f;    // stop-band attenuation
unsigned int tx_lp_n     = 128;       // number of samples
iirfilt_crcf tx_lp_q = NULL;

// audio high pass
unsigned int au_lp_order =   4;       // filter order
float        au_lp_fc    =   0.15f;    // cutoff frequency
float        au_lp_f0    =   0.2f;    // center frequency
float        au_lp_Ap    =   1.0f;    // pass-band ripple
float        au_lp_As    =  20.0f;    // stop-band attenuation
unsigned int au_lp_n     = 128;       // number of samples
iirfilt_crcf au_lp_q = NULL;

// AGC
agc_rrrf agc_q = NULL;

void showmax(float f)
{
static float fs=0; 

    if(f>fs)
    {
        fs = f;
        printf("max:%f\n",fs);
    }
}

liquid_float_complex rc(liquid_float_complex c)
{
    int r =10;
    c.real /=r;
    c.imag /=r;

    return c;
}

void init_liquid_modulator()
{
    // Upmixer
    upnco = nco_crcf_create(LIQUID_NCO);
    tune_upmixer(0);

    // SSB Upmixer Baseband -> 3kHz
    upssb = nco_crcf_create(LIQUID_NCO);
    float RADIANS_PER_SAMPLE   = ((2.0f * (float)M_PI * 3000.0f)/(float)AUDIOSAMPRATE);
    nco_crcf_set_phase(upssb, 0.0f);
    nco_crcf_set_frequency(upssb, RADIANS_PER_SAMPLE);

    // SSB Mmodulator
    mod   = ampmodem_create(mod_index, LIQUID_AMPMODEM_USB, 1);

    // create upsampler
    interp_q = resamp_crcf_create(interp_r,interp_h_len,interp_bw,interp_slsl,interp_npfb);
    if(interp_q == NULL) printf("interp_q error\n");

    // band pass
    createBandpass();

    // create audio filter
    au_lp_q = iirfilt_crcf_create_prototype(LIQUID_IIRDES_BESSEL, LIQUID_IIRDES_HIGHPASS, LIQUID_IIRDES_SOS,
                                         au_lp_order, au_lp_fc, au_lp_f0, au_lp_Ap, au_lp_As);

    // agc
    agc_q = agc_rrrf_create();     
    agc_rrrf_set_bandwidth(agc_q,0.01f);    // set loop filter bandwidth
}

void close_liquid_modulator()
{
    printf("close DSP modulator\n");

    if(upnco) nco_crcf_destroy(upnco);
    upnco = NULL;

    if(upssb) nco_crcf_destroy(upssb);
    upssb = NULL;

    if(mod) ampmodem_destroy(mod);
    mod = NULL;

    if(interp_q) resamp_crcf_destroy(interp_q);
    interp_q = NULL;

    if(tx_lp_q) iirfilt_crcf_destroy(tx_lp_q);
    tx_lp_q = NULL;

    if(au_lp_q) iirfilt_crcf_destroy(au_lp_q);
    au_lp_q = NULL;

    if(agc_q) agc_rrrf_destroy(agc_q);
    agc_q = NULL;
}

void createBandpass()
{
static int lasttxfilter = -1;

    if(txfilter != lasttxfilter)
    {
        lasttxfilter = txfilter;
        if(tx_lp_q) iirfilt_crcf_destroy(tx_lp_q);

        // Frequencies are designed for 3kHz carrier
        switch(txfilter)
        {
            case 0 : {tx_lp_fc=0.079f; tx_lp_f0=0.09f;} break;
            case 1 : {tx_lp_fc=0.074f; tx_lp_f0=0.09f;} break;
            case 2 : {tx_lp_fc=0.070f; tx_lp_f0=0.09f;} break;
            case 3 : {tx_lp_fc=0.066f; tx_lp_f0=0.09f;} break;
        }

        tx_lp_q = iirfilt_crcf_create_prototype(LIQUID_IIRDES_ELLIP, LIQUID_IIRDES_BANDPASS, LIQUID_IIRDES_SOS,
                                         tx_lp_order, tx_lp_fc, tx_lp_f0, tx_lp_Ap, tx_lp_As);
    }
}

void tune_upmixer(int offset)
{
static int lastoffset = -1;

    if(rfloop)
    {
        offset = RXoffsetfreq;
    }

    if(lastoffset != offset)
    {
        lastoffset = offset;
        printf("tune TX to %f\n",BASEQRG*1e3 + offset);
        float RADIANS_PER_SAMPLE   = ((2.0f * (float)M_PI * (offset-280000-3000))/(float)SAMPRATE);
        nco_crcf_set_phase(upnco, 0.0f);
        nco_crcf_set_frequency(upnco, RADIANS_PER_SAMPLE);
    }
}

liquid_float_complex txarr[PLUTOBUFSIZE];
int txarridx = 0;

// it looks like the AGC functions do not work with 0..1 levels
// so we need to adapt the levels
const float agcmult = 100.0f;
const float gainreduction = 1000.0f;
// the max gain is used to limit the background noise if no signal is present
const float maxgain = 0.6f;

void upmix(float *f, int len, int offsetfreq)
{
float fcompr;
float gain = 1;

    if (mod == NULL) return;
    if (interp_q == NULL) return;
    if (upnco == NULL) return;
    if (upssb == NULL) return;
    if (tx_lp_q == NULL) return;
    if (au_lp_q == NULL) return;
    if (agc_q == NULL) return;

    // re-tune if TX grq has been changed
    tune_upmixer(offsetfreq);

    // re-set bandpass if changed by user
    createBandpass();

    if(audioagc > 0)
    {
        // estimate AGC level for this number of new samples
        agc_rrrf_init(agc_q, f, len);
        gain = agc_rrrf_get_gain(agc_q);
        gain /= gainreduction;
        if(gain > maxgain) gain = maxgain;
        //printf("%f\n",gain);
    }

    // loop through all samples
    for(int i=0; i<len; i++)
    {
        if(audioagc > 0)
        {
            // audio AGC
            float fagc;
            agc_rrrf_set_gain(agc_q,gain);
            agc_rrrf_execute(agc_q, f[i] *agcmult, &fagc);
            //printf("%f   %f   %f\n",gain,f[i],fagc);
            fcompr = fagc;
        }
        else
            fcompr = f[i];

        // audio compression
        if(compressor > 0)
        {
            if(fcompr >= 1) fcompr = 0.99;
            if(fcompr <= -1) fcompr = -0.99;
            for(int co=0; co<compressor; co++)
            {
                fcompr = 3.3 * log10(fcompr+1);
                if(fcompr >= 1) fcompr = 0.99;
                if(fcompr <= -1) fcompr = -0.99;
            }
        }

        // modulator, at 48k audio sample rate
        liquid_float_complex ybase;
        ampmodem_modulate(mod, fcompr, &ybase);

        // up mix from baseband to 3000 Hz
        // this eliminates a couple of problems happening at baseband
        liquid_float_complex y;
        nco_crcf_step(upssb);
        nco_crcf_mix_up(upssb,ybase,&y);

        // filter SSB bandwidth at 3kHz
        liquid_float_complex cfilt;
        iirfilt_crcf_execute(tx_lp_q, y, &cfilt);

        // audio high pass filter
        liquid_float_complex aufiltout;
        if(audiohighpass)
        {
            iirfilt_crcf_execute(au_lp_q, cfilt, &aufiltout);
            aufiltout.real *= 3;
            aufiltout.imag *= 3;
        }
        else
            aufiltout = cfilt;

        // resample from AUDIOSAMPRATE (48000S/s) to pluto rate
        liquid_float_complex out[(int)interp_r+2];
        unsigned int num_written;
        resamp_crcf_execute(interp_q, aufiltout, out, &num_written);
        if(num_written <= 0)
        {
            printf("mod num_written error: %d\n",num_written);
            return;
        }
        if(num_written >= ((unsigned int)interp_r+2))
        {
            printf("out array too small: %d\n",num_written);
            return;
        }
        // num_written is the number of samples for Pluto
        for(unsigned int samp=0; samp<num_written; samp++)
        {
            // up mix to SSB channel into baseband
            nco_crcf_step(upnco);
            nco_crcf_mix_up(upnco,out[samp],&(txarr[txarridx]));
            
            // collect until we have a complete buffer filled
            if(++txarridx >= PLUTOBUFSIZE)
            {
                sendToPluto();
                txarridx = 0;
            }
            
        }

    }
}

void sendToPluto()
{
int32_t xi;
int32_t xq;
uint8_t txbuf[PLUTOBUFSIZE * 4];
int txbufidx = 0;

    for(int i=0; i<PLUTOBUFSIZE; i++)
    {
        // convert complex to pluto format
        xi = (int32_t)(txarr[i].real * 32768.0f);
        xq = (int32_t)(txarr[i].imag * 32768.0f);

        txbuf[txbufidx++] = xi & 0xff;
        txbuf[txbufidx++] = xi >> 8;
        txbuf[txbufidx++] = xq & 0xff;
        txbuf[txbufidx++] = xq >> 8;
    }

    write_fifo(TXfifo,txbuf,PLUTOBUFSIZE*4);
}

