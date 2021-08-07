using System;
using System.Collections;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Drawing.Imaging;

namespace trxGui
{
    public static class Udp
    {
        static int bigSpecW, bigSpecH, smallSpecW, smallSpecH;
        static int bigWFW, bigWFH, smallWFW, smallWFH;

        static public UdpQueue bigspecQ = new UdpQueue();
        static public UdpQueue smallspecQ = new UdpQueue();
        static public UdpQueue bigWFQ = new UdpQueue();
        static public UdpQueue smallWFQ = new UdpQueue();
        static public UdpQueue uq_tx = new UdpQueue();
        static public UdpQueue uq_rotary = new UdpQueue();

        static Bitmap bmBigWF, bmSmallWF;

        static color col = new color();

        // this threads handle udp RX and TX
        static Thread udprx_thread;
        static Thread udptx_thread;

        public static void InitUdp()
        {
            UpdateSize();

            // create thread for UDP RX
            udprx_thread = new Thread(new ThreadStart(Udprxloop));
            udprx_thread.Start();

            // create thread for UDP TX
            udptx_thread = new Thread(new ThreadStart(Udptxloop));
            udptx_thread.Start();
        }

        public static void UpdateSize()
        {
            bigSpecW = statics.panel_bigspec_Width;
            bigSpecH = statics.panel_bigspec_Height;

            smallSpecW = statics.panel_smallspec_Width;
            smallSpecH = statics.panel_smallspec_Height;

            bigWFW = statics.panel_bigwf_Width;
            bigWFH = statics.panel_bigwf_Height;

            smallWFW = statics.panel_smallwf_Width;
            smallWFH = statics.panel_smallwf_Height;

            bmBigWF = new Bitmap(bigWFW, bigWFH);
            bmSmallWF = new Bitmap(smallWFW, smallWFH);
        }

        static void Udptxloop()
        {
            DateTime dt = DateTime.UtcNow;
            UdpClient udpc = new UdpClient();

            while (statics.running)
            {
                bool wait = true;
                if (uq_tx.Count() > 0 && statics.ModemIP != null && statics.ModemIP.Length >= 7)
                {
                    // Control Message: send immediately
                    Byte[] b = uq_tx.Getarr();
                    udpc.Send(b, b.Length, statics.ModemIP, statics.UdpTXport);
                    wait = false;
                }

                if(wait) Thread.Sleep(10);
            }
        }

        static void Udprxloop()
        {
            // define UDP port
            UdpClient udpc = new UdpClient(statics.UdpRXport);
            udpc.Client.ReceiveTimeout = 100;

            while (statics.running)
            {
                try
                {
                    // receive data from UDP port
                    IPEndPoint RemoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
                    Byte[] rxarr = udpc.Receive(ref RemoteEndpoint);
                    if (rxarr != null)
                    {
                        // Data received:
                        // RemoteEndpoint.Address ... IP address of the sender
                        IPAddress ipad = RemoteEndpoint.Address;
                        statics.ModemIP = ipad.ToString();
                        //Console.WriteLine(statics.ModemIP);
                        // RemoteEndpoint.Port ... port
                        // b[0] ... Type of data
                        // b+1 ... Byte array containing the data
                        int rxtype = rxarr[0];
                        Byte[] b = new byte[rxarr.Length - 1];
                        Array.Copy(rxarr, 1, b, 0, b.Length);

                        //Console.WriteLine(rxtype);

                        // check bitmap queues to avoid unnecessary graphic operations
                        int q1 = bigspecQ.Count();
                        int q2 = smallWFQ.Count();
                        int q3 = bigWFQ.Count();
                        int q4 = smallspecQ.Count();

                        // for slower CPUs: allow only one graphics item in the buffer
                        // faster CPUs: allow more graphic updates
                        int maxfill = 1;
                        if (statics.cpuspeed == 0) maxfill = 2; // only for fast cpus

                        if (rxtype == 0 && q1 < maxfill)
                        {
                            // big Spectrum, mid values
                            int[] arr = getSpecArr(b);
                            drawBigSpec(arr);
                        }

                        if (rxtype == 1 && q4 < maxfill)
                        {
                            // small Spectrum
                            int[] arr = getSpecArr(b);
                            drawSmallSpec(arr);
                        }

                        if (rxtype == 2 && q3 < maxfill)
                        {
                            
                            // big WF (raw - no mid - values)
                            int[] arr = getSpecArr(b);
                            //DateTime dts = DateTime.UtcNow;
                            drawBigWF(arr);
                            //DateTime dtact = DateTime.UtcNow;
                            //TimeSpan ts = dtact - dts;
                            //Console.WriteLine("drawtime: [ms] " + ts.TotalMilliseconds);
                        }

                        if (rxtype == 3 && q2 < maxfill)
                        {
                            // small WF
                            int[] arr = getSpecArr(b);
                            drawSmallWF(arr);
                        }

                        if(rxtype == 4)
                        {
                            // b contains audio devices and init status
                            UInt16 driversn = b[0];
                            driversn <<= 8;
                            driversn += b[1];
                            //Console.WriteLine("Driver SN:" + driversn);
                            statics.driver_serno = driversn;
                            String s = statics.ByteArrayToStringUtf8(b, 6);
                            //Console.WriteLine("Audio Devices:" + s);
                            String[] sa1 = s.Split(new char[] { '^' });
                            statics.AudioPBdevs = sa1[0].Split(new char[] { '~' });
                            statics.AudioCAPdevs = sa1[1].Split(new char[] { '~' });
                            statics.GotAudioDevices = 1;
                        }

                        if (rxtype == 5)
                        {
                            int v = b[0];
                            v <<= 8;
                            v |= b[1];
                            v <<= 8;
                            v |= b[2];
                            v <<= 8;
                            v |= b[3];
                            statics.noiselevel = v * 50 / 51;

                            v = b[4];
                            v <<= 8;
                            v |= b[5];
                            v <<= 8;
                            v |= b[6];
                            v <<= 8;
                            v |= b[7];
                            statics.maxlevel = v * 55 / 50;

                            //Console.WriteLine("noiselevel: " + statics.noiselevel);
                        }

                        if (rxtype == 6)
                        {
                            int v = b[0];
                            v <<= 8;
                            v |= b[1];
                            v <<= 8;
                            v |= b[2];
                            v <<= 8;
                            v |= b[3];

                            statics.beaconoffset = v;
                            //Console.WriteLine("beaconoffset: " + statics.beaconoffset);
                        }

                        if (rxtype == 7)
                        {
                            // number of rotary steps for frequency setting
                            int steps = (int)b[0] - 128;
                            uq_rotary.Add(steps);
                        }
                    }
                }
                catch { }
            }
        }

        static int scaleY(int val, int valmin, int valmax, int max)
        {
            // scale
            int r = max * (val - valmin) / (valmax - valmin);

            // revers up/down
            return max - r;
        }

        static int scaleX(int x, int maxx, int width)
        {
            return x * width / maxx;
        }

        static int [] getSpecArr(Byte [] arr)
        {
            int [] sa = new int [arr.Length / 2];

            int didx = 0;
            for (int i = 0; i < arr.Length; i += 2)
            {
                UInt32 uv = arr[i];
                uv <<= 8;
                uv |= arr[i + 1];

                sa[didx++] = (int)uv;
            }
            return sa;
        }

        
        static Pen penmarker = new Pen(Brushes.Green, 2);
        static Pen penmarkerTX = new Pen(Brushes.Red, 2);
        static Brush brs = new SolidBrush(Color.FromArgb(60,60,60));
        static Pen penmarkerLN = new Pen(brs, 2);
        static Font rxtx = new Font("Verdana", 8.0f);
        static private Bandplan bp = new Bandplan();

        // palette
        static Color[] col_specfill = { Color.Blue, Color.FromArgb(255, 80, 80), Color.Green, Color.LightGray };
        static Color[] col_specline = { Color.LightGreen, Color.Yellow, Color.Cyan, Color.White };

        static SolidBrush[] br_spedFill = { new SolidBrush(col_specfill[0]), new SolidBrush(col_specfill[1]), new SolidBrush(col_specfill[2]), new SolidBrush(col_specfill[3]) };
        static Pen[] penline = { new Pen(col_specline[0], 1), new Pen(col_specline[1], 1), new Pen(col_specline[2], 1), new Pen(col_specline[3], 1) };

        static void drawBigSpec(int[] arr)
        {
            int noiselevel = statics.noiselevel * 54/50;

            Bitmap bmbigspec = new Bitmap(bigSpecW, bigSpecH);
            // using gibt Ressourcen von gr nicht frei !!! 
            // mono Update auf >= 6.12.xx erforderlich !!!
            using (Graphics gr = Graphics.FromImage(bmbigspec))
            {
                // Make a Polyline
                Point[] poly = new Point[arr.Length + 2];
                poly[0] = new Point(0, bigSpecH - 1);
                poly[arr.Length + 2 - 1] = new Point(bigSpecW - 1, bigSpecH - 1);

                for (int i = 0; i < arr.Length; i++)
                {
                    int val = scaleY(arr[i], noiselevel, statics.maxlevel, bigSpecH);
                    int xi = scaleX(i, arr.Length, bigSpecW);
                    poly[i + 1] = new Point(xi, val);
                }

                gr.FillRectangle(Brushes.Black, 0, 0, bigSpecW, bigSpecH);
                gr.FillPolygon(br_spedFill[statics.palette], poly);
                gr.DrawPolygon(penline[statics.palette], poly);

                // vertical lines at specific frequencies
                penmarkerLN.DashPattern = new float[] { 1.0f, 1.0f };
                if (statics.bandplan_mode == 0)
                {
                    for (int i = 0; i < bp.be.Length; i++)
                    {
                        int xs = qrgToPixelpos(bp.be[i].from);
                        xs = xs * bigSpecW / 1120;
                        gr.DrawLine(penmarkerLN, xs, 0, xs, bigSpecH);
                    }
                }
                else
                {
                    for (int qrg = 10489500; qrg <= 10490000; qrg += 50)
                    {
                        int spos = qrgToPixelpos(qrg);
                        spos = spos * bigSpecW / 1120;
                        gr.DrawLine(penmarkerLN, spos, 0, spos, bigSpecH);
                    }
                }

                // green vertical line at RX frequency
                // red vertical line at TX frequency
                int x = statics.RXoffset * 2 / 1000;
                x = x * bigSpecW / 1120;
                int xtx = statics.TXoffset * 2 / 1000;
                xtx = xtx * bigSpecW / 1120;
                int xydiff = Math.Abs(x - xtx);

                penmarker.DashPattern = new float[] { 2.0f, 2.0f };
                gr.DrawLine(penmarker, x, 20, x, bigSpecH);

                // red vertical line at TX frequency
                penmarkerTX.DashPattern = new float[] { 2.0f, 2.0f };
                gr.DrawLine(penmarkerTX, xtx, 24, xtx, bigSpecH - 4);

                if (xydiff > 10)
                {
                    gr.DrawString("RX", rxtx, Brushes.LightGreen, x - 6, 0);
                    gr.DrawString("TX", rxtx, Brushes.LightCoral, xtx - 6, 0);
                }
                else
                {
                    gr.DrawString("RX/TX", rxtx, Brushes.White, x - 15, 0);
                }
            }
            bigspecQ.Add(bmbigspec);
        }


        static int qrgToPixelpos(int qrg)
        {
            qrg -= 10489000;   // rest is kHz
            qrg -= 470;
            return qrg * 2;
        }

        static void drawSmallSpec(int[] arr)
        {
            Pen dotpen = new Pen(Brushes.Yellow, 1);
            dotpen.DashPattern = new float[] { 2.0f, 2.0f };

            Bitmap bmsmallspec = new Bitmap(smallSpecW, smallSpecH);
            using (Graphics gr = Graphics.FromImage(bmsmallspec))
            {
                // Make a Polyline
                Point[] poly = new Point[arr.Length + 2];
                poly[0] = new Point(0, smallSpecH - 1);
                poly[arr.Length + 2 - 1] = new Point(smallSpecW - 1, smallSpecH - 1);

                for (int i = 0; i < arr.Length; i++)
                {
                    int val = scaleY(arr[i], statics.noiselevel, statics.maxlevel, smallSpecH);
                    int xi = scaleX(i, arr.Length, smallSpecW);
                    poly[i + 1] = new Point(xi, val);
                }

                gr.FillRectangle(Brushes.Black, 0, 0, bigSpecW, smallSpecH);
                gr.FillPolygon(br_spedFill[statics.palette], poly);
                gr.DrawPolygon(penline[statics.palette], poly);

                // tuning (middle) line
                gr.DrawLine(dotpen, smallSpecW / 2, 0, smallSpecW / 2, smallSpecH);
            }
            smallspecQ.Add(bmsmallspec);
        }

        static int scaleYWF(int val, int valmin, int valmax, int max)
        {
            // scale
            return max * (val - valmin) / (valmax - valmin);
        }

        static void drawBigWF(int[] arr)
        {
            int lineincrement = statics.cpuspeed + 1;
            // create a new bitmap
            Bitmap bmnew = new Bitmap(bigWFW, bigWFH);
            using (Graphics gr = Graphics.FromImage(bmnew))
            {
                                
                // copy existing bitmap into bmnew, n lines lower
                gr.DrawImage(bmBigWF, 0, lineincrement);
                
                for (int i = 0; i < arr.Length; i++)
                {
                    // scale color
                    int v = 255 - scaleY(arr[i], statics.noiselevel, statics.maxlevel * 47 / 50, 255);
                    if (v < 0) v = 0;
                    if (v > 255) v = 255;
                    //Console.WriteLine(arr[i] + ": " + v);

                    int xi = scaleX(i, arr.Length, bigWFW);
                    gr.FillRectangle(col.getSolidBrush(v), xi, 0, xi, lineincrement);
                }
                
            }
            // copy the new bitmap back
            using (Graphics grbm = Graphics.FromImage(bmBigWF))
              grbm.DrawImage(bmnew, 0, 0);

            using (Graphics gr = Graphics.FromImage(bmnew))
            {
                // green vertical line at RX frequency
                // red vertical line at TX frequency
                int x = statics.RXoffset * 2 / 1000;
                x = x * bigWFW / 1120;
                int xtx = statics.TXoffset * 2 / 1000;
                xtx = xtx * bigWFW / 1120;
                int xydiff = Math.Abs(x - xtx);

                penmarker.DashPattern = new float[] { 2.0f, 2.0f };
                gr.DrawLine(penmarker, x, 4, x, bigWFH);

                // red vertical line at TX frequency
                penmarkerTX.DashPattern = new float[] { 2.0f, 2.0f };
                gr.DrawLine(penmarkerTX, xtx, 0, xtx, bigWFH - 4);
            }

            bigWFQ.Add(bmnew);
        }

        static void drawSmallWF(int[] arr)
        {
            int lineincrement = statics.cpuspeed + 1;

            // create a new bitmap
            Bitmap bmnew = new Bitmap(smallWFW, smallWFH);
            using (Graphics gr = Graphics.FromImage(bmnew))
            {
                // copy existing bitmap into bmnew, one line lower
                gr.DrawImage(bmSmallWF, 0, lineincrement);

                for (int i = 0; i < arr.Length; i++)
                {
                    // scale color
                    int v = 255 - scaleY(arr[i], statics.noiselevel * 48/50, statics.maxlevel * 47 / 50, 255);
                    //Console.WriteLine(arr[i] + ": " + v);

                    int xi = scaleX(i, arr.Length, smallWFW);
                    gr.FillRectangle(col.getSolidBrush(v), xi, 0, xi, lineincrement);
                }
            }

            // copy the new bitmap back
            using (Graphics grbm = Graphics.FromImage(bmSmallWF))
            {
                grbm.DrawImage(bmnew, 0, 0);
            }

            // draw scales after scrolling
            using (Graphics gr = Graphics.FromImage(bmnew))
            {
                // tuning (middle) line
                Pen dotpen = new Pen(Brushes.Yellow, 1);
                dotpen.DashPattern = new float[] { 2.0f, 2.0f };
                int xi = smallWFW / 2;
                gr.DrawLine(dotpen, xi, 0, xi, smallWFH);

                // 25Hz per pixel: 120px per 3kHz
                Pen dotgraypen = new Pen(Brushes.DarkGray, 1);
                dotgraypen.DashPattern = new float[] { 1.0f, 3.0f };
                for (int i=120; i<500; i+=120)
                {
                    xi = smallWFW / 2 + i * smallWFW / 1120;
                    gr.DrawLine(dotgraypen, xi, 0, xi, smallWFH);
                }
                for (int i = -120; i > -500; i -= 120)
                {
                    xi = smallWFW / 2 + i * smallWFW / 1120;
                    gr.DrawLine(dotgraypen, xi, 0, xi, smallWFH);
                }
            }

            smallWFQ.Add(bmnew);
        }

        public static Bitmap getBigSpecBitmap()
        {
            if (bigspecQ.Count() == 0) return null;

            return bigspecQ.GetBitmap();
        }

        public static bool getBigSpecBitmap_avail()
        {
            if (bigspecQ.Count() == 0) return false;
            return true;
        }

        public static Bitmap getSmallSpecBitmap()
        {
            if (smallspecQ.Count() == 0) return null;

            return smallspecQ.GetBitmap();
        }

        public static bool getSmallSpecBitmap_avail()
        {
            if (smallspecQ.Count() == 0) return false;
            return true;
        }

        public static Bitmap getBigWFBitmap()
        {
            if (bigWFQ.Count() == 0) return null;

            return bigWFQ.GetBitmap();
        }

        public static bool getBigWFBitmap_avail()
        {
            if (bigWFQ.Count() == 0) return false;
            return true;
        }

        public static Bitmap getSmallWFBitmap()
        {
            if (smallWFQ.Count() == 0) return null;

            return smallWFQ.GetBitmap();
        }

        public static bool getSmallWFBitmap_avail()
        {
            if (smallWFQ.Count() == 0) return false;
            return true;
        }

        public static void UdpSendData(Byte[] b)
        {
            uq_tx.Add(b);
        }

        public static int GetRotary()
        {
            if (uq_rotary.Count() == 0) return 0;
            return uq_rotary.Getint();
        }
    }

    // this class is a thread safe queue wich is used
    // to exchange data with the UDP RX/TX threads
    public class UdpQueue
    {
        Queue myQ = new Queue();

        public void Add(Byte[] b)
        {
            lock (myQ.SyncRoot)
            {
                myQ.Enqueue(b);
            }
        }

        public void Add(int b)
        {
            lock (myQ.SyncRoot)
            {
                myQ.Enqueue(b);
            }
        }

        public void Add(Bitmap bm)
        {
            lock (myQ.SyncRoot)
            {
                myQ.Enqueue(bm);
            }
        }

        public Bitmap GetBitmap()
        {
            Bitmap b;

            lock (myQ.SyncRoot)
            {
                b = (Bitmap)myQ.Dequeue();
            }
            return b;
        }

        public Byte[] Getarr()
        {
            Byte[] b;

            lock (myQ.SyncRoot)
            {
                b = (Byte[])myQ.Dequeue();
            }
            return b;
        }

        public int Getint()
        {
            int b;

            lock (myQ.SyncRoot)
            {
                b = (int)myQ.Dequeue();
            }
            return b;
        }

        public int Count()
        {
            int result;

            lock (myQ.SyncRoot)
            {
                result = myQ.Count;
            }
            return result;
        }

        public void Clear()
        {
            lock (myQ.SyncRoot)
            {
                myQ.Clear();
            }
        }
    }
}
