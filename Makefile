CXXFLAGS = -Wall -O3 -std=c++0x -Wno-write-strings -Wno-narrowing
LDFLAGS = -lpthread -lrt -lm -liio -lliquid -lad9361 -lfftw3 -lfftw3_threads -lsndfile -lasound -lsoundio
OBJ = qo100trx.o rx.o tx.o\
kmlib/kmtimer.o kmlib/km_helper.o kmlib/kmfifo.o\
udp/udp.o\
liquid/liquiddrv.o\
pluto/pluto_finder.o pluto/pluto_driver.o pluto/pluto_setup.o pluto/pluto_run.o\
libkmaudio/libkmaudio_fifo.o  libkmaudio/libkmaudio_getDevices.o  libkmaudio/libkmaudio_getDevices_Linux.o\
libkmaudio/libkmaudio_init.o libkmaudio/libkmaudio_init_linux.o libkmaudio/libkmaudio_interface.o libkmaudio/libkmaudio_capture_linux.o\
libkmaudio/libkmaudio_playback_linux.o libkmaudio/libkmaudio_resampler.o

default: $(OBJ)
	g++ $(CXXFLAGS) -o qo100trx $(OBJ) $(LDFLAGS)

clean:
	rm -r *.o
	rm -r kmlib/*.o 
	rm -r pluto/*.o 
	rm -r udp/*.o 
	rm -r liquid/*.o 
	rm -r libkmaudio/*.o 
	rm -r qo100trx
	