#define SAMPRATE  		560000			// 1.12MS/s

#define LNB_REF			24000UL			// kHz
#define BASEQRG			10489470UL		// kHz, spectrum range ist 10489.470 - 10490.03
#define RXBASEQRG		((BASEQRG - 390UL*LNB_REF)*1000UL)
#define TXBASEQRG		((2400000UL - 30UL)*1000UL)
#define PLUTOBUFSIZE  	(SAMPRATE/10)

#define MHZ(x) ((long long)(x*1000000.0 + .5))
#define GHZ(x) ((long long)(x*1000000000.0 + .5))

enum iodev { 
	RX = 0, 
	TX 
};

typedef struct _stream_cfg_ {
	long long bw_hz; // Analog bandwidth in Hz
	long long fs_hz; // Baseband sample rate in Hz
	long long lo_hz; // Local oscillator frequency in Hz
	const char* rfport; // Port name
} stream_cfg;


bool get_ad9361_stream_ch(__notused struct iio_context *ctx, enum iodev d, struct iio_device *dev, int chid, struct iio_channel **chn);
bool cfg_ad9361_streaming_ch(struct iio_context *ctx, stream_cfg *cfg, enum iodev type, int chid);
bool get_ad9361_stream_dev(struct iio_context *ctx, enum iodev d, struct iio_device **dev);
int pluto_get_USB(char *sn);
void pluto_setup();
void init_runloop();
void setTXfrequency(long long freq);
void setRXfrequency(long long freq);
void setTXpower();
void release_ptt();
void set_ptt();

extern stream_cfg rxcfg;
extern stream_cfg txcfg;
extern struct iio_context *ctx;
extern struct iio_buffer  *rxbuf;
extern struct iio_buffer  *txbuf;
extern struct iio_channel *rx0_i;
extern struct iio_channel *tx0_i;
extern float RX_BW;
extern float TX_BW;
extern double RX_FREQ;
extern double TX_FREQ;
extern float TX_GAIN;
extern char pluto_context_name[50];
