#pragma once
#include <cstdint>
#include <queue>
#include <mutex>

struct RigctlCommand {
  enum Type { SET_FREQ, SET_PTT } type;
  uint64_t freqHz;
  bool ptt;
};

// Thread-safe simple queue used to pass commands from rigctld server into main loop
class RigctlCommandQueue {
public:
  void push(const RigctlCommand &c) {
    std::lock_guard<std::mutex> lk(mutex_);
    q_.push(c);
  }

  bool pop(RigctlCommand &out) {
    std::lock_guard<std::mutex> lk(mutex_);
    if (q_.empty()) return false;
    out = q_.front(); q_.pop(); return true;
  }

private:
  std::queue<RigctlCommand> q_;
  std::mutex mutex_;
};

// Single global instance (simple approach)
RigctlCommandQueue &getRigctlQueue();
