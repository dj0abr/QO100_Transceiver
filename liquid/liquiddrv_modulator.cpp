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
nco_crcf tone_nco = NULL;

// ssb band pass
unsigned int tx_lp_order =   8;       // filter order
float        tx_lp_fc    =   0.066f;    // cutoff frequency
float        tx_lp_f0    =   0.09f;    // center frequency
float        tx_lp_Ap    =   1.0f;    // pass-band ripple
float        tx_lp_As    =  80.0f;    // stop-band attenuation
unsigned int tx_lp_n     = 128;       // number of samples
iirfilt_crcf tx_lp_q = NULL;

// audio high pass
unsigned int au_lp_order =   2;       // filter order
float        au_lp_fc    =   0.1f;    // cutoff frequency
float        au_lp_f0    =   0.2f;    // center frequency
float        au_lp_Ap    =   1.0f;    // pass-band ripple
float        au_lp_As    =  10.0f;    // stop-band attenuation
unsigned int au_lp_n     = 128;       // number of samples
iirfilt_crcf au_lp_q = NULL;

// AGC
agc_rrrf agc_q = NULL;

int32_t plutomax = 1;

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

    // test tone 800 Hz
    tone_nco = nco_crcf_create(LIQUID_NCO);
    float TT_RADIANS_PER_SAMPLE   = ((2.0f * (float)M_PI * 800.0f)/(float)AUDIOSAMPRATE);
    nco_crcf_set_phase(tone_nco, 0.0f);
    nco_crcf_set_frequency(tone_nco, TT_RADIANS_PER_SAMPLE);

    // SSB Mmodulator
    mod   = ampmodem_create(mod_index, LIQUID_AMPMODEM_USB, 1);

    // create upsampler
    interp_q = resamp_crcf_create(interp_r,interp_h_len,interp_bw,interp_slsl,interp_npfb);
    if(interp_q == NULL) printf("interp_q error\n");

    // band pass
    createBandpass();

    // create audio filter
    // LIQUID_IIRDES_BESSEL has a memory overflow error, only ELLIP works
    au_lp_q = iirfilt_crcf_create_prototype(LIQUID_IIRDES_ELLIP, LIQUID_IIRDES_HIGHPASS, LIQUID_IIRDES_SOS,
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

    if(tone_nco) nco_crcf_destroy(tone_nco);
    tone_nco = NULL;

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


void upmix(float *f, int len, int offsetfreq)
{
float fcompr;

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

    // loop through all samples
    for(int i=0; i<len; i++)
    {
        fcompr = f[i] ;//* 3.0f;

        

        // audio compression
        if(compressor > 0)
        {
            if(fcompr >= 1) fcompr = 0.99;
            if(fcompr <= -1) fcompr = -0.99;
            fcompr = 3.3 * log10(fcompr+1);
            if(fcompr >= 1) fcompr = 0.99;
            if(fcompr <= -1) fcompr = -0.99;
        }

        // insert a test tone
        if(sendtone)
        {
            // generate a 800Hz tone and send to transmitter
            nco_crcf_step(tone_nco);
            fcompr = nco_crcf_sin(tone_nco);
            fcompr /= 10;

        } 

    

        // modulator, at 48k audio sample rate
        liquid_float_complex ybase;
        ampmodem_modulate(mod, fcompr, &ybase);

        ybase.real /=2;
        ybase.imag /=2;

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
            aufiltout.real *= 2;
            aufiltout.imag *= 2;
        }
        else
        {
            aufiltout = cfilt;
        }

            

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

void agc(int32_t *fi, int32_t *fq, int len)
{
static float smult = 0;
static float maxvol = 30000.0f;

    // measure peak value of these samples
    int32_t p = 0;
    for(int i=0; i<len; i++)
    {
        if(fi[i] > p) p = fi[i];
        if(fi[i] < -p) p = -fi[i];

        if(fq[i] > p) p = fq[i];
        if(fq[i] < -p) p = -fq[i];
    }

    // required multiplicator to bring peak to maxvol
    float mult = maxvol / (float)p;
    if(mult > 40.0f) mult = 40.0f;

    // slowly adapt smult to mult
    if(mult < smult)
        smult = mult;
    else
    {
        float diff = mult - smult;
        smult += diff/10.0f;
    }

    // do agc
    for(int i=0; i<len; i++)
    {
        fi[i] *= smult;
        fq[i] *= smult;
    }

    // measure peak and mid value of these samples
    int32_t pki = 0, pkq = 0;
    for(int i=0; i<len; i++)
    {
        if(fi[i] > pki) pki = fi[i];
        if(fq[i] > pkq) pkq = fq[i];
    }

    //printf("peak:%d smult:%f newpeak:%d %d\n",p,smult,pki,pkq);
}

float pmult = 32767.0f;  // adjust to get maximum values

void sendToPluto()
{
int32_t xi[PLUTOBUFSIZE];
int32_t xq[PLUTOBUFSIZE];
uint8_t txbuf[PLUTOBUFSIZE * 4];
int txbufidx = 0;

    for(int i=0; i<PLUTOBUFSIZE; i++)
    {
        // convert complex to pluto format
        xi[i] = (int32_t)(txarr[i].real * pmult); 
        xq[i] = (int32_t)(txarr[i].imag * pmult);

        //measure_maxval(xi[i], 480000);
    }

    

    if(audioagc > 0 && sendtone == 0)
    {
        agc(xi, xq, PLUTOBUFSIZE);
    }

    for(int i=0; i<PLUTOBUFSIZE; i++)
    {
        measure_maxval(xi[i], 480000);

        txbuf[txbufidx++] = xi[i] & 0xff;
        txbuf[txbufidx++] = xi[i] >> 8;
        txbuf[txbufidx++] = xq[i] & 0xff;
        txbuf[txbufidx++] = xq[i] >> 8;
    }

    // wait for Space in TX fifo
    while(keeprunning)
    {
        int s = fifo_usedspace(TXfifo);
        if(s < 5) break;
        usleep(100);
    }

    write_fifo(TXfifo,txbuf,PLUTOBUFSIZE*4);
}

