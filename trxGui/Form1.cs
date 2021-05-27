using System;
using System.Drawing;
using System.Windows.Forms;

namespace trxGui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            panel_bigwf.MouseWheel += panel_bigwf_MouseWheel;
            panel_bigspec.MouseWheel += panel_bigwf_MouseWheel;

            panel_smallwf.MouseWheel += panel_smallwf_MouseWheel;
            panel_smallspec.MouseWheel += panel_smallwf_MouseWheel;

            this.Width = 1155;
            this.Height = 724;

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

            panel_bigspec.Location = new Point(13, 13);
            panel_bandplan.Location = new Point(13, panel_bigspec.Location.Y + panel_bigspec.Height);
            panel_bigwf.Location = new Point(13, panel_bandplan.Location.Y + panel_bandplan.Height);

            panel_smallspec.Location = new Point(13, panel_bigwf.Location.Y + panel_bigwf.Height+15);
            panel_smallqrg.Location = new Point(13, panel_smallspec.Location.Y + panel_smallspec.Height);
            panel_smallwf.Location = new Point(13, panel_smallqrg.Location.Y + panel_smallqrg.Height);

            rb_rit.Location = new Point(13, panel_smallwf.Location.Y + panel_smallwf.Height + 10);
            rb_xit.Location = new Point(rb_rit.Location.X + rb_rit.Width + 5, panel_smallwf.Location.Y + panel_smallwf.Height + 10);

            bt_info.Location = new Point(panel_bigspec.Location.X + panel_bigspec.Width - bt_info.Width, panel_smallwf.Location.Y + panel_smallwf.Height + 10);

            Udp.InitUdp(panel_bigspec.Width, panel_bigspec.Height);

            timer_draw.Start();
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
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
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

        private void sendRXoffset(Byte mode)
        {
            //if (statics.RXoffset < 0) statics.RXoffset = 0;
            //if (statics.RXoffset > 1030) statics.RXoffset = 1030;

            Byte[] txb = new Byte[5];
            txb[0] = mode;
            txb[1] = (Byte)(statics.RXoffset >> 24);
            txb[2] = (Byte)(statics.RXoffset >> 16);
            txb[3] = (Byte)(statics.RXoffset >> 8);
            txb[4] = (Byte)(statics.RXoffset & 0xff);

            Udp.UdpSendData(txb);
        }

        private void tuneBig(MouseEventArgs e)
        {
            statics.RXoffset = e.X * 500;

            if (e.Button == MouseButtons.Left)
                sendRXoffset(0);

            if (e.Button == MouseButtons.Right)
                sendRXoffset(1);
        }

        private void tuneSmall(MouseEventArgs e)
        {
            // small WF: 25Hz per pixel, with the tune qrg in the middle
            int hz = (e.X - 560) * 25;
            statics.RXoffset += hz;

            if (e.Button == MouseButtons.Left)
                sendRXoffset(0);

            if (e.Button == MouseButtons.Right)
                sendRXoffset(1);
        }

        private void panel_bigwf_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0) //wheel direction
                statics.RXoffset += 100;
            else
                statics.RXoffset -= 100;

            if(rb_rit.Checked) sendRXoffset(0);
            else sendRXoffset(2);
        }

        private void panel_smallwf_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0) //wheel direction
                statics.RXoffset += 10;
            else
                statics.RXoffset -= 10;

            if (rb_rit.Checked) sendRXoffset(0);
            else sendRXoffset(2);
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
    }

    class DoubleBufferedPanel : Panel { public DoubleBufferedPanel() : base() { DoubleBuffered = true; } }
}
