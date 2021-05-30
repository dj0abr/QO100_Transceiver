﻿using System;
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
            this.Height = 780;

            panel_qrg.Width = 1120;
            panel_qrg.Height = 40;

            panel_bigspec.Width = 1120;
            panel_bigspec.Height = 150;

            panel_bandplan.Width = 1120;
            panel_bandplan.Height = 16;
            
            panel_bigwf.Width = 1120;
            panel_bigwf.Height = 150;

            panel_smallspec.Width = 1120;
            panel_smallspec.Height = 150;

            panel_smallqrg.Width = 1120;
            panel_smallqrg.Height = 16;

            panel_smallwf.Width = 1120;
            panel_smallwf.Height = 150;

            panel_qrg.Location = new Point(13, 4);
            panel_bigspec.Location = new Point(13, panel_qrg.Location.Y + panel_qrg.Height+5);
            panel_bandplan.Location = new Point(13, panel_bigspec.Location.Y + panel_bigspec.Height);
            panel_bigwf.Location = new Point(13, panel_bandplan.Location.Y + panel_bandplan.Height);

            panel_smallspec.Location = new Point(13, panel_bigwf.Location.Y + panel_bigwf.Height+15);
            panel_smallqrg.Location = new Point(13, panel_smallspec.Location.Y + panel_smallspec.Height);
            panel_smallwf.Location = new Point(13, panel_smallqrg.Location.Y + panel_smallqrg.Height);

            int yspace = 12;

            bt_info.Location = new Point(panel_bigspec.Location.X + panel_bigspec.Width - bt_info.Width, panel_smallwf.Location.Y + panel_smallwf.Height + yspace);
            button_setup.Location = new Point(bt_info.Location.X - button_setup.Width - 5, bt_info.Location.Y);
            gp_testmodes.Location = new Point(button_setup.Location.X - gp_testmodes.Width - 5, panel_smallwf.Location.Y + panel_smallwf.Height + 1);
            gp_qrg.Location = new Point(13, panel_smallwf.Location.Y + panel_smallwf.Height + 1);
            gp_copyqrg.Location = new Point(gp_qrg.Location.X + gp_qrg.Width + 5, panel_smallwf.Location.Y + panel_smallwf.Height + 1);
            gp_audio.Location = new Point(gp_copyqrg.Location.X + gp_copyqrg.Width + 5, gp_copyqrg.Location.Y);
            panel1.Location = new Point(gp_audio.Location.X + gp_audio.Width + 5, gp_audio.Location.Y + 7);
            panel1.Width = gp_testmodes.Location.X - panel1.Location.X - 5;

            cb_rxtotx.Location = new Point(cb_rxtotx.Location.X, 19);
            cb_txtorx.Location = new Point(cb_txtorx.Location.X, 19);

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

            sendRXTXoffset();
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
            if(Udp.getBigSpecBitmap_avail())
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

            // wait for audio devices
            if (statics.GotAudioDevices == 0)
            {
                // request device list
                Byte[] txb = new Byte[1];
                txb[0] = 6;
                Udp.UdpSendData(txb);

                statics.newaudiodevs = true;
                sendAudioDevs();
                sendBaseQRG();
            }
        }

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

        private void sendRXTXoffset()
        {
            if (statics.RXoffset < 0) statics.RXoffset = 0;
            if (statics.RXoffset > 560000) statics.RXoffset = 560000;
            if (statics.TXoffset < 0) statics.RXoffset = 0;
            if (statics.TXoffset > 560000) statics.TXoffset = 560000;

            Byte[] txb = new Byte[9];
            txb[0] = 0;
            txb[1] = (Byte)(statics.RXoffset >> 24);
            txb[2] = (Byte)(statics.RXoffset >> 16);
            txb[3] = (Byte)(statics.RXoffset >> 8);
            txb[4] = (Byte)(statics.RXoffset & 0xff);
            txb[5] = (Byte)(statics.TXoffset >> 24);
            txb[6] = (Byte)(statics.TXoffset >> 16);
            txb[7] = (Byte)(statics.TXoffset >> 8);
            txb[8] = (Byte)(statics.TXoffset & 0xff);

            Udp.UdpSendData(txb);

            panel_qrg.Invalidate(); // show qrg
        }

        private void tuneBig(MouseEventArgs e)
        {
            

            if (e.Button == MouseButtons.Left)
            {
                statics.RXoffset = e.X * 500;
                sendRXTXoffset();
            }

            if (e.Button == MouseButtons.Right)
            {
                statics.RXoffset = e.X * 500;
                statics.TXoffset = e.X * 500;
                sendRXTXoffset();
            }
        }

        private void tuneSmall(MouseEventArgs e)
        {
            // small WF: 25Hz per pixel, with the tune qrg in the middle
            int hz = (e.X - 560) * 25;
            

            if (e.Button == MouseButtons.Left)
            {
                statics.RXoffset += hz;
                sendRXTXoffset();
            }

            if (e.Button == MouseButtons.Right)
            {
                statics.RXoffset += hz;
                statics.TXoffset += hz;
                sendRXTXoffset();
            }
        }

        private void panel_bigwf_MouseWheel(object sender, MouseEventArgs e)
        {
            int factor = 100;
            if (Control.ModifierKeys == Keys.Alt) factor = 1000;
            if (Control.ModifierKeys == Keys.Shift) factor = 10000;

            if (rb_rit.Checked)
            {
                if (e.Delta > 0) //wheel direction
                    statics.RXoffset += factor;
                else
                    statics.RXoffset -= factor;

                sendRXTXoffset();
            }
            else
            {
                if (e.Delta > 0) //wheel direction
                    statics.TXoffset += factor;
                else
                    statics.TXoffset -= factor;

                sendRXTXoffset();
            }
        }

        private void panel_smallwf_MouseWheel(object sender, MouseEventArgs e)
        {
            if (rb_rit.Checked)
            {
                if (e.Delta > 0) //wheel direction
                    statics.RXoffset += 10;
                else
                    statics.RXoffset -= 10;

                sendRXTXoffset();
            }
            else
            {
                if (e.Delta > 0) //wheel direction
                    statics.TXoffset += 10;
                else
                    statics.TXoffset -= 10;

                sendRXTXoffset();
            }
        }

        private void panel_bigwf_MouseHover(object sender, EventArgs e)
        {
            panel_bigwf.Focus();
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
        int titrightpos = 90;
        private void panel_qrg_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics gr = e.Graphics)
            {
                double val = (double)(statics.RXoffset + 10489470000) / 1e6;
                String s = String.Format(" RX:" + "{0:0.000000}" + " MHz", val);
                gr.DrawString(s, bigfnt, Brushes.Green, titrightpos, 0);

                val = (double)(statics.TXoffset + 10489470000) / 1e6 - 8089.5;
                s = String.Format(" TX:" + "{0:0.000000}" + " MHz", val);
                gr.DrawString(s, bigfnt, Brushes.DarkRed, 560+ titrightpos, 0);
            }
        }

        private void cb_audioloop_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_audioloop.Checked)
                cb_rfloop.Checked = false;

            Byte[] txb = new Byte[2];
            txb[0] = 3;
            txb[1] = (Byte)(cb_audioloop.Checked ? 1 : 0);

            Udp.UdpSendData(txb);
        }

        private void cb_rfloop_CheckedChanged(object sender, EventArgs e)
        {
            if(cb_rfloop.Checked)
                cb_audioloop.Checked = false;

            

            Byte[] txb = new Byte[2];
            txb[0] = 5;
            txb[1] = (Byte)(cb_rfloop.Checked ? 1 : 0);

            Console.WriteLine("cw " + cb_rfloop.Checked + " " + txb[1]);

            Udp.UdpSendData(txb);
        }

        private void cb_rxtotx_CheckedChanged(object sender, EventArgs e)
        {
            statics.TXoffset = statics.RXoffset;
            sendRXTXoffset();
            cb_rxtotx.Checked = false;
        }

        private void cb_txtorx_CheckedChanged(object sender, EventArgs e)
        {
            statics.RXoffset = statics.TXoffset;
            sendRXTXoffset();
            cb_txtorx.Checked = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics gr = e.Graphics)
            {
                if (statics.ptt || statics.pttkey)
                    gr.FillRectangle(Brushes.Red, 0, 0, panel1.Width, panel1.Height);
                else
                    gr.FillRectangle(Brushes.Green, 0, 0, panel1.Width, panel1.Height);

                Font fnt = new Font("Verdana", 20.0f);
                gr.DrawString("PTT", fnt, Brushes.Black, panel1.Width/2-10, 0);
            }

            Byte[] txb = new Byte[2];
            txb[0] = 4;
            txb[1] = (Byte)((statics.ptt | statics.pttkey) ? 1 : 0);
            Udp.UdpSendData(txb);
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (statics.ptt) statics.ptt = false;
            else statics.ptt = true;
            panel1.Invalidate();
        }

        private void button_setup_Click(object sender, EventArgs e)
        {
            Form_setup setupForm = new Form_setup();

            // Show the settings form
            var res = setupForm.ShowDialog();
            if(res == DialogResult.OK)
            {
                sendAudioDevs();
                sendBaseQRG();
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

        private void sendBaseQRG()
        {
            Byte[] txb = new Byte[9];
            txb[0] = 8;
            //Console.WriteLine("*********************************** " + statics.rxqrg + " * " + statics.txqrg);
            txb[1] = (Byte)(statics.rxqrg >> 24);
            txb[2] = (Byte)(statics.rxqrg >> 16);
            txb[3] = (Byte)(statics.rxqrg >> 8);
            txb[4] = (Byte)(statics.rxqrg & 0xff);
            txb[5] = (Byte)(statics.txqrg >> 24);
            txb[6] = (Byte)(statics.txqrg >> 16);
            txb[7] = (Byte)(statics.txqrg >> 8);
            txb[8] = (Byte)(statics.txqrg & 0xff);
            Udp.UdpSendData(txb);
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

        void load_Setup()
        {
            try
            {
                String fn = statics.getHomePath("", "trx.cfg");
                using (StreamReader sr = new StreamReader(fn))
                {
                    statics.AudioPBdev = ReadString(sr);
                    statics.AudioCAPdev = ReadString(sr);
                    statics.rxqrg = ReadInt(sr);
                    statics.txqrg = ReadInt(sr);
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
                    sw.WriteLine(statics.rxqrg.ToString());
                    sw.WriteLine(statics.txqrg.ToString());
                }
            }
            catch { }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Byte[] txb = new Byte[2];
            txb[0] = 9;
            txb[1] = (Byte)((checkBox2.Checked) ? 1 : 0);
            Udp.UdpSendData(txb);
        }

        private void bt_info_Click(object sender, EventArgs e)
        {
            Form_info fi = new Form_info();

            // Show the settings form
            var res = fi.ShowDialog();
            if (res == DialogResult.OK)
            {
            }
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
