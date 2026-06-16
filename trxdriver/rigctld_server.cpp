#include "rigctld_server.h"
#include "rigctl_commands.h"
#include <thread>
#include <atomic>
#include <string>
#include <sstream>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <sys/socket.h>
#include <unistd.h>
#include <cstring>
#include <iostream>

static std::atomic_bool s_running(false);
static std::thread s_thread;

static bool sendLine(int sock, const std::string &s) {
  size_t total = 0;
  const char *buf = s.c_str();
  size_t len = s.size();
  while (total < len) {
    ssize_t n = ::send(sock, buf + total, len - total, 0);
    if (n <= 0) return false;
    total += (size_t)n;
  }
  return true;
}

static bool recvLine(int sock, std::string &out) {
  out.clear();
  char c;
  while (true) {
    ssize_t n = ::recv(sock, &c, 1, 0);
    if (n <= 0) return false;
    if (c == '\n') break;
    if (c == '\r') continue;
    out.push_back(c);
  }
  return true;
}

// Hook functions for the main application
extern uint64_t rigctl_get_current_frequency_hz();
extern bool rigctl_get_current_ptt();

static void handleClient(int clientSock) {
  std::string line;
  printf("HandleClient running..\n");
  while (recvLine(clientSock, line)) {
    std::printf("rigctld: recv: %s\n", line.c_str());
    std::fflush(stdout);

    std::istringstream is(line);
    std::string cmd; is >> cmd;

    if (cmd == "f") { // get_freq
      uint64_t f = rigctl_get_current_frequency_hz();
      std::cout << "Hamlib Command get_freq received:\n" ;
      sendLine(clientSock, std::string("RPRT 0\nFREQ ") + std::to_string(f) + "\n");
    } else if (cmd == "F") { // set_freq
      uint64_t f=0; if (!(is >> f)) { sendLine(clientSock,"RPRT 1\n"); continue; }
      RigctlCommand c; c.type = RigctlCommand::SET_FREQ; c.freqHz = f;
      std::printf("rigctld: set_freq requested: %llu\n", (unsigned long long)f);
      std::fflush(stdout);
      getRigctlQueue().push(c);
      sendLine(clientSock, "RPRT 0\n");
    } else if (cmd == "t") { // get_ptt
	    std::cout << "Hamlib Command get_ppt received\n" ;
      bool p = rigctl_get_current_ptt();
      sendLine(clientSock, std::string("RPRT 0\nPTT ") + (p ? "1\n" : "0\n"));
    } else if (cmd == "T") { //set_ptt
      int v=-1; if (!(is >> v)) { sendLine(clientSock,"RPRT 1\n"); continue; }
      RigctlCommand c; c.type = RigctlCommand::SET_PTT; c.ptt = (v!=0);
      std::printf("rigctld: set_ptt requested: %d\n", v);
      std::fflush(stdout);
      getRigctlQueue().push(c);
      sendLine(clientSock, "RPRT 0\n");
      std::printf("rigctld: reply: RPRT 0\n");
      std::fflush(stdout);
    } else {
      sendLine(clientSock, "RPRT 1\n");
      std::printf("rigctld: reply: RPRT 1\n");
      std::fflush(stdout);
    }
  }
  close(clientSock);
}

static void serverLoop(int port) {
  int srv = socket(AF_INET, SOCK_STREAM, 0);
  
  if (srv < 0) return;
  int opt = 1; setsockopt(srv, SOL_SOCKET, SO_REUSEADDR, &opt, sizeof(opt));
  sockaddr_in addr{}; addr.sin_family = AF_INET; addr.sin_addr.s_addr = INADDR_ANY; addr.sin_port = htons(port);
  if (bind(srv, (sockaddr*)&addr, sizeof(addr)) != 0) { close(srv); return; }
  if (listen(srv, 4) != 0) { close(srv); return; }

  while (s_running) {
    sockaddr_in clientAddr; socklen_t len = sizeof(clientAddr);
    int client = accept(srv, (sockaddr*)&clientAddr, &len);
    if (client < 0) break;
    std::thread t(handleClient, client);
    t.detach();
  }
  close(srv);
}

void rigctld_start(int port) {
  if (s_running) return;
  s_running = true;
  s_thread = std::thread(serverLoop, port);
  printf("\nrigctld_server is running at port: %d\n",port);
}

void rigctld_stop() {
  if (!s_running) return;
  s_running = false;
  // wake accept() by connecting
  int s = socket(AF_INET, SOCK_STREAM, 0);
  if (s >= 0) {
    sockaddr_in a{}; a.sin_family = AF_INET; a.sin_port = htons(4532); a.sin_addr.s_addr = inet_addr("127.0.0.1");
    connect(s, (sockaddr*)&a, sizeof(a));
    close(s);
  }
  if (s_thread.joinable()) s_thread.join();
}

bool rigctld_is_running() { return s_running; }
