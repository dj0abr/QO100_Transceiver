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

        static Bitmap bmBigWF, bmSmallWF;

        static color col = new color();

        // this threads handle udp RX and TX
        static Thread udprx_thread;
        static Thread udptx_thread;

        public static void InitUdp(int bsw, int bsh)
        {
            bigSpecW = bsw;
            bigSpecH = bsh;

            smallSpecW = bsw;
            smallSpecH = bsh;

            bigWFW = bsw;
            bigWFH = bsh;

            smallWFW = bsw;
            smallWFH = bsh;

            bmBigWF = new Bitmap(bigWFW, bigWFH);
            bmSmallWF = new Bitmap(smallWFW, smallWFH);

            // create thread for UDP RX
            udprx_thread = new Thread(new ThreadStart(Udprxloop));
            udprx_thread.Start();

            // create thread for UDP TX
            udptx_thread = new Thread(new ThreadStart(Udptxloop));
            udptx_thread.Start();
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

                        if (rxtype == 0)
                        {
                            // big Spectrum, mid values
                            int[] arr = getSpecArr(b);
                            drawBigSpec(arr);
                        }

                        if (rxtype == 1)
                        {
                            // small Spectrum
                            int[] arr = getSpecArr(b);
                            drawSmallSpec(arr);
                        }

                        if (rxtype == 2)
                        {
                            // big WF (raw - no mid - values)
                            int[] arr = getSpecArr(b);
                            drawBigWF(arr);
                        }

                        if (rxtype == 3)
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

        static Pen penline = new Pen(Brushes.LightGreen, 1);
        static Pen penmarker = new Pen(Brushes.Green, 2);
        static Pen penmarkerTX = new Pen(Brushes.Red, 2);
        static Brush brs = new SolidBrush(Color.FromArgb(60,60,60));
        static Pen penmarkerLN = new Pen(brs, 2);
        static Font rxtx = new Font("Verdana", 8.0f);
        static private Bandplan bp = new Bandplan();
        static void drawBigSpec(int[] arr)
        {
            int noiselevel = statics.noiselevel * 54/50;

            Bitmap bmbigspec = new Bitmap(bigSpecW, bigSpecH);
            using (Graphics gr = Graphics.FromImage(bmbigspec))
            {
                // Make a Polyline
                Point[] poly = new Point[arr.Length + 2];
                poly[0] = new Point(0, bigSpecH-1);
                poly[arr.Length + 2 - 1] = new Point(1120-1, bigSpecH-1);

                for (int i = 0; i < arr.Length; i++)
                {
                    int val = scaleY(arr[i], noiselevel, statics.maxlevel, bigSpecH);
                    poly[i + 1] = new Point(i, val);
                }

                gr.FillRectangle(Brushes.Black, 0, 0, bigSpecW, bigSpecH);
                gr.FillPolygon(Brushes.Blue, poly);
                gr.DrawPolygon(penline, poly);

                // vertical lines at specific frequencies
                penmarkerLN.DashPattern = new float[] { 1.0f, 1.0f };
                if (statics.bandplan_mode == 0)
                {
                    for (int i = 0; i < bp.be.Length - 1; i++)
                    {
                        int xs = qrgToPixelpos(bp.be[i].from);
                        int ws = qrgToPixelpos(bp.be[i + 1].from) - xs;
                        gr.DrawLine(penmarkerLN, xs,0,xs, bigSpecH);
                    }
                }
                else
                {
                    for (int qrg = 10489500; qrg <= 10490000; qrg += 50)
                    {
                        int spos = qrgToPixelpos(qrg);
                        gr.DrawLine(penmarkerLN, spos, 0, spos, bigSpecH);
                    }
                }

                // green vertical line at RX frequency
                // red vertical line at TX frequency
                int x = statics.RXoffset * 2 / 1000;
                int xtx = statics.TXoffset * 2 / 1000;
                int xydiff = Math.Abs(x - xtx);
                
                penmarker.DashPattern = new float[] { 2.0f, 2.0f };
                gr.DrawLine(penmarker, x, 20, x, bigSpecH);

                // red vertical line at TX frequency
                penmarkerTX.DashPattern = new float[] { 2.0f, 2.0f };
                gr.DrawLine(penmarkerTX, xtx, 24, xtx, bigSpecH-4);

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

            Bitmap bmsmallspec = new Bitmap(bigSpecW, smallSpecH);
            using (Graphics gr = Graphics.FromImage(bmsmallspec))
            {
                // Make a Polyline
                Point[] poly = new Point[arr.Length + 2];
                poly[0] = new Point(0, smallSpecH - 1);
                poly[arr.Length + 2 - 1] = new Point(1120 - 1, smallSpecH - 1);

                for (int i = 0; i < arr.Length; i++)
                {
                    int val = scaleY(arr[i], statics.noiselevel, statics.maxlevel, smallSpecH);
                    poly[i + 1] = new Point(i, val);
                }

                gr.FillRectangle(Brushes.Black, 0, 0, bigSpecW, smallSpecH);
                gr.FillPolygon(Brushes.Blue, poly);
                gr.DrawPolygon(penline, poly);

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
            int lineincrement = statics.cpuspeed+1;
            // create a new bitmap
            Bitmap bmnew = new Bitmap(bigWFW, bigWFH);
            using (Graphics gr = Graphics.FromImage(bmnew))
            {
                // copy existing bitmap into bmnew, n lines lower
                gr.DrawImage(bmBigWF, 0, lineincrement);

                for (int i = 0; i < arr.Length; i++)
                {
                    // scale color
                    int v = 255 - scaleY(arr[i], statics.noiselevel, statics.maxlevel*47/50, 255);
                    //Console.WriteLine(arr[i] + ": " + v);

                    SolidBrush br = new SolidBrush(col.getColor(v));
                    gr.FillRectangle(br, i, 0, i, lineincrement);
                }
            }

            // copy the new bitmap back
            using (Graphics grbm = Graphics.FromImage(bmBigWF))
                grbm.DrawImage(bmnew, 0, 0);

            /* TODO: maybe a faster solution based on this example:
             * 
            Rectangle rect = new Rectangle(0, 0, bigWFW, 1);
            BitmapData bmpData = bmbigline.LockBits(rect, ImageLockMode.ReadWrite, bmbigline.PixelFormat);

            // Get the address of the first line (the only line in this case)
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bmbigline.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            // Set every third value to 255. A 24bpp bitmap will look red.  
            for (int counter = 2; counter < rgbValues.Length; counter += 3)
                rgbValues[counter] = 255;

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            bmbigline.UnlockBits(bmpData);
            */

            using (Graphics gr = Graphics.FromImage(bmnew))
            {
                // green vertical line at RX frequency
                // red vertical line at TX frequency
                int x = statics.RXoffset * 2 / 1000;
                int xtx = statics.TXoffset * 2 / 1000;
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

                    SolidBrush br = new SolidBrush(col.getColor(v));
                    gr.FillRectangle(br, i, 0, i, lineincrement);
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
                gr.DrawLine(dotpen, smallWFW / 2, 0, smallWFW / 2, smallWFH);

                // 25Hz per pixel: 120px per 3kHz
                Pen dotgraypen = new Pen(Brushes.DarkGray, 1);
                dotgraypen.DashPattern = new float[] { 1.0f, 3.0f };
                for (int i=120; i<500; i+=120)
                {
                    gr.DrawLine(dotgraypen, smallWFW / 2 + i, 0, smallWFW / 2 + i, smallWFH);
                }
                for (int i = -120; i > -500; i -= 120)
                {
                    gr.DrawLine(dotgraypen, smallWFW / 2 + i, 0, smallWFW / 2 + i, smallWFH);
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

/*

        // Pipes for data transferred via UDP ports
        static UdpQueue uq_rx = new UdpQueue();
        static UdpQueue uq_tx = new UdpQueue();
        static UdpQueue uq_ctrl = new UdpQueue();
        static UdpQueue uq_fft = new UdpQueue();
        static UdpQueue uq_iq = new UdpQueue();
        static UdpQueue uq_rtty_rx = new UdpQueue();

        public static int searchtimeout = 0;
        static String last_audiodevstring = "";

        // Constructor
        // called when Udp is created by the main program
        public static void InitUdp()
        {
            // create thread for UDP RX
            udprx_thread = new Thread(new ThreadStart(Udprxloop));
            udprx_thread.Name = "Thread: oscardata UDP-RX";
            udprx_thread.Start();

            // create thread for UDP TX
            udptx_thread = new Thread(new ThreadStart(Udptxloop));
            udptx_thread.Name = "Thread: oscardata UDP-TX";
            udptx_thread.Start();
        }

        public static void Close()
        {
            try
            {
                udprx_thread.Abort();
                udptx_thread.Abort();
            }
            catch { }
        }

        // Udp RX Loop runs in its own thread

        static void Udprxloop()
        {
            int extIPcnt = 0;

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
                        // RemoteEndpoint.Port ... port
                        // b[0] ... Type of data
                        // b+1 ... Byte array containing the data
                        int rxtype = rxarr[0];
                        Byte[] b = new byte[rxarr.Length - 1];
                        Array.Copy(rxarr, 1, b, 0, b.Length);

                        // payload
                        if (rxtype == statics.udp_payload)
                        {
                            //Console.WriteLine("payload");
                            uq_rx.Add(b);
                        }

                        // Broadcast response
                        if (rxtype == statics.udp_bc)
                        {
                            String ModIP = RemoteEndpoint.Address.ToString();
                            if (ModIP != statics.MyIP)
                            {
                                // this is not the local IP
                                // wait for 3 Receptions before accepting it
                                if (extIPcnt < 3)
                                {
                                    //Console.WriteLine("myIP:"+statics.MyIP+" modem is ext IP:"+ModIP+", waiting:" + extIPcnt);
                                    if (extIPcnt < 4) extIPcnt++;
                                    if (extIPcnt < 3)
                                        continue;
                                }
                                //Console.WriteLine("modem is ext IP, accepted");
                            }
                            else
                            {
                                //Console.WriteLine("modem is local IP");
                                extIPcnt = 0;
                            }

                            statics.ModemIP = ModIP;
                            searchtimeout = 0;
                            // message b contains audio devices and init status

                            statics.initAudioStatus = (b[0] == '1') ? 2 : 0;
                            statics.initAudioStatus |= (b[1] == '1') ? 1 : 0;
                            statics.initVoiceStatus = (b[2] == '1') ? 2 : 0;
                            statics.initVoiceStatus |= (b[3] == '1') ? 1 : 0;

                            //String s = statics.ByteArrayToString(b,4);
                            String s = statics.ByteArrayToStringUtf8(b, 4);
                            String[] sa1 = s.Split(new char[] { '^' });
                            statics.AudioPBdevs = sa1[0].Split(new char[] { '~' });
                            statics.AudioCAPdevs = sa1[1].Split(new char[] { '~' });

                            // has the device list changed ?
                            if (s != last_audiodevstring)
                            {
                                statics.GotAudioDevices = 1;
                                last_audiodevstring = s;
                            }

                            if (statics.GotAudioDevices == 0)
                                statics.GotAudioDevices = 1;
                        }

                        // FFT data
                        if (rxtype == statics.udp_fft)
                        {
                            int idx = 0;
                            statics.PBfifousage = b[idx++];
                            statics.CAPfifousage = b[idx++];
                            statics.RXlevelDetected = b[idx++];
                            statics.RXinSync = b[idx++];
                            statics.maxRXlevel = b[idx++];
                            statics.maxTXlevel = b[idx++];
                            statics.tune_frequency = b[idx++];
                            statics.tune_frequency <<= 8;
                            statics.tune_frequency += b[idx++];
                            statics.rtty_txon = b[idx++];
                            //Console.WriteLine("f:" + statics.tune_frequency);
                            Byte[] b1 = new byte[b.Length - idx];
                            Array.Copy(b, idx, b1, 0, b1.Length);
                            drawFftBitmap(b1);
                        }

                        // IQ data
                        if (rxtype == statics.udp_iq)
                        {
                            Int16[] re = new Int16[b.Length / 2];
                            Int16[] im = new Int16[b.Length / 2];
                            int idx = 0;

                            for (int i = 0; i < b.Length; i += 4)
                            {
                                re[idx] = b[i + 0];
                                re[idx] <<= 8;
                                re[idx] += b[i + 1];

                                im[idx] = b[i + 2];
                                im[idx] <<= 8;
                                im[idx] += b[i + 3];
                                idx++;
                            }
                            drawBitmap(re, im);
                        }

                        if (rxtype == statics.udp_rtty_rx)
                        {
                            uq_rtty_rx.Add(b);
                        }
                    }
                }
                catch { }
            }
        }

        static int panelw = 75, panelh = 75;
        static Bitmap bm;
        const int maxsum = 5000;
        static Int16[] resum = new Int16[maxsum];
        static Int16[] imsum = new Int16[maxsum];
        static int sumidx = 0;
        static SolidBrush bgcol = new SolidBrush(Color.Silver);//FromArgb(255, (byte) 0x40, (byte) 0x00, (byte) 0x00));

        static double scaleiq(int v)
        {
            double f = v;
            f /= 15000.0;
            // f goes from -1 to +1
            // scale it to the graphics
            const int sz = 45;
            f += 1;
            f /= 2;
            f *= sz;
            f += (panelw - sz) / 2;
            return f;
        }

        static void drawBitmap(Int16[] re, Int16[] im)
        {
            // collect IQ data
            for (int i = 0; i < re.Length; i++)
            {
                if (sumidx < maxsum)
                {
                    resum[sumidx] = re[i];
                    imsum[sumidx] = im[i];
                    sumidx++;
                }
            }

            // check if there is space in bitmap fifo
            // if the GUI does not process the bitmaps fast enough, just cancel it
            if (uq_iq.Count() > 2)
                return;

            // bitmap for drawing the complete picture
            bm = new Bitmap(panelw, panelh);

            using (Graphics gr = Graphics.FromImage(bm))
            {
                // background
                gr.FillRectangle(bgcol, 0, 0, panelw, panelh);
                // oscilloscope screen
                gr.DrawImage(new Bitmap(Properties.Resources.screen), 2, 1);
                // draw constellation points
                for (int i = 0; i < sumidx; i++)
                {
                    if (resum[i] == 0 || imsum[i] == 0) continue;
                    double dist = Math.Sqrt((resum[i] * resum[i]) + (imsum[i] * imsum[i]));
                    if (dist > 22000) continue; // do not draw outside scope

                    double x = scaleiq(resum[i]);
                    double y = scaleiq(imsum[i]);

                    double et = 1.6;
                    x -= et;
                    y -= et;
                    double w = et * 2;
                    double h = et * 2;
                    gr.FillEllipse(Brushes.Yellow, (int)x, (int)y, (int)w, (int)h);
                }
            }

            uq_iq.Add(bm);
            sumidx = 0;
        }

        static int fftw = 410, ffth = 72;
        static Bitmap bmskala = new Bitmap(fftw, ffth);
        static bool bmf = false;
        static Font fnt = new Font("Verdana", 10.0f);
        static Font smallfnt = new Font("Verdana", 8.0f);
        static Pen penyl = new Pen(Brushes.Yellow, 1);
        static Pen pengn = new Pen(Brushes.LightGreen, 3);

        static void drawFftBitmap(Byte[] b1)
        {
            int yl = ffth - 20;
            int yh = 20;

            if (!bmf)
            {
                // pre-draw background
                bmf = true;
                Pen pen = new Pen(Brushes.Navy, 1);
                Pen pensolid = new Pen(Brushes.Navy, 1);
                Pen pensolidwhite = new Pen(Brushes.White, 1);
                pen.DashPattern = new float[] { 1.0F, 2.0F, 1.0F, 2.0F };
                Pen penred = new Pen(Brushes.Red, 1);
                Pen penred2 = new Pen(Brushes.Red, 2);

                using (Graphics gr = Graphics.FromImage(bmskala))
                {
                    gr.FillRectangle(bgcol, 0, 0, fftw, ffth);
                    gr.DrawImage(new Bitmap(Properties.Resources.osci), 0, 0);

                    for (int x = 10; x <= 390; x += 10)
                        gr.DrawLine(pen, x, yl, x, yh);

Pen pen = new Pen(Brushes.Navy, 1);
                    gr.DrawLine(penred2, 11, yl, 11, yh);
                    gr.DrawLine(penred, 150, yl, 150, yh);
                    gr.DrawLine(pensolid, 20, yl, 20, yh);
                    gr.DrawLine(pensolid, 280, yl, 280, yh);
                    gr.DrawLine(pensolid, 360, yl, 360, yh);

                    gr.DrawLine(pensolidwhite, 11, 12, 11, 20);
                    gr.DrawLine(pensolidwhite, 150, yl + 2, 150, yl + 7);
                    gr.DrawLine(pensolidwhite, 20, yl + 2, 20, yl + 7);
                    gr.DrawLine(pensolidwhite, 280, yl + 2, 280, yl + 7);
                    gr.DrawLine(pensolidwhite, 360, yl + 2, 360, yl + 7);

                    gr.DrawRectangle(penred, 15, yh, 270, yl - yh);

                    gr.DrawString("200", smallfnt, Brushes.DarkBlue, 8, yl + 7);
                    gr.DrawString("1500", smallfnt, Brushes.DarkBlue, 135, yl + 7);
                    gr.DrawString("2800", smallfnt, Brushes.DarkBlue, 267, yl + 7);
                    gr.DrawString("3600", smallfnt, Brushes.DarkBlue, 345, yl + 7);

                    gr.DrawString(statics.langstr[8], fnt, Brushes.White, 100, 0);

                    gr.DrawString("Ref", smallfnt, Brushes.LightCoral, 1, 0);
                }

                bmskala.MakeTransparent(Color.White);
            }

            // check if there is space in bitmap fifo
            // if the GUI does not process the bitmaps fast enough, just cancel it
            if (uq_fft.Count() > 2)
                return;

            // bitmap for drawing the complete picture
            bm = new Bitmap(442, 76);

            int rshift = 14;
            using (Graphics gr = Graphics.FromImage(bm))
            {
                // background
                gr.FillRectangle(bgcol, 0, 0, bm.Width, bm.Height);
                // scala
                gr.DrawImage(bmskala, 16, 2);

                if (statics.real_datarate == 45)
                {
                    // RTTY Markers
                    int low = (statics.tune_frequency - 170 / 2) / 10;
                    int high = (statics.tune_frequency + 170 / 2) / 10;
                    gr.DrawLine(pengn, low + rshift, yl, low + rshift, yh + 3);
                    gr.DrawLine(pengn, high + rshift, yl, high + rshift, yh + 3);
                }
                // spectrum
                int lastus = -1;
                // values
                for (int i = 0; i < b1.Length - 1; i += 2)
                {
                    int us = b1[i];
                    us <<= 8;
                    us += b1[i + 1];
                    double fus = 0;
                    if (us > 0)
                        fus = 35 * Math.Log10((double)us / 10);

                    us = (int)(fus - 5.0);
                    if (lastus != -1 && i > 0)
                        gr.DrawLine(penyl, i / 2 + rshift, 76 - lastus, i / 2 + 1 + rshift, 76 - us); // 15 istead of 16 to get it in exact position
                    lastus = us;
                }
            }
            uq_fft.Add(bm);
        }

        // Udp TX Loop runs in its own thread
        static void Udptxloop()
        {
            DateTime dt = DateTime.UtcNow;
            UdpClient udpc = new UdpClient();

            while (statics.running)
            {
                bool wait = true;
                if (uq_ctrl.Count() > 0)
                {
                    // Control Message: send immediately
                    Byte[] b = uq_ctrl.Getarr();
                    udpc.Send(b, b.Length, statics.ModemIP, statics.UdpTXport);
                    wait = false;
                }

                if (statics.PBfifousage < 3)
                {
                    // we need to send more payload data
                    // but never send faster than 1000 Byte/s
                    // because statics.PBfifousage may be updated too slow
                    //DateTime dtact = DateTime.UtcNow;
                    //TimeSpan ts = dtact - dt;
                    //if (ts.TotalMilliseconds > statics.UdpBlocklen)
                    {
                        if (uq_tx.Count() > 0)
                        {
                            Byte[] b = uq_tx.Getarr();
                            udpc.Send(b, b.Length, statics.ModemIP, statics.UdpTXport);
                            wait = false;
                            //dt = dtact;
                        }
                    }
                }
                if (wait) Thread.Sleep(1);
            }
        }

        public static void UdpBCsend(Byte[] b, String ip, int port)
        {
            UdpClient udpc = new UdpClient();
            udpc.EnableBroadcast = true;
            udpc.Send(b, b.Length, ip, port);
        }

        // send a Byte array via UDP
        // this function can be called from anywhere in the program
        // it transfers the data to the udp-tx thread via a thread-safe pipe
        public static void UdpSendData(Byte[] b)
        {
            uq_tx.Add(b);
        }

        public static void UdpSendCtrl(Byte[] b)
        {
            uq_ctrl.Add(b);
        }

        public static int GetBufferCount()
        {
            return uq_tx.Count();
        }

        public static int GetBufferCountCtrl()
        {
            return uq_ctrl.Count();
        }

        public static Byte[] UdpReceive()
        {
            if (uq_rx.Count() == 0) return null;

            return uq_rx.Getarr();
        }

        public static qpskitem UdpGetIQ()
        {
            if (uq_iq.Count() == 0) return null;

            return uq_iq.GetQPSKitem();
        }

        public static Bitmap UdpBitmap()
        {
            if (uq_iq.Count() == 0) return null;

            return uq_iq.GetBitmap();
        }

        public static Bitmap UdpFftBitmap()
        {
            if (uq_fft.Count() == 0) return null;

            return uq_fft.GetBitmap();
        }

        public static bool IQavail()
        {
            if (uq_iq.Count() == 0) return false;
            return true;
        }

        public static Byte[] getRTTYrx()
        {
            if (uq_rtty_rx.Count() == 0) return null;
            return uq_rtty_rx.Getarr();
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

        public void Add(qpskitem b)
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

        public qpskitem GetQPSKitem()
        {
            qpskitem b;

            lock (myQ.SyncRoot)
            {
                b = (qpskitem)myQ.Dequeue();
            }
            return b;
        }

        public qpskitem GetItem()
        {
            qpskitem b;

            lock (myQ.SyncRoot)
            {
                b = (qpskitem)myQ.Dequeue();
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

    public class qpskitem
    {
        public int re;
        public int im;
    }
}
*/