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
    public partial class Form_reference : Form
    {
        public Form_reference()
        {
            InitializeComponent();

            foreach (Control c in Controls)
                c.Text = language.GetText(c.Text);
        }

        private void button_cal439_Click(object sender, EventArgs e)
        {
            // set receiver to standard values, clear any offset correction
            statics.calfreq = statics.calbasefreq; // shift the carrier into the center
            statics.sendReferenceOffset(0);
            statics.sendBaseQRG(statics.calfreq);
            statics.calmode = 1;
            label_cal439.Text = " CALIBRATING ... click 439 MHz peak";
        }

        private void button_fin_Click(object sender, EventArgs e)
        {
            statics.calmode = 0;
            label_cal439.Text = "";

            Int64 calibrFr = statics.calfreq;// + 250000;
            Int64 shift439 = calibrFr - statics.calbasefreq;
            Console.WriteLine("calbasefreq: " + statics.calbasefreq + " calibrFr: " + calibrFr);
            // shift439 ... this is the pluto-TCXO error related to 439 MHz
            //Int64 shift40 = shift439 * 40000000 / statics.calbasefreq;
            int shiftRXqrg = (int)(shift439 * statics.rxqrg / statics.calbasefreq);

            Console.WriteLine("error 439 MHz: " + shift439 + "error " + statics.rxqrg / 1e6 + " MHz: " + shiftRXqrg);
            statics.sendReferenceOffset(shiftRXqrg);

            // set back to QO100 frequencies
            statics.sendBaseQRG();
        }

        private void button_caliblnb_Click(object sender, EventArgs e)
        {
            statics.RXoffset = 280000;
            statics.TXoffset = 280000;
            statics.lnboffset = 0;
            statics.sendRXTXoffset();
            statics.sendBaseQRG();
            statics.calmode = 2;
            label_caliblnb.Text = " CALIBRATING ... click center of BPSK beacon";
        }

        private void button_lnbfinished_Click(object sender, EventArgs e)
        {
            statics.calmode = 0;
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            if (statics.calmode != 0)
                MessageBox.Show("FINISH calibration before closing this window","CALIBRATION in progress",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            else
                Close();
        }
    }
}
