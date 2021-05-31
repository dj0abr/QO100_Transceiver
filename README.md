# QO100_Transceiver
QO-100 Software Transceiver using an Adalm-Pluto and an SBC (Raspberry, Odroid ...)

This project is a fully functional software based transceiver for QO-100

Version Status:\
V1.0 ... May,28 2021 ... Now fully working, Setup via Setup Menu.\
V1.1 ... May,31 2021 ... Pluto USB/ETH setup, speech compressor

## Hardware requirements

* Pluto SDR (connected via USB or Eth/USB Adapter)
* SBC like Raspberry PI-4, Odroid C4, Odroid N2 and others
* USB sound stick (if the SBC does not have a microphone connector)
* Satellite-Dish, QO-100 Feed and 2,4 GHz Amplifier (i.e. Amsat-DL 46dB Amp)

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
