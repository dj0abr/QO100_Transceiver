# QO100_Transceiver
QO-100 Software Transceiver using an Adalm-Pluto and an SBC (Raspberry, Odroid ...)

This project is a fully functional software based transceiver for QO-100

Status May,28 2021 ... RX and TX are fully working, still need some work to make installation easier. In the meantime the base frequency for RX and TX has to be defined in pluto.h

## Hardware requirements

* Pluto SDR (connected via USB or Eth/USB Adapter)
* SBC like Raspberry PI-4, Odroid C4, Odroid N2 and others
* USB sound stick (if the SBC does not have a microphone connector)
* Satellite-Dish, QO-100 Feed and 2,4 GHz Amplifier (i.e. Amsat-DL 46dB Amp)

## Installation

Install all required libraries by running the script:
./prepare_ubuntu_pluto

## build the software
make clean
make

this builds the executable file: qo100trx

## configuration

#### configure Pluto

open the file qo100trx.cpp and look for "char plutoid[100]"

Run with a pluto connected via USB:

char plutoid[100] = {0};

or access a pluto which is connected via Ethernet-Adapter:

char plutoid[100] = {"ip:192.168.20.25"};

#### configure sound card

start the program qo100trx. All available sound cards are shown in the terminal.

open the file qo100trx.cpp and look for:
pbidx = kmaudio_startPlayback((char *)"USB Advanced Audio Device Analog Stereo", 48000);

replace "USB Advanced Audio Device Analog Stereo" with the name of your sound card.

## run the software

the software has two parts:

qo100trx ... this is the part doing all the work, without the GUI

trxGui.exe ... the user interface

To start the software driver run:  ./qo100trx

To start the GUI open another terminal, go into the directory: .../trxGui/bin/Release/   and run:  mono  trxGui.exe

