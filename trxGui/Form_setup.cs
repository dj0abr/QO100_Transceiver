using System;
using System.Drawing;
using System.Windows.Forms;

namespace trxGui
{
    public partial class Form_setup : Form
    {
        public Form_setup()
        {
            InitializeComponent();

            Width = 800;
            Height = 416;
            int yb = Height - 70;
            button1.Location = new Point(Width - button1.Width - 20, yb);
            button2.Location = new Point(button1.Location.X - button2.Width - 10, yb);
            button3.Location = new Point(button2.Location.X - button3.Width - 10, yb);

            button_shutdown.Location = new Point(Width - button_shutdown.Width - 20, yb - button1.Height - 10);

            comboBox1.SelectedIndex = statics.language;

            foreach (Control c in Controls)
                c.Text = language.GetText(c.Text);

            tb_rxqrg.Text = ((double)statics.rxqrg / 1000000.0).ToString();
            tb_txqrg.Text = ((double)statics.txqrg / 1000000.0).ToString();
            tb_plutooffset.Text = statics.rfoffset.ToString();
            tb_lnboffset.Text = statics.lnboffset.ToString();

            cb_audioPB.Text = statics.AudioPBdev;
            cb_audioCAP.Text = statics.AudioCAPdev;

            rb_plutousb.Checked = statics.plutousb == 1 ? true : false;
            rb_plutoeth.Checked = statics.plutousb == 1 ? false : true;
            tb_plutoip.Text = statics.plutoaddress;
            textBox_txpower.Text = statics.txpower.ToString();

            comboBox_cpuspeed.SelectedIndex = statics.cpuspeed;
            comboBox_color.SelectedIndex = statics.palette;

            cb_autosync.Checked = statics.autosync;
            cb_pttmode.SelectedIndex = statics.pttmode;

            // populate combo boxes
            if (statics.AudioPBdevs != null)
            {
                cb_audioPB.BeginUpdate();
                cb_audioPB.Items.Clear();
                foreach (String s in statics.AudioPBdevs)
                {
                    if (s.Length > 1)
                    {
                        cb_audioPB.Items.Add(s);
                    }
                }
                cb_audioPB.EndUpdate();
                // check if displayed text is available in the item list
                findDevice(cb_audioPB);
            }

            if (statics.AudioCAPdevs != null)
            {
                cb_audioCAP.BeginUpdate();
                cb_audioCAP.Items.Clear();
                foreach (String s in statics.AudioCAPdevs)
                {
                    if (s.Length > 1)
                    {
                        cb_audioCAP.Items.Add(s);
                    }
                }
                cb_audioCAP.EndUpdate();
                findDevice(cb_audioCAP);
            }
        }

        void findDevice(ComboBox cb)
        {
            int pos = -1;

            if (cb.Text.Length >= 4)
            {
                int anz = cb.Items.Count;
                for (int i = 0; i < anz; i++)
                {
                    if (cb.Text == cb.Items[i].ToString())
                    {
                        pos = i;
                        break;
                    }
                }
            }

            if (pos == -1)
            {
                // not available, reset to first item which usually is Default
                if (cb.Items.Count == 0)
                    cb.Text = "no sound devices available";
                else
                    cb.Text = cb.Items[0].ToString();
            }
            else
                cb.Text = cb.Items[pos].ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (statics.AudioPBdev != cb_audioPB.Text || statics.AudioCAPdev != cb_audioCAP.Text)
            {
                statics.AudioPBdev = cb_audioPB.Text;
                statics.AudioCAPdev = cb_audioCAP.Text;
                statics.newaudiodevs = true;
            }

            statics.rxqrg = (UInt32)(statics.MyToDouble(tb_rxqrg.Text) * 1000000.0);
            statics.txqrg = (UInt32)(statics.MyToDouble(tb_txqrg.Text) * 1000000.0);
            double dv = statics.MyToDouble(tb_plutooffset.Text);
            statics.rfoffset = (int)dv;
            dv = statics.MyToDouble(tb_lnboffset.Text);
            statics.lnboffset = (int)dv;
            dv = statics.MyToDouble(textBox_txpower.Text);
            statics.txpower = (int)dv;
            if (statics.txpower > 0) statics.txpower = 0;
            if (statics.txpower < -60) statics.txpower = -60;

            statics.plutousb = rb_plutousb.Checked?1:0;
            statics.plutoaddress = tb_plutoip.Text;
            statics.cpuspeed = comboBox_cpuspeed.SelectedIndex;
            statics.palette = comboBox_color.SelectedIndex;
            statics.autosync = cb_autosync.Checked;
            statics.pttmode = cb_pttmode.SelectedIndex;
        }

        private void rb_plutousb_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_plutousb.Checked)
            {
                tb_plutoip.Enabled = false;
            }
        }

        private void rb_plutoeth_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_plutoeth.Checked)
            {
                tb_plutoip.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            statics.OpenUrl("https://wiki.amsat-dl.org/doku.php?id=en:plutotrx:config");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            statics.language = comboBox1.SelectedIndex;

            foreach (Control c in Controls)
                c.Text = language.GetText(c.Text);
        }

        private void button_shutdown_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(language.GetText("Do you want to shut down the computer?"), "Shut Down", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                Byte[] txb = new Byte[1];
                txb[0] = 21;
                Udp.UdpSendData(txb);
            }
        }
    }
}
