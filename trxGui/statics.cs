using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        public static int GotAudioDevices = 0;
        public static String[] AudioPBdevs;
        public static String[] AudioCAPdevs;
        public static String AudioPBdev;
        public static String AudioCAPdev;
        public static bool newaudiodevs = false;
        public static int ostype;
        public static int rxqrg;
        public static int txqrg;
        public static int noiselevel = 1000;
        public static int maxlevel = 32000;
        public static int plutousb = 1;
        public static String plutoaddress = "";
        public static int rxmouse = -1;
        public static bool rxmute = false;
        public static bool rit = true;
        public static bool xit = false;
        public static bool audioagc = false;
        public static int compressor = 0;
        public static int rxfilter = 3;
        public static int txfilter = 3;
        public static bool audioloop = false;
        public static bool rfloop = false;
        public static int rfoffset = 0;

        static Process cmdtrx = null;
        public static bool StartQO100trx(bool start = true)
        {
            if (statics.ostype == 0) return false;

            // kill old processes already running
            killall(cmdtrx, "trxdriver");

            if (start == true)
            {
                // starte Prozesse
                try
                {
                    if (!File.Exists("trxdriver")) return false;
                    cmdtrx = new Process();
                    cmdtrx.StartInfo.FileName = "trxdriver";

                    if (cmdtrx != null)
                    {
                        cmdtrx.StartInfo.WindowStyle = ProcessWindowStyle.Normal;// .Hidden;
                        cmdtrx.StartInfo.Arguments = "";
                        cmdtrx.Start();
                        Console.WriteLine("trxdriver started");
                        Thread.Sleep(100);
                    }
                }
                catch { return false; }
            }
            return true;
        }

        static Process cmdmixer = null;
        public static bool StartMixer(bool start = true)
        {
            if (statics.ostype == 0) return false;

            // kill old processes already running
            killall(cmdmixer, "pavucontrol");

            if (start == true)
            {
                // starte Prozesse
                try
                {
                    cmdmixer = new Process();
                    cmdmixer.StartInfo.FileName = "pavucontrol";

                    if (cmdmixer != null)
                    {
                        cmdmixer.StartInfo.WindowStyle = ProcessWindowStyle.Normal;// .Hidden;
                        cmdmixer.StartInfo.Arguments = "";
                        cmdmixer.Start();
                        Console.WriteLine("pavucontrol started");
                        Thread.Sleep(100);
                    }
                }
                catch { return false; }
            }
            return true;
        }

        static public void killall(Process cmd, String s)
        {
            // kill a Linux process
            try
            {
                if (cmd != null)
                    cmd.Kill();
                cmd = null;

                Process proc = new Process();
                proc.EnableRaisingEvents = false;
                proc.StartInfo.FileName = "killall";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.OutputDataReceived += (sender, args) => { };   // schreibe Output ins nichts
                proc.StartInfo.RedirectStandardError = true;
                proc.ErrorDataReceived += (sender, args) => { };   // schreibe Output ins nichts
                proc.StartInfo.Arguments = s;
                proc.Start();
                proc.WaitForExit();
                Thread.Sleep(100);
            }
            catch { }
        }

        public static string ByteArrayToStringUtf8(byte[] arr, int offset = 0)
        {
            Byte[] ba = new byte[arr.Length];
            int dst = 0;
            for (int i = offset; i < arr.Length; i++)
            {
                if (arr[i] != 0) ba[dst++] = arr[i];
            }
            Byte[] ban = new byte[dst];
            Array.Copy(ba, ban, dst);

            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();

            return enc.GetString(ban);
        }

        public static byte[] StringToByteArrayUtf8(string str)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            return enc.GetBytes(str);
        }

        public static String getHomePath(String subpath, String filename)
        {
            String deli = "/";
            if (statics.ostype == 0) deli = "\\";

            //String home = Application.UserAppDataPath;
            String home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            home = home + deli;
            try { Directory.CreateDirectory(home); } catch { }

            // if not exists, create subfolder "oscardata"
            if (subpath.Length > 0)
                try
                {
                    home += subpath + deli;
                    Directory.CreateDirectory(home);
                }
                catch { }

            return home + filename;
        }

        // Culture invariant conversion

        public static double MyToDouble(String s)

        {
            double r = 0;

            try
            {
                s = s.Replace(',', '.');
                r = Convert.ToDouble(s, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
            }
            return r;

        }
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
