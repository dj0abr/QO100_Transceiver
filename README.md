# QO100_Transceiver
QO-100 Software Transceiver using an Adalm-Pluto and an SBC (Raspberry, Odroid ...)

This project is a fully functional software based transceiver for QO-100

Status May,28 2021 ... Now fully working, Setup via Setup Menu.
TODO... speech compressor for better TX audio volume

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

## run the software

the software has two parts:

qo100trx ... this is the part doing all the work, without the GUI

trxGui.exe ... the user interface

* Copy both files in a directory of your choice
* start the software by entering:   mono  trxGui.exe

this opens the user interface and automatically starts the background jobs

## configuration

click the SETUP button

* choose the sound device where microphone and loud speaker are connected
* enter the correct RX and TX frequencies. See the examples on the screen.
