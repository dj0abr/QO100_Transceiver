VERSION := 173
CXXFLAGS = -Wall -O3 -std=c++0x -Wno-write-strings -Wno-narrowing -DDRIVER_SERIAL=$(VERSION) $(RASPI)
LDFLAGS = -lpthread -lrt -lm -liio -lliquid -lad9361 -lfftw3 -lfftw3_threads -lsndfile -lasound -lsoundio -lgpiod
OBJ = qo100trx.o rx.o tx.o fft.o\
kmlib/kmtimer.o kmlib/km_helper.o kmlib/kmfifo.o kmlib/rotary.o\
udp/udp.o\
liquid/liquiddrv.o liquid/liquiddrv_modulator.o\
pluto/pluto_finder.o pluto/pluto_driver.o pluto/pluto_setup.o pluto/pluto_run.o\
libkmaudio/libkmaudio_fifo.o  libkmaudio/libkmaudio_getDevices.o  libkmaudio/libkmaudio_getDevices_Linux.o\
libkmaudio/libkmaudio_init.o libkmaudio/libkmaudio_init_linux.o libkmaudio/libkmaudio_interface.o libkmaudio/libkmaudio_capture_linux.o\
libkmaudio/libkmaudio_playback_linux.o libkmaudio/libkmaudio_resampler.o

default: $(OBJ)
	g++ $(CXXFLAGS) -o Release/trxdriver $(OBJ) $(LDFLAGS)
	echo $(VERSION) > version.txt
	rm -rf  Release/*.config Release/*.pdb
	chmod 755 Release/startQO100trx

clean:
	rm -rf *.o
	rm -rf kmlib/*.o 
	rm -rf pluto/*.o 
	rm -rf udp/*.o 
	rm -rf liquid/*.o 
	rm -rf libkmaudio/*.o 
	rm -rf Release/trxdriver
	