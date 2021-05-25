/*
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
* universal thread safe fifo, used by many drivers and applications
*
*/

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdint.h>
#include <pthread.h>
#include "kmfifo.h"


FIFOOBJ fifo[MAXFIFOS];
int fifonum = 0;

#define LOCK(id)	pthread_mutex_lock(&fifo[id].crit_sec)
#define UNLOCK(id)	pthread_mutex_unlock(&fifo[id].crit_sec)

// creates a new fifo
// maxelem_num ... max. number of elements in fifo
// maxelem_len ... max. length of one element (shorter elements are allowed)
// returns: ID of a fifo object, to be used for fifo functions
int create_fifo(int maxelem_num, int maxelem_len)
{
    // create the fifo memory and init variables
    fifo[fifonum].fifomem = malloc(maxelem_num * maxelem_len);
    fifo[fifonum].plen = (int *)malloc(maxelem_num * sizeof(int));
    fifo[fifonum].maxelemlen =maxelem_len;
    fifo[fifonum].maxelem = maxelem_num;
    fifo[fifonum].rdidx = 0;
    fifo[fifonum].wridx = 0;
    
    pthread_mutex_init(&fifo[fifonum].crit_sec, NULL);

    int id = fifonum;
    fifonum++;
    return id;
}

void destroy_fifos()
{
    for(int i=0; i<fifonum; i++)
    {
        free(fifo[fifonum].fifomem);
        free(fifo[fifonum].plen);
        if (&fifo[fifonum].crit_sec != NULL) pthread_mutex_destroy(&fifo[fifonum].crit_sec);
    }
}

// write into the fifo
// ignore data if the fifo is full
void write_fifo(int id, uint8_t *pdata, int len)
{
    FIFOOBJ *pfo = &fifo[id];

    if(len > pfo->maxelemlen)
    {
        printf("element too long\n");
        return;
    }

    LOCK(id);

    if (((pfo->wridx + 1) % pfo->maxelem) == pfo->rdidx)
    {
        printf("cannot WRITE fifo %d full\n",id);
        UNLOCK(id);
        return;
    }

    // insert length of new data
    *(pfo->plen) = len; // real length of the element

    // insert new data
    void *dst = (void *)((uint8_t *)pfo->fifomem + pfo->wridx * pfo->maxelemlen);
    memcpy(dst, pdata, len);
    if(++pfo->wridx >= pfo->maxelem) pfo->wridx = 0;

    UNLOCK(id);
}

// read from the fifo
// return: number of bytes read
int read_fifo(int id, uint8_t* pdata, int maxlen)
{
    FIFOOBJ *pfo = &fifo[id];
    LOCK(id);

    if(pfo->rdidx == pfo->wridx)
    {
        // Fifo empty, no data available
        //printf("read: no data\n");
        UNLOCK(id);
        return 0;
    }

    // read length
    int len =  *pfo->plen;  // length of an element

    if (len > maxlen)
    {
        printf("read_fifo: %d, pdata too small. Need:%d has:%d\n", id, len, maxlen);
        return 0; // pdata too small
    }

    // read data
    void *src = (void *)((uint8_t *)pfo->fifomem + pfo->rdidx * pfo->maxelemlen);
    memcpy(pdata, src, len);
    if (++pfo->rdidx >= pfo->maxelem) pfo->rdidx = 0;

    UNLOCK(id);
    return len;
}

void fifo_clear(int id)
{
    fifo[id].wridx = fifo[id].rdidx = 0;
}

int fifo_freespace(int id)
{
    int freebuf = 0;

    LOCK(id);

    int elemInFifo = (fifo[id].wridx + fifo[id].maxelem - fifo[id].rdidx) % fifo[id].maxelem;
    freebuf = fifo[id].maxelem - elemInFifo;

    UNLOCK(id);
    return freebuf;
}

int fifo_dataavail(int id)
{
    LOCK(id);

    if (fifo[id].rdidx == fifo[id].wridx)
    {
        // Fifo empty, no data available
        UNLOCK(id);
        return 0;
    }
    UNLOCK(id);
    return 1;
}

int fifo_usedspace(int id)
{
    int us = fifo[id].maxelem - fifo_freespace(id);
    return us;
}
