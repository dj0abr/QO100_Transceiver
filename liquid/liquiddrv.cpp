#include "../qo100trx.h"
#include <fftw3.h>

void createSSBfilter();
void init_beaconlock();
void close_beaconlock();
void exec_beaconlock(liquid_float_complex samp);

float rxvolume = 1.0f;

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
//    printf("tune_downmixer: newoffset %d, lastoffset %d\n", newoffset,lastoffset);
    if(lastoffset != newoffset)
    {
        lastoffset = newoffset;
        float RADIANS_PER_SAMPLE   = ((2.0f * (float)M_PI * (lastoffset-280000))/(float)SAMPRATE);
        printf("tune RX to %f, radianspersample %f\n",BASEQRG*1e3 + lastoffset,RADIANS_PER_SAMPLE);
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

        //measure_maxval(fabs(soundsamp.real), 1000);

        // the rate here is 48000S/s
        // demodulate
        ampmodem_demodulate(demod, soundsamp, &z);

        // send z to soundcard
        if(audioloop == 0 && pbidx != -1)
        {
            static float playvol = 1.0f;
            if(((ptt && rxmute) || mute) && rfloop == 0) playvol = 0.08f;
            else playvol = 2.0f;
            float vol = playvol * rxvolume;
            if(fabs(vol) > 1.0f) 
            {
                // reduce volume if overdriven
                rxvolume = 1.0f / playvol;
                vol = playvol * rxvolume;
            }

            kmaudio_playsamples(pbidx,&z,1,vol);
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
int bcnoffset_new = -1;

int bcn_spurious_count = 0;
int bcn_spurious_max = 5;
int bcn_huge_offset_count = 0;
int bcn_huge_offset_max = 2;
bool bcn_spurious_recovery = false;
bool bcn_huge_offset_recovery_needed    = false;
bool bcn_huge_offset_recovery_deploy    = false;

void init_beaconlock()
{
    // Downmixer
    bcn_dnnco = nco_crcf_create(LIQUID_NCO);
    int offset = startqrg - 470000;
    float RADIANS_PER_SAMPLE   = ((2.0f * (float)M_PI * (offset-280000))/(float)SAMPRATE);
    nco_crcf_set_phase(bcn_dnnco, 0.0f);
    nco_crcf_set_frequency(bcn_dnnco, RADIANS_PER_SAMPLE);
    printf("radianspersample %f\n",RADIANS_PER_SAMPLE);

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

static int beacon_search_countdown = 0;
static int beacon_search_hz;
static int previous_beacon_detection = 0; //0 no detection, 1 double peaks, 2 single low, single hi
static int wait_for_beacon_to_stabilize = 0;
	
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
    const int EXPECTED_BEACON_SEPARATION = 400;
    const int BEACON_TOLERANCE = 10;
    const int MAX_ALLOWED_OFFSET_CHANGE = 100;
    const int MAX_ALLOWED_HUGE_RECOVERY = 300;
    
    const int MAX_COUNTDOWN_UNTIL_BEACON_SEARCH = 30;
    const int MAX_HZ_BEACON_SEARCH = 1500;
    const int STEP_HZ_BEACON_SEARCH = 15;



    //printf("%d  %d  %d\n",minqrg,maxqrg,diff);

    if(diff > (EXPECTED_BEACON_SEPARATION - BEACON_TOLERANCE) && 
       diff < (EXPECTED_BEACON_SEPARATION + BEACON_TOLERANCE)) 
    {
        //printf("%d  %d  %d\n",minqrg,maxqrg,diff);
        // we have both frequencies, then measure the beacon mid frequency
        int bcnqrg = minqrg + diff/2;
        int bcnqrgsoll = 500200;    // expected frequency
        bcnoffset_new = (bcnqrg - bcnqrgsoll);
        printf("lower beacon %d .. %d: mid QRG: %d kHz. Offset: %d Hz\n",minqrg,maxqrg,bcnqrg,bcnoffset);
        // Try to recover if there is big offset, but two peaks are detected with correct distances.
        // Huge offset happens usually when we happen to be locked to wrong peak.
        // Then we can recover by switching to the new offset.
        if(previous_beacon_detection == 1)
            wait_for_beacon_to_stabilize++;
        previous_beacon_detection = 1;
        bcn_spurious_count=0;
        beacon_search_countdown = 0;
        
        if(bcn_huge_offset_recovery_needed ) {
            printf("Huge offset Recover\n");
            bcn_huge_offset_count=0;
            bcn_huge_offset_recovery_needed = false;
            bcn_huge_offset_recovery_deploy = true;
            wait_for_beacon_to_stabilize = 0;
        }
        else return;
        
    }
    else if ( (diff <= EXPECTED_BEACON_SEPARATION - BEACON_TOLERANCE) && 
              (diff >= EXPECTED_BEACON_SEPARATION + BEACON_TOLERANCE)) 
    {
        // wrong offset between peaks, probably spurious signal, ignore
        printf("%d  %d  %d SPURIOUS\n",minqrg,maxqrg,diff);
        bcn_spurious_count++;
        previous_beacon_detection = 0;
    }
    // Detect one CW peak
    // Don't go there if we are in recovery mode
    // If the frequency was uncertain in previous round we can't be sure if it will be lo or hi peak
    else if(!bcn_huge_offset_recovery_needed && wait_for_beacon_to_stabilize > 1)
    {
        bcn_spurious_count=0;
        int difflow = minqrg - 500000;
        int diffhigh = maxqrg - 500400;
        if(abs(difflow) < abs(diffhigh))
        {
            printf("lower beacon low QRG: %d kHz. Offset: %d Hz\n",minqrg,difflow);
            bcnoffset_new = difflow;
            previous_beacon_detection = 2;
        }
        else
        {
            printf("lower beacon hi  QRG: %d kHz. Offset: %d Hz\n",maxqrg,diffhigh);
            bcnoffset_new = diffhigh;
            previous_beacon_detection = 3;
        }
        if((abs(bcnoffset_new) > MAX_ALLOWED_OFFSET_CHANGE) /*&& previous_beacon_detection != 1*/){
            printf("Offset too big to be determined from single CW carrier. prev_bcn: %d \n", previous_beacon_detection);
            bcnoffset_new=0;
            bcn_huge_offset_recovery_needed = true;
            previous_beacon_detection = 0;
            wait_for_beacon_to_stabilize = 0;
        }
    }
    else {
		bcn_huge_offset_recovery_needed = true;
		wait_for_beacon_to_stabilize = 0;
		bcnoffset_new=0;
		if(beacon_search_countdown < MAX_COUNTDOWN_UNTIL_BEACON_SEARCH) {
			printf("WAITING FOR BEACON LOCK RECOVERY! (%d/%d)\n", beacon_search_countdown+1, MAX_COUNTDOWN_UNTIL_BEACON_SEARCH);
			beacon_search_countdown++;
			beacon_search_hz = 0;
			bcnoffset_new = 0;
        }
        else{
			if(beacon_search_hz<MAX_HZ_BEACON_SEARCH){
				printf("SEARCHING BEACON FOR LOCK RECOVERY! (%d Hz/%d Hz)\n", beacon_search_hz, MAX_HZ_BEACON_SEARCH);
				beacon_search_hz += STEP_HZ_BEACON_SEARCH;
				bcnoffset_new = STEP_HZ_BEACON_SEARCH;//beacon_search_hz;
				
				
			}
			else {
				//Start from the beginning of the search band
				printf("MAX HZ REACHED SEARCHING BEACON FOR LOCK RECOVERY! Starting over. (%d Hz/%d Hz)\n", beacon_search_hz, MAX_HZ_BEACON_SEARCH);
				bcnoffset_new = bcnoffset - beacon_search_hz;
				beacon_search_hz = 0;
				bcn_huge_offset_recovery_deploy = true;
				}
			//printf("SEARCHING BEACON FOR LOCK RECOVERY! (%d Hz/%d Hz)\n", beacon_search_hz, MAX_HZ_BEACON_SEARCH);
			//bcnoffset_new += STEP_HZ_BEACON_SEARCH;
	    }
	    previous_beacon_detection = 0;
    }

    // send offset to GUI
    if((abs(bcnoffset_new)>MAX_ALLOWED_OFFSET_CHANGE && !bcn_huge_offset_recovery_needed && !bcn_huge_offset_recovery_deploy)){
            printf("HUGE OFFSET with lo and hi CW detected. Do nothing. Offset_new: %d Hz\n",bcnoffset_new);
            bcn_huge_offset_count++;
            if(bcn_huge_offset_count >= bcn_huge_offset_max) {
                bcn_huge_offset_recovery_needed = true;
            }
        }
    else
    {
        uint8_t drift[5];
        int a_iir = 1;
        int b_iir = 0; //with 3 taps frequency offset stays too long
        if(bcnoffset_new==0){
            //printf("new=0\n");
            bcnoffset=bcnoffset_new;
        }
        else if((abs(bcnoffset) > MAX_ALLOWED_HUGE_RECOVERY) && !bcn_huge_offset_recovery_deploy) {
			//printf("zero\n");
            bcnoffset=0;
            bcnoffset_new=0;
        }
        else{
            //printf("IIR\n");
            bcnoffset=((a_iir*bcnoffset_new+b_iir*bcnoffset)/(a_iir+b_iir)); //IIR filtering the offset to requce changes
        }
        bcn_huge_offset_recovery_deploy = false;
        printf("Offset: %d Hz. Offset_new: %d Hz\n",bcnoffset,bcnoffset_new);
        drift[0] = 6;
        drift[1] = bcnoffset >> 24;
        drift[2] = bcnoffset >> 16;
        drift[3] = bcnoffset >> 8;
        drift[4] = bcnoffset & 0xff;
        
        sendUDP(gui_ip, GUI_UDPPORT, drift, 5);

        tune_downmixer();
    }
}

