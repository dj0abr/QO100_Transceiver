/*
* Adalm Pluto Driver
* ==================
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
* =======================================
* various subroutines to access the pluto
* =======================================
*
* most subroutines from here:
* https://raw.githubusercontent.com/analogdevicesinc/libiio/master/examples/ad9361-iiostream.c
*
*/

#include "../qo100trx.h"

void setTXfrequency(long long freq);

// static scratch mem for strings 
static char tmpstr[64];

// check return value of attr_write function 
static void errchk(int v, const char* what) 
{
	if (v < 0) 
	{ 
		printf("Error %d writing to channel \"%s\"\nvalue may not be supported.\n", v, what); 
	}
}

// write attribute: double 
static void wr_ch_double(struct iio_channel *chn, const char* what, double val)
{
	errchk(iio_channel_attr_write_double(chn, what, val), what);
}

// write attribute: long long int 
static void wr_ch_lli(struct iio_channel *chn, const char* what, long long val)
{
	errchk(iio_channel_attr_write_longlong(chn, what, val), what);
}

// write attribute: string 
static void wr_ch_str(struct iio_channel *chn, const char* what, const char* str)
{
	errchk(iio_channel_attr_write(chn, what, str), what);
}

// helper function generating channel names 
static char* get_ch_name(const char* type, int id)
{
	snprintf(tmpstr, sizeof(tmpstr), "%s%d", type, id);
	return tmpstr;
}

// returns ad9361 phy device 
static struct iio_device* get_ad9361_phy(struct iio_context *ctx)
{
	struct iio_device *dev =  iio_context_find_device(ctx, "ad9361-phy");
	return dev;
}

// finds AD9361 streaming IIO devices 
bool get_ad9361_stream_dev(struct iio_context *ctx, enum iodev d, struct iio_device **dev)
{
	switch (d) {
	case TX: *dev = iio_context_find_device(ctx, "cf-ad9361-dds-core-lpc"); return *dev != NULL;
	case RX: *dev = iio_context_find_device(ctx, "cf-ad9361-lpc");  return *dev != NULL;
	}
    return 0;
}

// finds AD9361 streaming IIO channels 
bool get_ad9361_stream_ch(__notused struct iio_context *ctx, enum iodev d, struct iio_device *dev, int chid, struct iio_channel **chn)
{
	*chn = iio_device_find_channel(dev, get_ch_name("voltage", chid), d == TX);
	if (!*chn)
		*chn = iio_device_find_channel(dev, get_ch_name("altvoltage", chid), d == TX);
	return *chn != NULL;
}

// finds AD9361 phy IIO configuration channel with id chid 
static bool get_phy_chan(struct iio_context *ctx, enum iodev d, int chid, struct iio_channel **chn)
{
	switch (d) {
	case RX: *chn = iio_device_find_channel(get_ad9361_phy(ctx), get_ch_name("voltage", chid), false); return *chn != NULL;
	case TX: *chn = iio_device_find_channel(get_ad9361_phy(ctx), get_ch_name("voltage", chid), true);  return *chn != NULL;
	}
    return 0;
}

// finds AD9361 local oscillator IIO configuration channels 
static bool get_lo_chan(struct iio_context *ctx, enum iodev d, struct iio_channel **chn)
{
	switch (d) {
	 // LO chan is always output, i.e. true
	case RX: *chn = iio_device_find_channel(get_ad9361_phy(ctx), get_ch_name("altvoltage", 0), true); return *chn != NULL;
	case TX: *chn = iio_device_find_channel(get_ad9361_phy(ctx), get_ch_name("altvoltage", 1), true); return *chn != NULL;
	}
    return 0;
}

// applies streaming configuration through IIO 
bool cfg_ad9361_streaming_ch(struct iio_context *ctx, stream_cfg *cfg, enum iodev type, int chid)
{
	struct iio_channel *chn = NULL;

	// Configure phy and lo channels
	if (!get_phy_chan(ctx, type, chid, &chn)) {	return false; }
	wr_ch_str(chn, "rf_port_select",     cfg->rfport);
	wr_ch_lli(chn, "rf_bandwidth",       cfg->bw_hz);
	//printf("write sampling_frequency: %lld\n",cfg->fs_hz);
	wr_ch_lli(chn, "sampling_frequency", cfg->fs_hz);
	
	if(type == TX)
	{
		//wr_ch_double(chn, "hardwaregain", txcfg.hwgain);
		setTXpower();
		setTXfrequency(cfg->lo_hz);
	}
	else
	{
		// show possible values: iio_attr -n 192.168.20.25 -c ad9361-phy voltage0
		wr_ch_str(chn, "gain_control_mode","slow_attack");
		//wr_ch_str(chn, "gain_control_mode","manual");
		//wr_ch_lli(chn, "hardwaregain", 66);
		// Configure LO channel
		setRXfrequency(cfg->lo_hz);
	}
	return true;
}

int lasttxpower = -1;

void setTXpower()
{
	if(txpower != lasttxpower)
	{
		if(txpower > 0) txpower = 0;	// 0 is pluto's max value

		lasttxpower = txpower;
		printf("set TX power to %d dBm\n",txpower);

		struct iio_channel *chn = NULL;
		if (!get_phy_chan(ctx, TX, 0, &chn)) { return; }
	
		// txpower=-40: switches OFF the PTT in the F5OEO firmware
		wr_ch_double(chn, "hardwaregain", (double)txpower);
	}
}

void set_ptt()
{
	lasttxpower = -1;
	setPort("p", 1);
}

void release_ptt()
{
	printf("release PTT by setting -40dBm\n");

	// set the power to -40, this releases the GPO0,1 PTT in the F5OEO firmware (>V.2021)
	struct iio_channel *chn = NULL;
	if (!get_phy_chan(ctx, TX, 0, &chn)) { return; }
	wr_ch_double(chn, "hardwaregain", (double)-40);

	setPort("p", 0);
}

long long lastfreq = 0;

void setTXfrequency(long long freq)
{
	freq += refoffset;

	if(freq != lastfreq)
	{
		lastfreq = freq;

		//printf("************* TX-QRG *************** %lld, offset:%d\n",freq,refoffset);
		
		struct iio_channel *chn = NULL;
		if (!get_lo_chan(ctx, TX, &chn)) { return; }
		wr_ch_lli(chn, "frequency", freq);	
	}
}

long long lastrxfreq = 0;

void setRXfrequency(long long freq)
{
	freq += refoffset;
	
	if(freq != lastrxfreq)
	{
		lastrxfreq = freq;

		//printf("************* RX-QRG *************** %lld, offset:%d\n",freq,refoffset);
		
		struct iio_channel *chn = NULL;
		if (!get_lo_chan(ctx, RX, &chn)) { return; }
		wr_ch_lli(chn, "frequency", freq);	
	}
}