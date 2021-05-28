using System;
using System.Collections.Generic;
using System.Drawing;
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
        public static int TXoffset = 280000;    // Tuner: 470 + Offset: 280 = 750kHz (mid Beacon)
        public static bool ptt = false;
        public static bool pttkey = false;
    }

    public class Bandentry
    {
        public Color col;
        public int from;
        public String text;
        public int textpos;

        public Bandentry(Color c, int f, String txt, int tp)
        {
            col = c;
            from = f;
            text = txt;
            textpos = tp;
        }

    }

    public class Bandplan
    {
        public Bandentry[] be = new Bandentry[11];

        public Bandplan()
        {
            be[0] = new Bandentry(Color.FromArgb(255,255,0,0), 10489500, "B", 10489500);
            be[1] = new Bandentry(Color.FromArgb(0xff, 0x00, 0xaf, 0xef), 10489505, "CW only", 10489510);
            be[2] = new Bandentry(Color.FromArgb(0xff, 0x6f, 0x2f, 0x9f), 10489540, "NB digi", 10489550);
            be[3] = new Bandentry(Color.FromArgb(0xff, 0xfe, 0xbf, 0x00), 10489580, "digital", 10489605);
            be[4] = new Bandentry(Color.FromArgb(0xff, 0x91, 0xcf, 0x4f), 10489650, "SSB only", 10489688);
            be[5] = new Bandentry(Color.FromArgb(0xff, 0xff, 0x00, 0x00), 10489745, "B", 10489748);
            be[6] = new Bandentry(Color.FromArgb(0xff, 0x91, 0xcf, 0x4f), 10489755, "SSB only", 10489790);
            be[7] = new Bandentry(Color.FromArgb(0xff, 0xc5, 0x59, 0x10), 10489850, "MIXED", 10489850);
            be[8] = new Bandentry(Color.FromArgb(0xff, 0xb5, 0xa9, 0x10), 10489870, "MIXED + Contest", 10489905);
            be[9] = new Bandentry(Color.FromArgb(0xff, 0xff, 0x00, 0x00), 10489990, "B", 10489992);
            be[10] =new Bandentry(Color.FromArgb(0xff, 0xff, 0x00, 0x00),10490000, "---", 0);
        }
    }
}
