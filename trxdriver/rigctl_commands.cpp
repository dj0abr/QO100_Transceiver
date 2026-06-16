#include "rigctl_commands.h"

static RigctlCommandQueue g_queue;

RigctlCommandQueue &getRigctlQueue() { return g_queue; }

