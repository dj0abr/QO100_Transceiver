#include "../qo100trx.h"

void tune_upmixer(int offset);
void sendToPluto();

// Modulator
float mod_index  = 0.9f;                // modulation index (bandwidth)
ampmodem mod = NULL;

// Up-Sampler
unsigned int interp_h_len = 13;    // filter semi-length (filter delay)
float interp_r = (float)((double)SAMPRATE / (double)48000); // resampling rate (output/input)
float interp_bw=0.1f;              // cutoff frequency
float interp_slsl= 60.0f;          // resampling filter sidelobe suppression level
unsigned int interp_npfb=32;       // number of filters in bank (timing resolution)
resamp_crcf interp_q = NULL;

// up mixer
nco_crcf upnco = NULL;    

// Low pass
unsigned int tx_lp_order =   4;       // filter order
float        tx_lp_fc    =   0.05f;    // cutoff frequency
float        tx_lp_f0    =   0.015f;    // center frequency
float        tx_lp_Ap    =   1.0f;    // pass-band ripple
float        tx_lp_As    =  60.0f;    // stop-band attenuation
unsigned int tx_lp_n     = 128;       // number of samples
iirfilt_crcf tx_lp_q = NULL;


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

    // SSB Mmodulator
    mod   = ampmodem_create(mod_index, LIQUID_AMPMODEM_USB, 1);

    // create upsampler
    interp_q = resamp_crcf_create(interp_r,interp_h_len,interp_bw,interp_slsl,interp_npfb);
    if(interp_q == NULL) printf("interp_q error\n");

        // low pass
    tx_lp_q = iirfilt_crcf_create_prototype(LIQUID_IIRDES_ELLIP, LIQUID_IIRDES_BANDPASS, LIQUID_IIRDES_SOS,
                                         tx_lp_order, tx_lp_fc, tx_lp_f0, tx_lp_Ap, tx_lp_As);

}

void close_liquid_modulator()
{
    printf("close DSP modulator\n");

    if(upnco) nco_crcf_destroy(upnco);
    upnco = NULL;

    if(mod) ampmodem_destroy(mod);
    mod = NULL;

    if(interp_q) resamp_crcf_destroy(interp_q);
    interp_q = NULL;

    if(tx_lp_q) iirfilt_crcf_destroy(tx_lp_q);
    tx_lp_q = NULL;

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
        printf("tune TX to %f\n",BASEQRG*1e6 + offset);
        float RADIANS_PER_SAMPLE   = ((2.0f * (float)M_PI * offset)/(float)SAMPRATE);
        nco_crcf_set_phase(upnco, 0.0f);
        nco_crcf_set_frequency(upnco, RADIANS_PER_SAMPLE);
    }
}

liquid_float_complex txarr[PLUTOBUFSIZE];
int txarridx = 0;

void upmix(float *f, int len, int offsetfreq)
{
    if (mod == NULL) return;
    if (interp_q == NULL) return;
    if (upnco == NULL) return;
    if (tx_lp_q == NULL) return;

    // re-tune if TX grq has been changed
    tune_upmixer(offsetfreq);

    for(int i=0; i<len; i++)
    {
        // modulator, at 48k audio sample rate
        liquid_float_complex y;
        ampmodem_modulate(mod, f[i], &y);

        // filter SSB bandwidth
        liquid_float_complex cfilt;
        iirfilt_crcf_execute(tx_lp_q, y, &cfilt);

        // resample from 48000S/s to pluto rate
        liquid_float_complex out[(int)interp_r+2];
        unsigned int num_written;
        resamp_crcf_execute(interp_q, cfilt, out, &num_written);
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
int16_t xi;
int16_t xq;
uint8_t txbuf[PLUTOBUFSIZE * 4];
int txbufidx = 0;

    for(int i=0; i<PLUTOBUFSIZE; i++)
    {
        // convert complex to pluto format
        xi = (int16_t)(txarr[i].real * 32768.0f);
        xq = (int16_t)(txarr[i].imag * 32768.0f);

        txbuf[txbufidx++] = xi & 0xff;
        txbuf[txbufidx++] = xi >> 8;
        txbuf[txbufidx++] = xq & 0xff;
        txbuf[txbufidx++] = xq >> 8;
    }

    write_fifo(TXfifo,txbuf,PLUTOBUFSIZE*4);
}
