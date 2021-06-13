using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace trxGui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // set the position of the GUI elements
            // this is required because "mono" does some strange/wrong scaling

            load_Setup();

            panel_bigwf.MouseWheel += panel_bigwf_MouseWheel;
            panel_bigspec.MouseWheel += panel_bigwf_MouseWheel;

            panel_smallwf.MouseWheel += panel_smallwf_MouseWheel;
            panel_smallspec.MouseWheel += panel_smallwf_MouseWheel;

            this.Width = 1155;
            this.Height = 790;

            panel_qrg.Location = new Point(13, 4);
            panel_qrg.Width = 1092;
            panel_qrg.Height = 40;

            panel_sync.Location = new Point(panel_qrg.Location.X + panel_qrg.Width + 4, panel_qrg.Location.Y);
            panel_sync.Width = 1120 - (panel_qrg.Location.X + panel_qrg.Width-4);

            panel_beaconlock.Location = new Point(panel_qrg.Location.X + panel_qrg.Width + 4, panel_sync.Location.Y + panel_sync.Height + 4);
            panel_beaconlock.Width = panel_sync.Width;
            panel_beaconlock.Height = panel_sync.Height;

            panel_bigspec.Width = 1120;
            panel_bigspec.Height = 150;

            panel_bandplan.Width = 1120;
            panel_bandplan.Height = 16;
            
            panel_bigwf.Width = 1120;
            panel_bigwf.Height = 150;

            panel_rxline.Width = 1120;
            panel_rxline.Height = 15;

            panel_smallspec.Width = 1120;
            panel_smallspec.Height = 150;

            panel_smallqrg.Width = 1120;
            panel_smallqrg.Height = 16;

            panel_smallwf.Width = 1120;
            panel_smallwf.Height = 150;

            
            panel_bigspec.Location = new Point(13, panel_qrg.Location.Y + panel_qrg.Height+5);
            panel_bandplan.Location = new Point(13, panel_bigspec.Location.Y + panel_bigspec.Height);
            panel_bigwf.Location = new Point(13, panel_bandplan.Location.Y + panel_bandplan.Height);

            panel_rxline.Location = new Point(13, panel_bigwf.Location.Y + panel_bigwf.Height);

            panel_smallspec.Location = new Point(13, panel_rxline.Location.Y + panel_rxline.Height);
            panel_smallqrg.Location = new Point(13, panel_smallspec.Location.Y + panel_smallspec.Height);
            panel_smallwf.Location = new Point(13, panel_smallqrg.Location.Y + panel_smallqrg.Height);

            // button panel positions
            int xspace = 4;

            panel_txhighpass.Size = panel_pavucontrol.Size = panel_rxfilter.Size = panel_txfilter.Size = panel_rfloop.Size = panel_audioloop.Size = panel_comp.Size = panel_setup.Size = panel_info.Size = panel_rit.Size = panel_xit.Size = panel_copyRtoT.Size = panel_copyTtoR.Size = panel_agc.Size = panel_txmute.Size =new Size(48, 48);
            panel_rit.Location = new Point(13, panel_smallwf.Location.Y + panel_smallwf.Height + 4);
            panel_xit.Location = new Point(panel_rit.Location.X + panel_rit.Width + xspace, panel_rit.Location.Y);

            panel_copyRtoT.Location = new Point(panel_xit.Location.X + panel_xit.Width + xspace+6, panel_rit.Location.Y);
            panel_copyTtoR.Location = new Point(panel_copyRtoT.Location.X + panel_copyRtoT.Width + xspace, panel_rit.Location.Y);

            panel_agc.Location = new Point(panel_copyTtoR.Location.X + panel_copyTtoR.Width + xspace+6, panel_rit.Location.Y);
            panel_txmute.Location = new Point(panel_agc.Location.X + panel_agc.Width + xspace, panel_rit.Location.Y);
            panel_comp.Location = new Point(panel_txmute.Location.X + panel_txmute.Width + xspace, panel_rit.Location.Y);

            panel_rxfilter.Location = new Point(panel_comp.Location.X + panel_comp.Width + xspace+6, panel_rit.Location.Y);
            panel_txfilter.Location = new Point(panel_rxfilter.Location.X + panel_rxfilter.Width + xspace, panel_rit.Location.Y);
            panel_txhighpass.Location = new Point(panel_txfilter.Location.X + panel_txfilter.Width + xspace, panel_rit.Location.Y);

            panel_audioloop.Location = new Point(panel_txhighpass.Location.X + panel_txhighpass.Width + xspace+6, panel_rit.Location.Y);
            panel_rfloop.Location = new Point(panel_audioloop.Location.X + panel_audioloop.Width + xspace, panel_rit.Location.Y);

            panel_info.Location = new Point(panel_bigspec.Location.X + panel_bigspec.Width - panel_info.Width, panel_rit.Location.Y);
            panel_setup.Location = new Point(panel_info.Location.X - panel_setup.Width - 5, panel_rit.Location.Y);
            panel_pavucontrol.Location = new Point(panel_setup.Location.X - panel_pavucontrol.Width - 5, panel_rit.Location.Y);

            // PTT Panel
            panel1.Location = new Point(panel_rfloop.Location.X + panel_rfloop.Width + xspace+6, panel_rit.Location.Y);
            panel1.Size = new Size(panel_pavucontrol.Location.X - panel1.Location.X - 6, panel_rfloop.Height);

            // test OS type
            OperatingSystem osversion = System.Environment.OSVersion;
            if (osversion.VersionString.Contains("indow"))
                statics.ostype = 0; // Win$
            else
                statics.ostype = 1; // Linux

            // if this program was started from another loacation
            // set the working directory to the path of the .exe file
            // so it can find hsmodem(.exe)
            try
            {
                String s = System.Reflection.Assembly.GetExecutingAssembly().Location;
                s = Path.GetDirectoryName(s);
                Directory.SetCurrentDirectory(s);
                Console.WriteLine("working path: " + s);
            }
            catch (Exception e)
            {
                Console.WriteLine("cannot set working path: " + e.ToString());
            }

            Udp.InitUdp(panel_bigspec.Width, panel_bigspec.Height);

            statics.StartQO100trx();

            timer_draw.Start();

            sendAndRefreshRXTXoffset();
        }

        private void panel_bigspec_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bm = Udp.getBigSpecBitmap();

            if (bm != null)
            {
                e.Graphics.DrawImage(bm, 0, 0);
                bm.Dispose();
            }

            while (Udp.getBigSpecBitmap() != null);
        }

        private void timer_draw_Tick(object sender, EventArgs e)
        {
            if (Udp.getBigSpecBitmap_avail())
                panel_bigspec.Invalidate();

            if (Udp.getSmallSpecBitmap_avail())
                panel_smallspec.Invalidate();

            if (Udp.getBigWFBitmap_avail())
                panel_bigwf.Invalidate();

            if (Udp.getSmallWFBitmap_avail())
                panel_smallwf.Invalidate();

            if (Control.ModifierKeys == Keys.Shift && !statics.pttkey)
            {
                // PTT pressed (shift key)
                statics.pttkey = true;
                panel1.Invalidate();
            }
            if (Control.ModifierKeys == Keys.None && statics.pttkey)
            {
                // PTT released (shift key)
                statics.pttkey = false;
                statics.ptt = false;
                panel1.Invalidate();
            }

            if (statics.GotAudioDevices == 1)
            {
                // just got the list of audio devices
                statics.GotAudioDevices = 0;

                // check if the current selected device is in the new device list
                bool pbfound = false;
                foreach(String s in statics.AudioPBdevs)
                {
                    if(s == statics.AudioPBdev)
                    {
                        pbfound = true;
                        break;
                    }
                }

                bool capfound = false;
                foreach (String s in statics.AudioCAPdevs)
                {
                    if (s == statics.AudioCAPdev)
                    {
                        capfound = true;
                        break;
                    }
                }

                if(!pbfound)
                {
                    Console.WriteLine("NO Playback Device found, using default");
                    if (statics.AudioPBdevs.Length == 0)
                        statics.AudioPBdev = "Default";
                    else
                        statics.AudioPBdev = statics.AudioPBdevs[0];
                }

                if (!capfound)
                {
                    Console.WriteLine("NO Captur Device found, using default");
                    if (statics.AudioCAPdevs.Length == 0)
                        statics.AudioCAPdev = "Default";
                    else
                        statics.AudioCAPdev = statics.AudioCAPdevs[0];
                }

                statics.newaudiodevs = true;
                sendAudioDevs();
                statics.sendBaseQRG();
                sendPlutoAddress();
                panel_comp_Click(null, null);   // send "compress"
                panel_agc_Click(null, null);   // send "AGC"
                panel_txmute_Click(null, null);
                panel_rxfilter_Click(null, null);
                panel_txfilter_Click(null, null);
                statics.sendReferenceOffset(statics.rfoffset);
                sendCpuSpeed();
                this.Text += " GUI: " + formatSN(statics.gui_serno) + " Driver: " + formatSN(statics.driver_serno);
                if(statics.gui_serno != statics.driver_serno)
                {
                    MessageBox.Show("Warning!\nGUI and Driver have different serial numbers. Please re-install this software", "Version Number Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                panel_beaconlock.Invalidate();
            }

            if(statics.beaconoffset != oldbcnoffset)
            {
                oldbcnoffset = statics.beaconoffset;
                panel_qrg.Invalidate();
            }
        }

        private String formatSN(int sn)
        {
            String s = "V";

            s += (sn / 100).ToString();
            s += ".";
            s += (sn - (sn / 100) * 100).ToString();

            return s;
        }

        int oldbcnoffset = -1;

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            statics.StartQO100trx(false);
            save_Setup();
            statics.running = false;
        }

        private void panel_smallspec_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bm = Udp.getSmallSpecBitmap();

            if (bm != null)
            {
                e.Graphics.DrawImage(bm, 0, 0);
                bm.Dispose();
            }

            while (Udp.getSmallSpecBitmap() != null) ;
        }

        private void panel_bigwf_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bm = Udp.getBigWFBitmap();

            if (bm != null)
            {
                e.Graphics.DrawImage(bm, 0, 0);
                bm.Dispose();
            }

            while (Udp.getBigWFBitmap() != null) ;
        }

        private void panel_smallwf_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bm = Udp.getSmallWFBitmap();

            if (bm != null)
            {
                e.Graphics.DrawImage(bm, 0, 0);
                bm.Dispose();
            }

            while (Udp.getSmallWFBitmap() != null) ;
        }

        private void panel_bigwf_MouseClick(object sender, MouseEventArgs e)
        {
            tuneBig(e);
        }

        private void panel_smallwf_MouseClick(object sender, MouseEventArgs e)
        {
            tuneSmall(e);
        }

        private void tuneBig(MouseEventArgs e)
        {
            

            if (e.Button == MouseButtons.Left)
            {
                if (statics.calmode == 0)
                {
                    statics.RXoffset = e.X * 500;
                    sendAndRefreshRXTXoffset();
                }
                else if (statics.calmode == 1)
                {
                    // cal to BPSK beacon
                    int off = e.X * 500 - 280000;
                    //Console.WriteLine("Pluto offset: " + off + " Hz");
                    if(off > 0)
                        statics.calfreq += (UInt32)off;
                    else if (off < 0)
                        statics.calfreq -= (UInt32)(-off);

                    statics.sendBaseQRG(statics.calfreq);
                }
                else if (statics.calmode == 2)
                {
                    int off = e.X * 500 - 280000;
                    Console.WriteLine("offset: " + off);
                    statics.lnboffset += off;
                    statics.sendBaseQRG();
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                statics.RXoffset = e.X * 500;
                statics.TXoffset = e.X * 500;
                sendAndRefreshRXTXoffset();
            }
        }

        private void sendAndRefreshRXTXoffset()
        {
            statics.sendRXTXoffset();

            panel_qrg.Invalidate();
            panel_rxline.Invalidate();
        }

        private void tuneSmall(MouseEventArgs e)
        {
            // small WF: 25Hz per pixel, with the tune qrg in the middle
            int hz = (e.X - 560) * 25;
            

            if (e.Button == MouseButtons.Left)
            {
                if (statics.calmode == 0)
                {
                    statics.RXoffset += hz;
                    sendAndRefreshRXTXoffset();
                }
                else if (statics.calmode == 1)
                {
                    //Console.WriteLine("Pluto offset: " + hz);
                    if (hz > 0)
                        statics.calfreq += (UInt32)hz;
                    else if (hz < 0)
                        statics.calfreq -= (UInt32)(-hz);

                    statics.sendBaseQRG(statics.calfreq);
                    //Console.WriteLine("calfreq: " + statics.calfreq);
                }
                else if (statics.calmode == 2)
                {
                    Console.WriteLine("offset: " + hz);
                    statics.lnboffset += hz;
                    statics.sendBaseQRG();
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                statics.RXoffset += hz;
                statics.TXoffset += hz;
                sendAndRefreshRXTXoffset();
            }
        }

        private void panel_bigwf_MouseWheel(object sender, MouseEventArgs e)
        {
            int factor = 100;
            if (Control.ModifierKeys == Keys.Alt) factor = 1000;
            if (Control.ModifierKeys == Keys.Control) factor = 10000;

            if (statics.rit)
            {
                if (e.Delta > 0) //wheel direction
                    statics.RXoffset += factor;
                else
                    statics.RXoffset -= factor;

            }
            if (statics.xit)
            {
                if (e.Delta > 0) //wheel direction
                    statics.TXoffset += factor;
                else
                    statics.TXoffset -= factor;
            }

            if (statics.rit || statics.xit)
                sendAndRefreshRXTXoffset();
        }

        private void panel_smallwf_MouseWheel(object sender, MouseEventArgs e)
        {
            if (statics.rit)
            {
                if (e.Delta > 0) //wheel direction
                    statics.RXoffset += 10;
                else
                    statics.RXoffset -= 10;
            }

            if (statics.xit)
            {
                if (e.Delta > 0) //wheel direction
                    statics.TXoffset += 10;
                else
                    statics.TXoffset -= 10;
            }

            if (statics.rit || statics.xit)
                sendAndRefreshRXTXoffset();
        }

        private void panel_bigwf_MouseHover(object sender, EventArgs e)
        {
            panel_bigwf.Focus();
        }

        private void panel_bigwf_MouseMove(object sender, MouseEventArgs e)
        {
            panel_bigwf.Focus();
            statics.rxmouse = e.X * 500;
            panel_qrg.Invalidate();
        }

        private void panel_bigspec_MouseLeave(object sender, EventArgs e)
        {
            statics.rxmouse = -1;
            panel_qrg.Invalidate();
        }

        private void panel_bigspec_MouseMove(object sender, MouseEventArgs e)
        {
            panel_bigspec.Focus();
            statics.rxmouse = e.X * 500;
            panel_qrg.Invalidate();
        }

        private void panel_smallspec_MouseLeave(object sender, EventArgs e)
        {
            statics.rxmouse = -1;
            panel_qrg.Invalidate();
        }

        private void panel_smallspec_MouseMove(object sender, MouseEventArgs e)
        {
            panel_smallspec.Focus();
            int hz = (e.X - 560) * 25;
            statics.rxmouse = statics.RXoffset + hz;
            panel_qrg.Invalidate();
        }

        private void panel_smallwf_MouseLeave(object sender, EventArgs e)
        {
            statics.rxmouse = -1;
            panel_qrg.Invalidate();
        }

        private void panel_smallwf_MouseMove(object sender, MouseEventArgs e)
        {
            panel_smallwf.Focus();
            int hz = (e.X - 560) * 25;
            statics.rxmouse = statics.RXoffset + hz;
            panel_qrg.Invalidate();
        }

        private void panel_bigwf_MouseLeave(object sender, EventArgs e)
        {
            statics.rxmouse = -1;
            panel_qrg.Invalidate();
        }

        private void panel_bigspec_MouseHover(object sender, EventArgs e)
        {
            panel_bigspec.Focus();
        }

        private void panel_smallspec_MouseHover(object sender, EventArgs e)
        {
            panel_smallspec.Focus();
        }

        private void panel_smallwf_MouseHover(object sender, EventArgs e)
        {
            panel_smallwf.Focus();
        }

        Font smallfnt = new Font("Verdana", 8.0f);
        private void panel_smallqrg_Paint(object sender, PaintEventArgs e)
        {
            using(Graphics gr = e.Graphics)
            {
                String s = "0 Hz";
                gr.DrawString(s, smallfnt, Brushes.Black, panel_smallqrg.Width / 2 - 10, 0);
                for(int i=120; i<500; i+=120)
                {
                    s = (i * 25 / 1000).ToString() + "kHz";
                    gr.DrawString(s, smallfnt, Brushes.Black, i + panel_smallqrg.Width / 2 - 10, 0);
                }
                for (int i = -120; i > -500; i -= 120)
                {
                    s = (i * 25 / 1000).ToString() + "kHz";
                    gr.DrawString(s, smallfnt, Brushes.Black, i + panel_smallqrg.Width / 2 - 10, 0);
                }
            }
        }

        int qrgToPixelpos(int qrg)
        {
            qrg -= 10489000;   // rest is kHz
            qrg -= 470;
            return qrg*2;
        }

        Font qrgfnt = new Font("Verdana", 9.0f);
        private void panel_bandplan_Paint(object sender, PaintEventArgs e)
        {
            Bandplan bp = new Bandplan();
            using (Graphics gr = e.Graphics)
            {
                gr.FillRectangle(new SolidBrush(Color.DarkBlue), 0, 0, panel_bandplan.Width, panel_bandplan.Height);
                for (int i=0; i < bp.be.Length-1; i++)
                {
                    int x = qrgToPixelpos(bp.be[i].from);
                    int w = qrgToPixelpos(bp.be[i + 1].from) - x;
                    Color col = bp.be[i].col;
                    gr.FillRectangle(new SolidBrush(col), x, 0, w, panel_bandplan.Height);
                    String s = bp.be[i].text;
                    int spos = qrgToPixelpos(bp.be[i].textpos);
                    gr.DrawString(s, qrgfnt, Brushes.White, spos, -1);
                }
            }
        }

        Font bigfnt = new Font("Verdana", 24.0f);
        Font smlfnt = new Font("Verdana", 10.0f);
        int titrightpos = 10;
        int mouserightpos = 820;
        int bigy = 2;

        // insert a space betwenn each char:
        // string.Join(" ", s.ToCharArray())
        private void panel_qrg_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics gr = e.Graphics)
            {
                double val = (double)(statics.RXoffset + 10489470000) / 1e6;
                String s = String.Format("RX:" + "{0:0.000000}" + " MHz", val);
                gr.DrawString(s, bigfnt, Brushes.Green, titrightpos, bigy);

                val = (double)(statics.TXoffset + 10489470000) / 1e6 - 8089.5;
                s = String.Format("TX:" + "{0:0.000000}" + " MHz", val);
                gr.DrawString(s, bigfnt, Brushes.DarkRed, 420+ titrightpos, bigy);

                if(statics.rxmouse != -1)
                {
                    val = (double)(statics.rxmouse + 10489470000) / 1e6;
                    s = String.Format("Mouse:  {0:0.000000}" + " MHz", val);
                    gr.DrawString(s, smlfnt, Brushes.Blue, mouserightpos + titrightpos, 0);
                }
                else
                {
                    gr.FillRectangle(Brushes.Gray, mouserightpos + titrightpos, 0, panel_qrg.Width - (mouserightpos + titrightpos), panel_qrg.Height);
                }

                s = String.Format("Drift : {0:0}" + " Hz", statics.beaconoffset);
                gr.DrawString(s, smlfnt, Brushes.Blue, mouserightpos + titrightpos, 20);
            }
        }

        private void setPTT(bool onoff)
        {
            Byte[] txb = new Byte[2];
            txb[0] = 4;
            txb[1] = (Byte)(onoff ? 1 : 0);
            Udp.UdpSendData(txb);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics gr = e.Graphics)
            {
                if (statics.ptt || statics.pttkey)
                {
                    gr.FillRectangle(Brushes.Red, 0, 0, panel1.Width, panel1.Height);
                    using (Bitmap bm = new Bitmap(Properties.Resources.ptt_tx))
                        gr.DrawImage(bm, panel1.Width/2 - bm.Width/2, 0);
                }
                else
                {
                    gr.FillRectangle(Brushes.Green, 0, 0, panel1.Width, panel1.Height);
                    using (Bitmap bm = new Bitmap(Properties.Resources.ptt_rx))
                        gr.DrawImage(bm, panel1.Width / 2 - bm.Width / 2, 0);
                }
            }
            setPTT(statics.ptt | statics.pttkey);
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (statics.ptt) statics.ptt = false;
            else statics.ptt = true;
            panel1.Invalidate();
        }

        private void butto_setup_click(object sender, EventArgs e)
        {
            int oldpluto = statics.plutousb;
            String oldpladr = statics.plutoaddress;

            Form_setup setupForm = new Form_setup();

            // Show the settings form
            var res = setupForm.ShowDialog();
            if(res == DialogResult.OK)
            {
                sendAudioDevs();
                statics.sendBaseQRG();
                statics.sendReferenceOffset(statics.rfoffset);
                sendCpuSpeed();

                if (oldpluto != statics.plutousb || oldpladr != statics.plutoaddress)
                {
                    // pluto setting has been changed, restart required
                    MessageBox.Show("Pluto settings changed. Press OK to close this software, then start it again", "RESTART REQUIRED", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Close();
                }
            }
        }

        private void sendAudioDevs()
        {
            try
            {
                if (!statics.newaudiodevs) return;
                statics.newaudiodevs = false;

                //Console.WriteLine("<" + statics.AudioPBdev.Trim() + ">" + "<" + statics.AudioCAPdev + ">");
                Byte[] pb = statics.StringToByteArrayUtf8(statics.AudioPBdev.Trim());
                Byte[] cap = statics.StringToByteArrayUtf8(statics.AudioCAPdev.Trim());

                if (pb.Length > 100 || cap.Length > 100) return;

                Byte[] txb = new Byte[201];
                for (int i = 0; i < txb.Length; i++) txb[i] = 0;
                Array.Copy(pb, 0, txb, 1, pb.Length);
                Array.Copy(cap, 0, txb, 101, cap.Length);
                txb[0] = 7;
                Udp.UdpSendData(txb);
            }
            catch
            {
                // no audio devs
            }
        }
              

        private String ReadString(StreamReader sr)
        {
            try
            {
                String s = sr.ReadLine();
                if (s != null)
                {
                    return s;
                }
            }
            catch { }
            return " ";
        }

        private int ReadInt(StreamReader sr)
        {
            int v;

            try
            {
                String s = sr.ReadLine();
                if (s != null)
                {
                    v = Convert.ToInt32(s);
                    return v;
                }
            }
            catch { }
            return 0;
        }

        private UInt32 ReadUInt32(StreamReader sr)
        {
            UInt32 v;

            try
            {
                String s = sr.ReadLine();
                if (s != null)
                {
                    v = Convert.ToUInt32(s);
                    return v;
                }
            }
            catch { }
            return 0;
        }

        private double ReadMyDouble(StreamReader sr)
        {
            double v;

            try
            {
                String s = sr.ReadLine();
                if (s != null)
                {
                    v = statics.MyToDouble(s);
                    return v;
                }
            }
            catch { }
            return 0;
        }

        void load_Setup()
        {
            try
            {
                String fn = statics.getHomePath("", "trx.cfg");
                using (StreamReader sr = new StreamReader(fn))
                {
                    statics.AudioPBdev = ReadString(sr);
                    statics.AudioCAPdev = ReadString(sr);
                    String dummy2 = ReadString(sr);
                    String dummy3 = ReadString(sr);
                    statics.plutousb = ReadInt(sr);
                    statics.plutoaddress = ReadString(sr);
                    String dummy1 = ReadString(sr);
                    statics.audioagc = ReadString(sr) == "1";
                    statics.compressor = ReadInt(sr);
                    statics.rxfilter = ReadInt(sr);
                    statics.txfilter = ReadInt(sr);
                    statics.rxmute = ReadString(sr) == "1";
                    statics.rit = ReadString(sr) == "1";
                    statics.xit = ReadString(sr) == "1";
                    String dummy4 = ReadString(sr);
                    statics.beaconlock = ReadString(sr) == "1";
                    statics.rxqrg = ReadUInt32(sr);
                    statics.txqrg = ReadUInt32(sr);
                    statics.rfoffset = ReadInt(sr);
                    statics.lnboffset = ReadInt(sr);
                    statics.cpuspeed = ReadInt(sr);
                }
            }
            catch
            {
            }
        }

        void save_Setup()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(statics.getHomePath("", "trx.cfg")))
                {
                    sw.WriteLine(statics.AudioPBdev);
                    sw.WriteLine(statics.AudioCAPdev);
                    sw.WriteLine("");
                    sw.WriteLine("");
                    sw.WriteLine(statics.plutousb.ToString());
                    sw.WriteLine(statics.plutoaddress);
                    sw.WriteLine("");
                    sw.WriteLine(statics.audioagc ? "1" : "0");
                    sw.WriteLine(statics.compressor.ToString());
                    sw.WriteLine(statics.rxfilter.ToString());
                    sw.WriteLine(statics.txfilter.ToString());
                    sw.WriteLine(statics.rxmute ? "1" : "0");
                    sw.WriteLine(statics.rit ? "1" : "0");
                    sw.WriteLine(statics.xit ? "1" : "0");
                    sw.WriteLine("");
                    sw.WriteLine(statics.beaconlock ? "1" : "0");
                    sw.WriteLine(statics.rxqrg.ToString());
                    sw.WriteLine(statics.txqrg.ToString());
                    sw.WriteLine(statics.rfoffset.ToString());
                    sw.WriteLine(statics.lnboffset.ToString());
                    sw.WriteLine(statics.cpuspeed.ToString());
                }
            }
            catch { }
        }

        private void bt_info_click(object sender, EventArgs e)
        {
            Form_info fi = new Form_info();

            // Show the settings form
            var res = fi.ShowDialog();
            if (res == DialogResult.OK)
            {
            }
        }

        private void sendPlutoAddress()
        {
            //Console.WriteLine("send pluto ID <" + statics.plutoaddress.Trim() + ">");
            Byte[] iparr = statics.StringToByteArrayUtf8(statics.plutoaddress.Trim());

            Byte[] txb = new Byte[iparr.Length + 2];
            txb[0] = 10;
            txb[1] = (Byte)statics.plutousb;
            Array.Copy(iparr, 0, txb, 2, iparr.Length);
            Udp.UdpSendData(txb);
        }

        private void panel_rit_Click(object sender, EventArgs e)
        {
            statics.rit = !statics.rit;
            panel_rit.Invalidate();
        }

        private void panel_xit_Click(object sender, EventArgs e)
        {
            statics.xit = !statics.xit;
            panel_xit.Invalidate();
        }

        private void drawButtonPanel(Graphics gr, bool state, Bitmap bm, Bitmap bminact)
        {
            if (state)
                gr.DrawImage(bm, 0, 0);
            else
                gr.DrawImage(bminact, 0, 0);
        }
                
        private void panel_rit_Paint(object sender, PaintEventArgs e)
        {
            using (Bitmap bm = new Bitmap(Properties.Resources.rit_button))
                using (Bitmap bminact = new Bitmap(Properties.Resources.rit_button_inact))
                    drawButtonPanel(e.Graphics, statics.rit, bm, bminact);
        }

        private void panel_xit_Paint(object sender, PaintEventArgs e)
        {
            using (Bitmap bm = new Bitmap(Properties.Resources.xit_button))
                using (Bitmap bminact = new Bitmap(Properties.Resources.xit_button_inact))
                    drawButtonPanel(e.Graphics, statics.xit, bm, bminact);
        }

        
        private void panel_copyRtoT_Paint(object sender, PaintEventArgs e)
        {
            using (Bitmap bm = new Bitmap(Properties.Resources.rt_button))
                using (Bitmap bminact = new Bitmap(Properties.Resources.rt_button_inact))
                    drawButtonPanel(e.Graphics,true, bm, bminact);
        }

        private void panel_copyTtoR_Paint(object sender, PaintEventArgs e)
        {
            using (Bitmap bm = new Bitmap(Properties.Resources.tr_button))
                using (Bitmap bminact = new Bitmap(Properties.Resources.tr_button_inact))
                    drawButtonPanel(e.Graphics, true, bm, bminact);
        }

        private void panel_copyRtoT_Click(object sender, EventArgs e)
        {
            statics.TXoffset = statics.RXoffset;
            sendAndRefreshRXTXoffset();
            panel_copyRtoT.Invalidate();
        }

        private void panel_agc_Paint(object sender, PaintEventArgs e)
        {
            using (Bitmap bm = new Bitmap(Properties.Resources.agc_button))
                using (Bitmap bminact = new Bitmap(Properties.Resources.agc_button_inact))
                    drawButtonPanel(e.Graphics, statics.audioagc, bm, bminact);
        }

        private void panel_txmute_Paint(object sender, PaintEventArgs e)
        {
            using (Bitmap bm = new Bitmap(Properties.Resources.mute))
                using (Bitmap bminact = new Bitmap(Properties.Resources.mute_inact))
                    drawButtonPanel(e.Graphics, statics.rxmute, bm, bminact);
        }

        void sendCpuSpeed()
        {
            Byte[] txb = new Byte[2];
            txb[0] = 17;
            txb[1] = (Byte)statics.cpuspeed;
            Udp.UdpSendData(txb);
        }

        private void panel_agc_Click(object sender, EventArgs e)
        {
            if(e != null) statics.audioagc = !statics.audioagc;
            Byte[] txb = new Byte[2];
            txb[0] = 11;
            txb[1] = (Byte)(statics.audioagc ? 1 : 0);
            Udp.UdpSendData(txb);
            panel_agc.Invalidate();
        }

        private void panel_txmute_Click(object sender, EventArgs e)
        {
            if (e != null) statics.rxmute = !statics.rxmute;
            Byte[] txb = new Byte[2];
            txb[0] = 14;
            txb[1] = (Byte)(statics.rxmute ? 1 : 0);
            Udp.UdpSendData(txb);
            panel_txmute.Invalidate();
        }

        private void panel_comp_Click(object sender, EventArgs e)
        {
            if (e != null) if (++statics.compressor > 3) statics.compressor = 0;

            Byte[] txb = new Byte[2];
            txb[0] = 9;
            txb[1] = (Byte)statics.compressor; // compression factor
            Udp.UdpSendData(txb);

            panel_comp.Invalidate();
        }

        private void panel_comp_Paint(object sender, PaintEventArgs e)
        {
            switch(statics.compressor)
            {
                case 0:
                    using (Bitmap bm = new Bitmap(Properties.Resources.comp_off))
                        e.Graphics.DrawImage(bm, 0, 0);
                    break;
                case 1:
                    using (Bitmap bm = new Bitmap(Properties.Resources.comp_low))
                        e.Graphics.DrawImage(bm, 0, 0);
                    break;
                case 2:
                    using (Bitmap bm = new Bitmap(Properties.Resources.comp_mid))
                        e.Graphics.DrawImage(bm, 0, 0);
                    break;
                case 3:
                    using (Bitmap bm = new Bitmap(Properties.Resources.comp_high))
                        e.Graphics.DrawImage(bm, 0, 0);
                    break;
            }
        }

        private void panel_copyTtoR_Click(object sender, EventArgs e)
        {
            statics.RXoffset = statics.TXoffset;
            sendAndRefreshRXTXoffset();
            panel_copyTtoR.Invalidate();
        }

        private void panel_rxfilter_Paint(object sender, PaintEventArgs e)
        {
            switch (statics.rxfilter)
            {
                case 0:
                    using (Bitmap bm = new Bitmap(Properties.Resources.rx_filter_1))
                        e.Graphics.DrawImage(bm, 0, 0);
                    break;
                case 1:
                    using (Bitmap bm = new Bitmap(Properties.Resources.rx_filter_18))
                        e.Graphics.DrawImage(bm, 0, 0);
                    break;
                case 2:
                    using (Bitmap bm = new Bitmap(Properties.Resources.rx_filter_27))
                        e.Graphics.DrawImage(bm, 0, 0);
                    break;
                case 3:
                    using (Bitmap bm = new Bitmap(Properties.Resources.rx_filter_36))
                        e.Graphics.DrawImage(bm, 0, 0);
                    break;
            }
        }

        private void panel_txfilter_Paint(object sender, PaintEventArgs e)
        {
            switch (statics.txfilter)
            {
                case 0:
                    using (Bitmap bm = new Bitmap(Properties.Resources.tx_filter_1))
                        e.Graphics.DrawImage(bm, 0, 0);
                    break;
                case 1:
                    using (Bitmap bm = new Bitmap(Properties.Resources.tx_filter_18))
                        e.Graphics.DrawImage(bm, 0, 0);
                    break;
                case 2:
                    using (Bitmap bm = new Bitmap(Properties.Resources.tx_filter_22))
                        e.Graphics.DrawImage(bm, 0, 0);
                    break;
                case 3:
                case 4:
                    using (Bitmap bm = new Bitmap(Properties.Resources.tx_filter_27))
                        e.Graphics.DrawImage(bm, 0, 0);
                    break;
            }
        }

        private void panel_rxfilter_Click(object sender, EventArgs e)
        {
            if (e != null) if (++statics.rxfilter > 3) statics.rxfilter = 0;

            Byte[] txb = new Byte[2];
            txb[0] = 12;
            txb[1] = (Byte)statics.rxfilter;
            Udp.UdpSendData(txb);

            panel_rxfilter.Invalidate();
        }

        private void panel_txfilter_Click(object sender, EventArgs e)
        {
            if (e != null) if (++statics.txfilter > 3) statics.txfilter = 0;

            Byte[] txb = new Byte[2];
            txb[0] = 13;
            txb[1] = (Byte)statics.txfilter;
            Udp.UdpSendData(txb);

            panel_txfilter.Invalidate();
        }

        private void panel_audioloop_Paint(object sender, PaintEventArgs e)
        {
            using (Bitmap bm = new Bitmap(Properties.Resources.audioloop))
                using (Bitmap bminact = new Bitmap(Properties.Resources.audioloop_inact))
                    drawButtonPanel(e.Graphics, statics.audioloop, bm, bminact);
        }

        private void panel_rfloop_Paint(object sender, PaintEventArgs e)
        {
            using (Bitmap bm = new Bitmap(Properties.Resources.rfloop))
                using (Bitmap bminact = new Bitmap(Properties.Resources.rfloop_inact))
                    drawButtonPanel(e.Graphics, statics.rfloop, bm, bminact);
        }

        private void panel_audioloop_Click(object sender, EventArgs e)
        {
            statics.audioloop = !statics.audioloop;
            if (statics.audioloop && statics.rfloop)
            {
                panel_rfloop_Click(null, null);
            }

            Byte[] txb = new Byte[2];
            txb[0] = 3;
            txb[1] = (Byte)(statics.audioloop ? 1 : 0);

            Udp.UdpSendData(txb);
            panel_audioloop.Invalidate();
            panel_rfloop.Invalidate();
        }

        private void panel_rfloop_Click(object sender, EventArgs e)
        {
            statics.rfloop = !statics.rfloop;
            if (statics.audioloop && statics.rfloop)
            {
                panel_audioloop_Click(null, null);
            }

            statics.ptt = statics.rfloop;
            panel1.Invalidate();

            Byte[] txb = new Byte[2];
            txb[0] = 5;
            txb[1] = (Byte)(statics.rfloop ? 1 : 0);

            Udp.UdpSendData(txb);
            panel_rfloop.Invalidate();
            panel_audioloop.Invalidate();
        }

        private void panel_pavucontrol_Click(object sender, EventArgs e)
        {
            statics.StartMixer(true);
        }

        static Pen cline = new Pen(Brushes.Yellow, 2);
        private void panel_rxline_Paint(object sender, PaintEventArgs e)
        {
            int x = statics.RXoffset / 500;
            //Console.WriteLine("pp " + x);
            using (Graphics gr = e.Graphics)
            {
                gr.DrawLine(cline, panel_rxline.Width / 2, panel_rxline.Height, x, 0);
            }
        }

        private void panel_sync_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics gr = e.Graphics)
            {
                Font syfont = new Font("Verdana", 7.0f);
                gr.DrawString("CAL", syfont, Brushes.Black, -1, 2);
            }
        }

        

        private void panel_beaconlock_Paint(object sender, PaintEventArgs e)
        {
            String s = "FRE";
            if (statics.beaconlock) s = "LCK";

            using (Graphics gr = e.Graphics)
            {
                Font syfont = new Font("Verdana", 7.0f);
                gr.DrawString(s, syfont, Brushes.Black, -1, 2);
            }

            Byte[] txb = new Byte[2];
            txb[0] = 16;
            txb[1] = (Byte)(statics.beaconlock ? 1 : 0);
            Udp.UdpSendData(txb);
        }

        private void panel_beaconlock_Click(object sender, EventArgs e)
        {
            statics.beaconlock = !statics.beaconlock;
            panel_beaconlock.Invalidate();
        }

        private void panel_sync_Click(object sender, EventArgs e)
        {
            Form_reference fr = new Form_reference();
            fr.Show();
        }

        private void panel_txhighpass_Click(object sender, EventArgs e)
        {
            statics.audioHighpass = !statics.audioHighpass;

            Byte[] txb = new Byte[2];
            txb[0] = 18;
            txb[1] = (Byte)(statics.audioHighpass ? 1 : 0);
            Udp.UdpSendData(txb);
            panel_txhighpass.Invalidate();
        }

        private void panel_txhighpass_Paint(object sender, PaintEventArgs e)
        {
            using (Bitmap bm = new Bitmap(Properties.Resources.tx_bass_inact))
                using (Bitmap bminact = new Bitmap(Properties.Resources.tx_bass))
                    drawButtonPanel(e.Graphics, statics.audioHighpass, bm, bminact);
        }
    }

    class DoubleBufferedPanel : Panel 
    { 
        public DoubleBufferedPanel() : base() 
        {
            // double buffering crashes under windows
            OperatingSystem osversion = System.Environment.OSVersion;
            if (osversion.VersionString.Contains("indow"))
                statics.ostype = 0; // Win$
            else
            {
                statics.ostype = 1; // Linux
                DoubleBuffered = true;
            }
        }
    }
}
