#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <string.h>
#include <math.h>
#include <stdbool.h>
#include <stdint.h>
#include <signal.h>
#include <pthread.h>
#include <iio.h>
#include <ad9361.h>
#include <arpa/inet.h>
#include <errno.h>
#include <fcntl.h>
#include <sys/ioctl.h>
#include <sys/time.h>

#include "kmlib/km_helper.h"
#include "kmlib/kmfifo.h"
#include "kmlib/kmtimer.h"
#include "udp/udp.h"
#include "liquid.h"
#include "liquid/liquiddrv.h"
#include "pluto/pluto.h"
#include "libkmaudio/libkmaudio.h"

#define GUI_UDPPORT 40820

void init_rx();
void init_tx();
void init_fft();
void close_fft();

extern int RXfifo;
extern int TXfifo;
extern int FFTfifo;
extern int pbidx, capidx;
extern char gui_ip[20];
extern int RXoffsetfreq;
extern int TXoffsetfreq;
extern uint8_t audioloop;
extern uint8_t ptt;
extern uint8_t rfloop;
extern int compressor;
extern int audioagc;
extern int rxfilter;
extern int txfilter;
extern int rxmute;
extern int refoffset;