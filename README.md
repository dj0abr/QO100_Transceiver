# QO100_Transceiver
QO-100 Software Transceiver using an Adalm-Pluto and an SBC (Raspberry, Odroid ...)

This project is a fully functional software based transceiver for QO-100

Version Status:\
V1.0 ... May,28 2021 ... Now fully working, Setup via Setup Menu.\
V1.1 ... May,31 2021 ... Pluto USB/ETH setup, speech compressor\
V1.2 ... June,2 2021 ... Audio Filter, Muting, new user interface\
V1.4 ... June,6 2021 ... calibration for Pluto and LNB\
V1.5 ... June,11 2021 ... Major Upgrade (!) PLEASE CHECK YOUR SETUP (frequencies have been changed)\
V1.6 ... June,12 2021 ... various bug fixes (Sound card name, now working on 64bit Raspi)
V1.63 .. June,16 2021 ... new settings: Pluto TX power, Screen Size

![alt text](https://github.com/dj0abr/QO100_Transceiver/blob/main/trxGui/Properties/sampleGUI.png)

## Hardware requirements

* Pluto SDR (connected via USB or Eth/USB Adapter)
* SBC like Raspberry PI-4, Odroid C4, Odroid N2 and others
* USB sound stick (if the SBC does not have a microphone connector)
* Satellite-Dish, QO-100 Feed and 2,4 GHz Amplifier (i.e. Amsat-DL 46dB Amp)

## Documentation

this software was developed for Amsat-DL. You can find the detailed documentation in the Amsat-Wiki: 
http://wiki.amsat-dl.org/doku.php?id=en:plutotrx:overview

## Installation

open a terminal and run these commands:

```
git clone https://github.com/dj0abr/QO100_Transceiver
cd QO100_Transceiver
./prepare_ubuntu_pluto
./install
```

## run the software

the software has two parts:

trxdriver .... this is the part doing all the work, without the GUI\
trxGui.exe ... the user interface

After installation both files are located in the folder\
.../QO100_Transceiver/Release

Additionally a start script was created

* start the software by entering:   ./startQO100trx

this opens the user interface and automatically starts the background jobs

## configuration

click the SETUP button

* enter how the pluto is connected
* choose the sound device where microphone and loud speaker are connected
* enter the correct RX and TX frequencies. See the examples on the screen.

#### Audio on Raspberry PI OS: 

Raspi OS shows many many audio devices nobody really needs.\
It is highly recommended to choose Pulseaudio for loudspeaker and microphone and then use the sound mixer pavucontrol (is installed) to select devices and control volume.

## Linux OS issues

In general this software runs on all linux distributions.
The script prepare_ubuntu_pluto contains the installation for all required libraries.

But different distributions may use different names for their libraries. The script was developed für Debian based systems, like Ubuntu, Mint, Raspbery-OS and many others.

If you try to run prepare_ubuntu_pluto on different platforms like Suse, Fedora... you maybe need to change the name of some libraries.
