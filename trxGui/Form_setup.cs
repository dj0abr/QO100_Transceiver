using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace trxGui
{
    public partial class Form_setup : Form
    {
        public Form_setup()
        {
            InitializeComponent();

            tb_rxqrg.Text = ((double)statics.rxqrg / 1000).ToString();
            tb_txqrg.Text = ((double)statics.txqrg / 1000).ToString();

            cb_audioPB.Text = statics.AudioPBdev;
            cb_audioCAP.Text = statics.AudioCAPdev;

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

            statics.rxqrg = (int)(statics.MyToDouble(tb_rxqrg.Text) * 1000);
            statics.txqrg = (int)(statics.MyToDouble(tb_txqrg.Text) * 1000);
        }

    }
}
