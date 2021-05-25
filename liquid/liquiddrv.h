void streamToSamples(uint8_t *stream, int streamlen, liquid_float_complex *samples);
void downmix(liquid_float_complex *samples, int len, int offsetfreq);
void init_liquid();
void close_liquid();
void tune_downmixer(int offset);