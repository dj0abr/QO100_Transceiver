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
* ==========================================
* find a pluto connected via USB or Ethernet
* ==========================================
* 
*/

#include "../qo100trx.h"

char pluto_context_name[50];

int pluto_get_IP(char *url_IP)
{
    // === connect Pluto at a given IP address ===
    struct sockaddr_in sa;
    
    int res = inet_pton(AF_INET,url_IP+3,&(sa.sin_addr));
    if(res == 1)
    {
        // we have a valid pluto IP continue using this IP
        sprintf(pluto_context_name,"%s",url_IP);
        //printf("searching Pluto on %s\n",pluto_context_name);
        return 1;
    }
    //printf("cannot evaluate %s\n",url_IP);
    return 0;
}

int pluto_get_USB(char *sn)
{    
    char s[500];
    snprintf(s,499,"iio_info -s 2>/dev/null");
    s[499] = 0;

    pluto_context_name[0] = 0;
    FILE *fp = popen(s,"r");
    if(fp)
    {
        while (fgets(s, sizeof(s)-1, fp) != NULL) 
        {
            // get the USB id
            char usbid[50];
            char usbsn[100];

            char *hp = strstr(s,"[usb:");
            if(hp)
            {
                hp += 1;
                char *he = strchr(hp,']');
                if(he)
                {
                    *he = 0;
                    strncpy(usbid,hp,49);
                    usbid[sizeof(usbid)-1] = 0;

                    // read serial number
                    char *psn = strstr(s,"serial=");
                    if(psn)
                    { 
                        psn+=7;
                        char *spn = strchr(psn,' ');
                        if(spn)
                        {
                            *spn = 0;
                            strncpy(usbsn,psn,99);
                            usbsn[sizeof(usbsn)-1] = 0;
                            printf("PLUTO found, SN:%s ID:%s\n",usbsn,usbid);

                            // if no special pluto requested, then use the first found pluto
                            if(*sn == 0)
                            {
                                strcpy(pluto_context_name, usbid);
                                return 1;
                            }

                            // search for a specific SN
                            if(!strcmp(sn, usbsn))
                            {
                                strcpy(pluto_context_name, usbid);
                                return 1;
                            }
                        }
                        
                    }
                }
            }
        }
        pclose(fp);
    }
    else
        printf("cannot execute iio_info command\n");
    
    printf("no PLUTO found\n");
    return 0;
}