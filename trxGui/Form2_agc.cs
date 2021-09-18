using System;
using System.Drawing;
using System.Windows.Forms;

namespace trxGui
{
    public partial class Form2_agc : Form
    {
        UdpQueue valq;

        public Form2_agc(Point parentpos, UdpQueue vq)
        {
            valq = vq;

            InitializeComponent();

            Width = 460;
            
            StartPosition = FormStartPosition.Manual;
            Location = new Point(parentpos.X, parentpos.Y);

            tb_micboostcol.Width = Width - tb_micboostcol.Location.X - 15;
            button1.Location = new Point(tb_micboostcol.Location.X + tb_micboostcol.Width - button1.Width, button1.Location.Y);

            try
            {
                tb_micboostcol.Value = (decimal)statics.micboost;
            }
            catch
            {
                tb_micboostcol.Value = 1;
            }
            cb_bass.Checked = !statics.audioHighpass;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        void sendVals()
        {
            Byte[] txb = new Byte[5];
            txb[0] = 11;
            txb[1] = (Byte)(statics.audioHighpass ? 1 : 0);
            txb[2] = (Byte)statics.micboost;
            txb[3] = (Byte)(statics.agcvalue >> 8);
            txb[4] = (Byte)(statics.agcvalue & 0xff);
            valq.Add(txb);
        }

        private void cb_bass_CheckedChanged(object sender, EventArgs e)
        {
            statics.audioHighpass = !cb_bass.Checked;
            sendVals();
        }

        private void tb_micboostcol_Scroll(object sender, ScrollEventArgs e)
        {
            statics.micboost = (int)tb_micboostcol.Value;
            sendVals();
        }
    }
}
