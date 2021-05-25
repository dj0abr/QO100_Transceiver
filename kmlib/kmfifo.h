// max number of fifo, just a high number, never really used
#define MAXFIFOS 200

typedef struct _FIFOOBJ_ {
    int maxelem;
    int maxelemlen;
    int rdidx;
    int wridx;
    void *fifomem;
    int *plen;   // real length of an element
    pthread_mutex_t crit_sec;
} FIFOOBJ;

int create_fifo(int elem_num, int elem_len);
void destroy_fifos();
void write_fifo(int id, uint8_t *pdata, int len);
int read_fifo(int id, uint8_t* pdata, int maxlen);
void fifo_clear(int id);
int fifo_freespace(int id);
int fifo_dataavail(int id);
int fifo_usedspace(int id);
