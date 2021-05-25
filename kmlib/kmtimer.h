#include <time.h>
#include <stdio.h>
#include <signal.h>
#include <pthread.h>
#include <unistd.h>
#include <errno.h>
#include <sys/time.h>

int start_timer(int mSec, void(*timer_func_handler)(void));
void stop_timer(int timer);
void sleep_ms(int ms);


