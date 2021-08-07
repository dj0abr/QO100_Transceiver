/*
    this is an extract from kmclib (see github, dj0abr repos)
    it works on raspberry pi only

* Raspberry PI / Zero AddOn Board specially for Ham Radio Applications
* ====================================================================
* Author: DJ0ABR
*
*   (c) DJ0ABR
*   www.dj0abr.de
*
*   This program is free software; you can redistribute it and/or modify
*   it under the terms of the GNU General Public License as published by
*   the Free Software Foundation; either version 2 of the License, or
*   (at your option) any later version.
*
*   This program is distributed in the hope that it will be useful,
*   but WITHOUT ANY WARRANTY; without even the implied warranty of
*   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*   GNU General Public License for more details.
*
*   You should have received a copy of the GNU General Public License
*   along with this program; if not, write to the Free Software
*   Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
* 
encoder.cpp
===========

function to handle rotary encoder

Usage:
======
initialize and assign ports to the encoders
-------------------------------------------
up to 3 encoders are supported, If not used, then assign port nunmber -1
init_rotencoder(int portA1, int portB1, int portA2, int portB2, int portA3, int portB3)
see gpio.h _INPUTS_ for valid port numbers

read encoder:
-------------
int getEncSteps(int idx)
idx ... encoder number 0,1 or 2
returns the number of steps since last read

*
*/

#include "../qo100trx.h"

int init_gpio();
int getPort(char *port);

#define NUM_OF_ENCODERS 2

pthread_mutex_t     rotenc_crit_sec;
#define LOCK	    pthread_mutex_lock(&(rotenc_crit_sec))
#define UNLOCK	    pthread_mutex_unlock(&(rotenc_crit_sec))

void *rotencproc(void *);    

char portA[NUM_OF_ENCODERS][3] = {"fa","va"};
char portB[NUM_OF_ENCODERS][3] = {"fb","vb"};

int encsteps[NUM_OF_ENCODERS];

pthread_t rotenc_tid;

// port numbers see gpio.h _INPORTS_
// -1 if not used
void init_rotencoder()
{
    // use the rotary encoder on a raspberry board only
    // RASPI is passed to the Makefile as: make RASPI='-DRASPI'
    #ifndef RASPI
    return;
    #endif 

    if(init_gpio() == -1) return;

    memset(&encsteps, 0, sizeof(int)*NUM_OF_ENCODERS);

    int pret = pthread_create(&rotenc_tid,NULL,rotencproc, NULL);
    if(pret)
    {
        printf("rotenc process NOT started\n");
        exit(0);
    }
}

int getEncSteps(int idx)
{
    int v;
    LOCK;
    v = encsteps[idx];
    encsteps[idx] = 0;
    UNLOCK;
    return v;
}

void *rotencproc(void *pdata)
{
    pthread_detach(pthread_self());
    
    printf("rotenc process started\n");

    int aval[NUM_OF_ENCODERS];
    int bval[NUM_OF_ENCODERS];
    int oldaval[NUM_OF_ENCODERS];
    while(keeprunning)
    {
        for(int i=0; i<NUM_OF_ENCODERS; i++)
        {
            aval[i] = getPort(portA[i]);
            bval[i] = getPort(portB[i]);

            if(aval[i]==1 && oldaval[i]==0)
            {
                // falling edge of port A
                int dir = -1;
                if(bval[i] == 1) dir = 1;
                LOCK;
                encsteps[i] += dir;
                UNLOCK;
            }
            oldaval[i] = aval[i];
        }

        usleep(100);
    }

    printf("exit rotenc thread\n");
    pthread_exit(NULL);
}

/* ============= Linux GPIO interface =============
 requires packages: gpiod  libgpiod-dev

 command line interface see:
 https://git.kernel.org/pub/scm/libs/libgpiod/libgpiod.git/tree/README

 C-Functions see: /usr/include/gpiod.h

 connect rotary encoders to these pins:

 frequency-encoder A ... GPIO17 (Pin11)
 frequency-encoder B ... GPIO27 (Pin13)
 volume   -encoder A ... GPIO4  (Pin 7)
 volume   -encoder B ... GPIO22 (Pin15)
*/

#include "gpiod.h"

struct gpiod_chip *chip = NULL;
struct gpiod_line *fa = NULL;
struct gpiod_line  *fb = NULL, *va = NULL, *vb = NULL;

int init_gpio()
{
    int ret;

    chip= gpiod_chip_open("/dev/gpiochip0");
    if(!chip)
    {
        printf("cannot open GPIO chip\n");
        return -1;
    }

    fa= gpiod_chip_get_line(chip,17);
    if (!fa) {
        printf("cannot access GPIO17\n");
        gpiod_chip_close(chip);
        return -1;
    }

    fb= gpiod_chip_get_line(chip,27);
    if (!fa) {
        printf("cannot access GPIO27\n");
        gpiod_chip_close(chip);
        return -1;
    }

    va= gpiod_chip_get_line(chip,4);
    if (!fa) {
        printf("cannot access GPIO4\n");
        gpiod_chip_close(chip);
        return -1;
    }

    vb= gpiod_chip_get_line(chip,22);
    if (!fa) {
        printf("cannot access GPIO22\n");
        gpiod_chip_close(chip);
        return -1;
    }

    ret = gpiod_line_request_input(fa, "huhu");
    if (ret) {
        printf("cannot init GPIO17 for input\n");
        gpiod_chip_close(chip);
        return -1;
    }

    ret = gpiod_line_request_input(fb, "huhu");
    if (ret) {
        printf("cannot init GPIO27 for input\n");
        gpiod_chip_close(chip);
        return -1;
    }

    ret = gpiod_line_request_input(va, "huhu");
    if (ret) {
        printf("cannot init GPIO4 for input\n");
        gpiod_chip_close(chip);
        return -1;
    }

    ret = gpiod_line_request_input(vb, "huhu");
    if (ret) {
        printf("cannot init GPIO22 for input\n");
        gpiod_chip_close(chip);
        return -1;
    }

    printf("GPIO for Encoders initialized\n");

    return 0;
}
void close_gpio()
{
    if(fa) gpiod_line_release(fa);
    if(fb) gpiod_line_release(fb);
    if(va) gpiod_line_release(va);
    if(vb) gpiod_line_release(vb);
    if(chip) gpiod_chip_close(chip);
}

// get status of physical port
int getPort(char *port)
{
    if(port[0] == 'f')
    {
        if(port[1] == 'a') return gpiod_line_get_value(fa);

        return gpiod_line_get_value(fb);
    }

    if(port[1] == 'a') return gpiod_line_get_value(va);

    return gpiod_line_get_value(vb);
}
