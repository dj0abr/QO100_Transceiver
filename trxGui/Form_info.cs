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
    public partial class Form_info : Form
    {
        public Form_info()
        {
            InitializeComponent();

            this.Width = 800;
            button_ok.Location = new Point(Width - button_ok.Width - 30, Height - button_ok.Height - 30);

            foreach (Control c in Controls)
                c.Text = language.GetText(c.Text);

            textBox1.Select(0, 0);
        }

        private void button_manual_Click(object sender, EventArgs e)
        {
            if(statics.language == 0)
                statics.OpenUrl("https://wiki.amsat-dl.org/doku.php?id=en:plutotrx:overview");
            if (statics.language == 1)
                statics.OpenUrl("https://wiki.amsat-dl.org/doku.php?id=de:plutotrx:overview");
        }
    }
}
