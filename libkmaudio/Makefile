# Make file for libkmaudio
# make clean ... clean up intermediate files
# make ... generate libkmaudio as executable for testing purposes
#          and also creates the static library libkmaudio.a to be linked with another program
#	   when linking to another program also include "-lliquid -lsoundio" to your linker

CXXFLAGS = -Wall -O3 -std=c++0x -Wno-write-strings -Wno-narrowing
LDFLAGS = -lliquid -lsoundio
OBJM = libkmaudio.o 
OBJ = libkmaudio_fifo.o  libkmaudio_getDevices.o  libkmaudio_getDevices_Linux.o libkmaudio_init.o libkmaudio_init_linux.o libkmaudio_interface.o libkmaudio_capture_linux.o libkmaudio_playback_linux.o libkmaudio_resampler.o

default: $(OBJ) $(OBJM)
	g++ $(CXXFLAGS) -o libkmaudio $(OBJ) $(OBJM) $(LDFLAGS)
	ar rcs libkmaudio.a $(OBJ)

clean:
	rm -f *.o libkmaudio.a
	-rm -rf ./Release/*
	-rmdir Release
	-rm -rf ./Debug/*
	-rmdir Debug
	
