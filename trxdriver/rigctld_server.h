#pragma once

// Start/stop the minimal hamlib/rigctld-compatible server
void rigctld_start(int port = 4532);
void rigctld_stop();
bool rigctld_is_running();
