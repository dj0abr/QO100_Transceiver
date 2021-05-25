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

#define AUDIO

extern int RXfifo;
extern int TXfifo;
extern int pbidx, capidx;