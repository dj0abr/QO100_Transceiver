using System;
using System.Windows.Forms;

namespace trxGui
{
    public partial class Form_screen : Form
    {
        public Form_screen()
        {
            InitializeComponent();
            try
            {
                comboBox1.SelectedIndex = statics.windowsize;
            }
            catch 
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            statics.windowsize = comboBox1.SelectedIndex;
            Close();
        }
    }
}
