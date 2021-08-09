using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;

namespace trxGui
{
    public static class statics
    {
        public static UInt16 gui_serno = 169;   // 123 means: V1.23
        public static UInt16 driver_serno = 0;
        public static bool running = true;
        public static String ModemIP;
        public static int UdpTXport = 40821;
        public static int UdpRXport = 40820;
        public static bool ptt = false;
        public static bool pttkey = false;
        public static int GotAudioDevices = 0;
        public static String[] AudioPBdevs;
        public static String[] AudioCAPdevs;
        public static String AudioPBdev;
        public static String AudioCAPdev;
        public static bool newaudiodevs = false;
        public static int ostype;
        public static int noiselevel = 1000;
        public static int maxlevel = 32000;
        public static int plutousb = 1;
        public static String plutoaddress = "";
        public static int rxmouse = -1;
        public static bool rxmute = false;
        public static bool rit = true;
        public static bool xit = false;
        public static bool audioagc = false;
        public static bool compressor = false;
        public static int rxfilter = 3;
        public static int txfilter = 3;
        public static bool audioloop = false;
        public static bool rfloop = false;
        public static int beaconoffset = 0;
        public static bool beaconlock = false;
        public static int calmode = 0;  // 0=off, 1=439MHz cal
        public static int cpuspeed = 0;
        public static bool audioHighpass = false;
        public static int bandplan_mode = 0; // 0=QO100-Bandplan, 1=QO100-RX-QRGs, 2=QO100-TX-QRGs, 3=Pluto-RX-QRGs, 4=Pluto-TX-QRGs
        public static int language = 1;     // 0=en, 1=de
        public static int palette = 1;      // colors: 0=blue, 1=red, 2=green, 3=white
        public static int txpower = 0;
        public static int windowsize = 5;
        public static int panel_bigspec_Width, panel_bigspec_Height, panel_bigwf_Width, panel_bigwf_Height,
                        panel_smallspec_Width, panel_smallspec_Height, panel_smallwf_Width, panel_smallwf_Height;


        // Pluto frequency settings
        public static UInt32 rxqrg;             // baseband QRG of lower beacon, RX tuner = this value - 30kHz
        public static UInt32 txqrg;             // baseband QRG of lower beacon, TX tuner = this value - 30kHz
        public static int RXoffset = 280000;    // Tuner: 470 + Offset: 280 = 750kHz (mid Beacon)
        public static int TXoffset = 280000;    // Tuner: 470 + Offset: 280 = 750kHz (mid Beacon)
        public static int RXTXoffset = 0;       // offset between RX and TX
        public static int lastRXoffset = 280000;
        public static int lastTXoffset = 280000;
        public static UInt32 calbasefreq = 439000000; // base frequency for calibration
        public static UInt32 calfreq;           // same as rxqrg, but used during offset calibration
        public static int rfoffset = 0;         // Pluto's TCXO offset to RX/TX frequency
        public static int lnboffset = 0;        // LNB offset to RX frequency


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

        public static void sendBaseQRG(UInt32 newrx = 0)
        {
            UInt32 baserx = rxqrg;
            if (lnboffset > 0)
                baserx += (UInt32)lnboffset;
            if (lnboffset < 0)
                baserx -= (UInt32)(-lnboffset);

                Byte[] txb = new Byte[9];
            txb[0] = 8;
            if (newrx == 0)
            {
                //Console.WriteLine("*********************************** " + statics.rxqrg + " * " + statics.txqrg);
                txb[1] = (Byte)(baserx >> 24);
                txb[2] = (Byte)(baserx >> 16);
                txb[3] = (Byte)(baserx >> 8);
                txb[4] = (Byte)(baserx & 0xff);
            }
            else
            {
                //Console.WriteLine("*********************************** " + newrx + " * " + statics.txqrg);
                txb[1] = (Byte)(newrx >> 24);
                txb[2] = (Byte)(newrx >> 16);
                txb[3] = (Byte)(newrx >> 8);
                txb[4] = (Byte)(newrx & 0xff);
            }
            txb[5] = (Byte)(txqrg >> 24);
            txb[6] = (Byte)(txqrg >> 16);
            txb[7] = (Byte)(txqrg >> 8);
            txb[8] = (Byte)(txqrg & 0xff);
            Udp.UdpSendData(txb);
        }

        // Pluto TCXO correction value, for RX and TX
        public static void sendReferenceOffset(int hz = 0)
        {
            if (hz < -12000) hz = -12000;
            if (hz > 12000) hz = 12000;

            statics.rfoffset = hz;

            //Console.WriteLine("Set Clock Reference: " + statics.rfoffset);

            int val = hz + 12000;   // make it always positive

            Byte[] txb = new Byte[5];
            txb[0] = 15;
            txb[1] = (Byte)(val >> 24);
            txb[2] = (Byte)(val >> 16);
            txb[3] = (Byte)(val >> 8);
            txb[4] = (Byte)(val & 0xff);

            Udp.UdpSendData(txb);
        }

        // send tuned RX and TX offsets
        public static void sendRXTXoffset()
        {
            if (RXoffset < 0) RXoffset = 0;
            if (RXoffset > 560000) RXoffset = 560000;
            if (TXoffset < 0) TXoffset = 0;
            if (TXoffset > 560000) TXoffset = 560000;

            int rxoff = RXoffset;

            Byte[] txb = new Byte[9];
            txb[0] = 0;
            txb[1] = (Byte)(rxoff >> 24);
            txb[2] = (Byte)(rxoff >> 16);
            txb[3] = (Byte)(rxoff >> 8);
            txb[4] = (Byte)(rxoff & 0xff);
            txb[5] = (Byte)(TXoffset >> 24);
            txb[6] = (Byte)(TXoffset >> 16);
            txb[7] = (Byte)(TXoffset >> 8);
            txb[8] = (Byte)(TXoffset & 0xff);

            Udp.UdpSendData(txb);
        }

        static public void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                Process.Start("xdg-open", url);
            }
        }

        static public string RunExternalProgram(string filename, string arguments = null)
        {
            var process = new Process();

            process.StartInfo.FileName = filename;
            if (!string.IsNullOrEmpty(arguments))
                process.StartInfo.Arguments = arguments;

            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;

            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            var stdOutput = new StringBuilder();
            process.OutputDataReceived += (sender, args) => stdOutput.AppendLine(args.Data); // Use AppendLine rather than Append since args.Data is one line of output, not including the newline character.

            string stdError = null;
            try
            {
                process.Start();
                process.BeginOutputReadLine();
                stdError = process.StandardError.ReadToEnd();
                process.WaitForExit();
            }
            catch
            {
                return "";
            }

            if (process.ExitCode == 0)
                return stdOutput.ToString();

            return "";
        }

        static private string Format(string filename, string arguments)
        {
            return "'" + filename +
                ((string.IsNullOrEmpty(arguments)) ? string.Empty : " " + arguments) +
                "'";
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
            be[1] = new Bandentry(Color.FromArgb(0xff, 0x00, 0xaf, 0xef), 10489505, "CW", 10489510);
            be[2] = new Bandentry(Color.FromArgb(0xff, 0x6f, 0x2f, 0x9f), 10489540, "NB dig", 10489550);
            be[3] = new Bandentry(Color.FromArgb(0xff, 0xfe, 0xbf, 0x00), 10489580, "digital", 10489605);
            be[4] = new Bandentry(Color.FromArgb(0xff, 0x91, 0xcf, 0x4f), 10489650, "SSB only", 10489688);
            be[5] = new Bandentry(Color.FromArgb(0xff, 0xff, 0x00, 0x00), 10489745, "B", 10489748);
            be[6] = new Bandentry(Color.FromArgb(0xff, 0x91, 0xcf, 0x4f), 10489755, "SSB only", 10489790);
            be[7] = new Bandentry(Color.FromArgb(0xff, 0xc5, 0x59, 0x10), 10489850, "MIX", 10489850);
            be[8] = new Bandentry(Color.FromArgb(0xff, 0xb5, 0xa9, 0x10), 10489870, "MIXED + Contest", 10489905);
            be[9] = new Bandentry(Color.FromArgb(0xff, 0xff, 0x00, 0x00), 10489990, "B", 10489992);
            be[10] =new Bandentry(Color.FromArgb(0xff, 0xff, 0x00, 0x00),10490000, "---", 0);
        }
    }
}
