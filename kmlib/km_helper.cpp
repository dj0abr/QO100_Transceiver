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
kmhelper.cpp
===========

useful functions

*
*/

#include "../qo100trx.h"
#include <pwd.h>

int keeprunning = 1;    // set to 0 at program end to exit all processes
char configfile[512] = {0};

// check if it is already running
int isRunning(char *prgname)
{
    int num = 0;
    char s[256];
    sprintf(s,"ps -e | grep %s",prgname);
    
    FILE *fp = popen(s,"r");
    if(fp)
    {
        // gets the output of the system command
        while (fgets(s, sizeof(s)-1, fp) != NULL) 
        {
            if(strstr(s,prgname) && !strstr(s,"grep"))
            {
                if(++num == 2)
                {
                    printf("%s is already running, do not start twice !\n",prgname);
                    pclose(fp);
                    return 1;
                }
            }
        }
        pclose(fp);
    }
    return 0;
}

void (*sigfunc)();

// signal handler
void sighandler(int signum)
{
    //printf("\n\nprogram stopped by signal\n");

    (*sigfunc)();
}

void install_signal_handler(void (*signalfunction)())
{
    sigfunc = signalfunction;
    // signal handler, mainly used if the user presses Ctrl-C
    struct sigaction sigact;
    sigact.sa_handler = sighandler;
	sigemptyset(&sigact.sa_mask);
	sigact.sa_flags = 0;
	sigaction(SIGINT, &sigact, NULL);
	sigaction(SIGTERM, &sigact, NULL);
	sigaction(SIGQUIT, &sigact, NULL);
    sigaction(SIGABRT, &sigact, NULL); // assert() error
    
    //sigaction(SIGSEGV, &sigact, NULL);
    
    // switch off signal 13 (broken pipe)
    // instead handle the return value of the write or send function
    signal(SIGPIPE, SIG_IGN);
}

void getParameters(int argc, char *argv[])
{
	int opt;
	while ((opt = getopt(argc, argv, "c:")) != -1)
    {
        switch (opt)
        {
        case 'c':
            // specify config file name
            if(optarg && strlen(optarg) > 2 && strlen(optarg) < (sizeof(configfile)-1))
            {
                strcpy(configfile,optarg);
            }
            else
            {
                printf("invalid Config File Parameter [-c Configfilename]: %s\n", optarg);
                exit(0);
            }
            break;
        }
    }
}

void showbitstring(char* title, uint8_t* data, int totallen, int anz)
{
    printf("%s. len %d: ", title, totallen);
    for (int i = 0; i < anz; i++)
        printf("%01X ", data[i]);
    printf("\n");
}

void showbytestring(char *title, uint8_t *data, int totallen, int anz)
{
    printf("%s. len % 4d: ",title, totallen);
    for(int i=0; i<anz; i++)
        printf("%02X ",data[i]);
    printf("\n");
}

void showbytestring16(char *title, uint16_t *data, int anz)
{
    printf("%s. len %d: ",title,anz);
    for(int i=0; i<anz; i++)
        printf("%04X ",data[i]);
    printf("\n");
}

void showbytestring32(char* title, uint32_t* data, int anz)
{
    printf("%s. len %d: ", title, anz);
    for (int i = 0; i < anz; i++)
        printf("%08X ", data[i]);
    printf("\n");
}

void showbytestringf(char* title, float* data, int totallen, int anz)
{
    printf("%s. len %d: ", title, totallen);
    for (int i = 0; i < anz; i++)
        printf("%7.4f ", data[i]);
    printf("\n");
}

// get own IP adress
char* ownIP()
{
    static char ip[20] = { 0 };

    struct ifaddrs* ifaddr, * ifa;
    int s;
    char host[NI_MAXHOST];

    if (getifaddrs(&ifaddr) == -1)
    {
        printf("cannot read own IP address, getifaddrs faield. Check Networking\n");
        return ip;
    }


    for (ifa = ifaddr; ifa != NULL; ifa = ifa->ifa_next)
    {
        if (ifa->ifa_addr == NULL)
            continue;

        s = getnameinfo(ifa->ifa_addr, sizeof(struct sockaddr_in), host, NI_MAXHOST, NULL, 0, NI_NUMERICHOST);

        if (ifa->ifa_addr->sa_family == AF_INET)
        {
            if (s != 0)
            {
                printf("getnameinfo() failed: %s\n", gai_strerror(s));
                printf("cannot read own IP address, getnameinfo failed: %s. Check Networking\n", gai_strerror(s));
                break;
            }
            if (strncmp(host, "127", 3) != 0)
            {
                strcpy(ip, host);
                break;
            }
        }
    }

    freeifaddrs(ifaddr);
    return ip;
}

char* cleanStr(char *s)
{
    if(s[0] > ' ') 
    {
        // remove trailing crlf
        for(size_t j=0; j<strlen(s); j++)
            if(s[j] == '\n' || s[j] == '\r') s[j] = 0;
        return s;    // nothing to do
    }

    for(size_t i=0; i<strlen(s); i++)
    {
        if(s[i] >= '0')
        {
            // i is on first character
            memmove(s,s+i,strlen(s)-i);
            s[strlen(s)-i] = 0;
            // remove trailing crlf
            for(size_t j=0; j<strlen(s); j++)
                if(s[j] == 'n' || s[j] == '\r') s[j] = 0;
            return s;
        }
    }
    return NULL;   // no text found in string
}

char *getword(char *s, int idx)
{
    if(idx == 0)
    {
        for(size_t i=0; i<strlen(s); i++)
        {
            if(s[i] < '0')
            {
                s[i] = 0;
                return s;
            }
        }
        return NULL;
    }

    for(size_t j=0; j<strlen(s); j++)
    {
        if(s[j] > ' ')
        {
            char *start = s+j;
            for(size_t k=0; k<strlen(start); k++)
            {
                if(start[k] == ' ' || start[k] == '\r' || start[k] == '\n')
                {
                    start[k] = 0;
                    return start;
                }
            }   
            return start;
        }
    }

    return NULL;
}

// read the value of an element from the config file
// Format:
// # ... comment
// ElementName-space-ElementValue
// the returned value is a static string and must be copied somewhere else
// before this function can be called again
char *getConfigElement(char *elemname)
{
    if(strlen(configfile) < 2) return NULL;

    static char s[501];
    int found = 0;
    char fn[1024];
    
    if(strlen(configfile) > 512)
    {
        printf("config file path+name too long: %s\n",configfile);
        exit(0);
    }
    strcpy(fn,configfile);
    
    if(fn[0] == '~')
    {
        struct passwd *pw = getpwuid(getuid());
        const char *homedir = pw->pw_dir;
        sprintf(fn,"%s%s",homedir,configfile+1);
    }

    //printf("read Configuration file %s\n",fn);
    FILE *fr = fopen(fn,"rb");
    if(!fr) 
    {
        printf("!!! Configuration file %s not found !!!\n",fn);
        exit(0);
    }

    while(1)
    {
        if(fgets(s,500,fr) == NULL) break;
        // remove leading SPC or TAB
        if(cleanStr(s) == 0) continue;
        // check if its a comment
        if(s[0] == '#') continue;
        // get word on index
        char *p = getword(s,0);
        if(!p) break;
        if(strcmp(p, elemname) == 0)
        {
            char val[500];
            if(*(s+strlen(p)+1) == 0) continue;
            p = getword(s+strlen(p)+1,1);
            if(!p) break;
            // replace , with .
            char *pkomma = strchr(p,',');
            if(pkomma) *pkomma = '.';
            strcpy(val,p);
            strcpy(s,val);
            found = 1;
            break;
        }

    }

    fclose(fr);
    if(found == 0) return NULL;
    return s;
}

void getConfigElement_double(char *elemname, double *pv, double multiplier)
{
    char *p = getConfigElement(elemname);
	if(p) *pv = atof(p) * multiplier;
}

void getConfigElement_longlong(char *elemname, long long *pv, double multiplier)
{
    char *p = getConfigElement(elemname);
	if(p) *pv = (long long)(atof(p) * multiplier);
}

void getConfigElement_int(char *elemname, int *pv, double multiplier)
{
    char *p = getConfigElement(elemname);
    if(p) *pv = (int)(atof(p) * multiplier);
}

float measure_samplerate(int id, int samp, int prt)
{
static unsigned long lastus[10];
static unsigned long samples[10];
static int f[10];
static int cnt[10];
static int first=1;

    if(first)
    {
        first = 0;
        for(int i=0; i<10; i++)
        {
            f[i] = 1;
            cnt[i] = 0;
        }
    }

    // measure sample rate
    struct timeval  tv;
    gettimeofday(&tv, NULL);
    if(f[id])
    {
        f[id] = 0;
        lastus[id] = tv.tv_sec * 1000000 + tv.tv_usec;
        samples[id] = samp;
    }
    samples[id] += samp;
    unsigned long tnow = tv.tv_sec * 1000000 + tv.tv_usec;
    
    float sr = 0;
    if(++(cnt[id]) >= prt)
    {
        cnt[id] = 0;
        sr = (samples[id]*1e6)/(tnow - lastus[id]);
        printf("%d: %.6f\n",id,sr/1e6);
    }
    return sr;
}

void measure_maxval(double v)
{
static double max = - 99999999;    

    v = fabs(v);
    if(v > max)
    {
        max = v;
        printf("maxval: %f\n",max);
    }
}
