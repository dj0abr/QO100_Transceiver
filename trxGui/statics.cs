using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trxGui
{
    public static class statics
    {
        public static bool running = true;
        public static String ModemIP;
        public static int UdpTXport = 40821;
        public static int UdpRXport = 40820;
        public static int RXoffset = 280000;    // Tuner: 470 + Offset: 280 = 750kHz (mid Beacon)
    }
}
