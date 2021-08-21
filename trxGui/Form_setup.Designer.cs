
namespace trxGui
{
    partial class Form_setup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.cb_audioPB = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cb_audioCAP = new System.Windows.Forms.ComboBox();
            this.tb_txqrg = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_rxqrg = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.rb_plutousb = new System.Windows.Forms.RadioButton();
            this.rb_plutoeth = new System.Windows.Forms.RadioButton();
            this.tb_plutoip = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_plutooffset = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tb_lnboffset = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBox_cpuspeed = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.comboBox_color = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBox_txpower = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.button_shutdown = new System.Windows.Forms.Button();
            this.cb_autosync = new System.Windows.Forms.CheckBox();
            this.label19 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(588, 301);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Location = new System.Drawing.Point(507, 301);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cb_audioPB
            // 
            this.cb_audioPB.FormattingEnabled = true;
            this.cb_audioPB.Location = new System.Drawing.Point(154, 47);
            this.cb_audioPB.Name = "cb_audioPB";
            this.cb_audioPB.Size = new System.Drawing.Size(509, 21);
            this.cb_audioPB.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 50);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(133, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "Loadspeaker/Headphone:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 76);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(66, 13);
            this.label11.TabIndex = 12;
            this.label11.Text = "Microphone:";
            // 
            // cb_audioCAP
            // 
            this.cb_audioCAP.FormattingEnabled = true;
            this.cb_audioCAP.Location = new System.Drawing.Point(154, 74);
            this.cb_audioCAP.Name = "cb_audioCAP";
            this.cb_audioCAP.Size = new System.Drawing.Size(509, 21);
            this.cb_audioCAP.TabIndex = 13;
            // 
            // tb_txqrg
            // 
            this.tb_txqrg.Location = new System.Drawing.Point(154, 127);
            this.tb_txqrg.Name = "tb_txqrg";
            this.tb_txqrg.Size = new System.Drawing.Size(137, 20);
            this.tb_txqrg.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Transmitter Frequency:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(295, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(307, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "MHz ( enter center frequency corresponding to 10489.750 MHz )";
            // 
            // tb_rxqrg
            // 
            this.tb_rxqrg.Location = new System.Drawing.Point(154, 101);
            this.tb_rxqrg.Name = "tb_rxqrg";
            this.tb_rxqrg.Size = new System.Drawing.Size(137, 20);
            this.tb_rxqrg.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Receiver Frequency:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(295, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(301, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "MHz ( enter center frequency corresponding to 2400.250 MHz )";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 31;
            this.label4.Text = "Pluto Address:";
            // 
            // rb_plutousb
            // 
            this.rb_plutousb.AutoSize = true;
            this.rb_plutousb.Checked = true;
            this.rb_plutousb.Location = new System.Drawing.Point(154, 15);
            this.rb_plutousb.Name = "rb_plutousb";
            this.rb_plutousb.Size = new System.Drawing.Size(72, 17);
            this.rb_plutousb.TabIndex = 32;
            this.rb_plutousb.TabStop = true;
            this.rb_plutousb.Text = "local USB";
            this.rb_plutousb.UseVisualStyleBackColor = true;
            this.rb_plutousb.CheckedChanged += new System.EventHandler(this.rb_plutousb_CheckedChanged);
            // 
            // rb_plutoeth
            // 
            this.rb_plutoeth.AutoSize = true;
            this.rb_plutoeth.Location = new System.Drawing.Point(259, 15);
            this.rb_plutoeth.Name = "rb_plutoeth";
            this.rb_plutoeth.Size = new System.Drawing.Size(65, 17);
            this.rb_plutoeth.TabIndex = 33;
            this.rb_plutoeth.Text = "Ethernet";
            this.rb_plutoeth.UseVisualStyleBackColor = true;
            this.rb_plutoeth.CheckedChanged += new System.EventHandler(this.rb_plutoeth_CheckedChanged);
            // 
            // tb_plutoip
            // 
            this.tb_plutoip.Enabled = false;
            this.tb_plutoip.Location = new System.Drawing.Point(330, 14);
            this.tb_plutoip.Name = "tb_plutoip";
            this.tb_plutoip.Size = new System.Drawing.Size(142, 20);
            this.tb_plutoip.TabIndex = 34;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(480, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(141, 13);
            this.label5.TabIndex = 35;
            this.label5.Text = "enter IP address of the Pluto";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(295, 157);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(340, 13);
            this.label7.TabIndex = 38;
            this.label7.Text = "Hz (correction value for the Pluto TCXO, or use CAL calibration function)";
            // 
            // tb_plutooffset
            // 
            this.tb_plutooffset.Location = new System.Drawing.Point(154, 153);
            this.tb_plutooffset.Name = "tb_plutooffset";
            this.tb_plutooffset.Size = new System.Drawing.Size(137, 20);
            this.tb_plutooffset.TabIndex = 37;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 154);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "Pluto Offset:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(295, 183);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(306, 13);
            this.label9.TabIndex = 41;
            this.label9.Text = "Hz (correction value for the LNB, or use CAL calibration function)";
            // 
            // tb_lnboffset
            // 
            this.tb_lnboffset.Location = new System.Drawing.Point(154, 179);
            this.tb_lnboffset.Name = "tb_lnboffset";
            this.tb_lnboffset.Size = new System.Drawing.Size(137, 20);
            this.tb_lnboffset.TabIndex = 40;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 180);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 13);
            this.label12.TabIndex = 39;
            this.label12.Text = "LNB Offset:";
            // 
            // comboBox_cpuspeed
            // 
            this.comboBox_cpuspeed.FormattingEnabled = true;
            this.comboBox_cpuspeed.Items.AddRange(new object[] {
            "fast CPU (RPI4...)",
            "normal CPU",
            "very slow CPU (RPI3...)"});
            this.comboBox_cpuspeed.Location = new System.Drawing.Point(154, 231);
            this.comboBox_cpuspeed.Name = "comboBox_cpuspeed";
            this.comboBox_cpuspeed.Size = new System.Drawing.Size(137, 21);
            this.comboBox_cpuspeed.TabIndex = 42;
            this.comboBox_cpuspeed.Text = "normal CPU";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(11, 232);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(136, 13);
            this.label13.TabIndex = 43;
            this.label13.Text = "Spectrum/Waterfall Speed:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(296, 233);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(197, 13);
            this.label14.TabIndex = 44;
            this.label14.Text = "to reduce CPU load on slower computers";
            // 
            // button3
            // 
            this.button3.ForeColor = System.Drawing.Color.Red;
            this.button3.Location = new System.Drawing.Point(426, 302);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 45;
            this.button3.Text = "Help";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(11, 284);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(58, 13);
            this.label15.TabIndex = 47;
            this.label15.Text = "Language:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "English",
            "German/Deutsch",
            "French/Français",
            "Spanish/Español",
            "Portuguese/Portugués",
            "Italian/Italiano"});
            this.comboBox1.Location = new System.Drawing.Point(154, 285);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(137, 21);
            this.comboBox1.TabIndex = 46;
            this.comboBox1.Text = "English";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(11, 258);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(70, 13);
            this.label16.TabIndex = 49;
            this.label16.Text = "Color Palette:";
            // 
            // comboBox_color
            // 
            this.comboBox_color.FormattingEnabled = true;
            this.comboBox_color.Items.AddRange(new object[] {
            "blue / blau",
            "red / rot",
            "green / grün",
            "white / weiß"});
            this.comboBox_color.Location = new System.Drawing.Point(154, 258);
            this.comboBox_color.Name = "comboBox_color";
            this.comboBox_color.Size = new System.Drawing.Size(137, 21);
            this.comboBox_color.TabIndex = 48;
            this.comboBox_color.Text = "blue / blau";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(295, 208);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(226, 13);
            this.label17.TabIndex = 52;
            this.label17.Text = "Pluto TX power [dBm], allowed values: -40 to 0";
            // 
            // textBox_txpower
            // 
            this.textBox_txpower.Location = new System.Drawing.Point(154, 205);
            this.textBox_txpower.Name = "textBox_txpower";
            this.textBox_txpower.Size = new System.Drawing.Size(137, 20);
            this.textBox_txpower.TabIndex = 51;
            this.textBox_txpower.Text = "0";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(12, 206);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(56, 13);
            this.label18.TabIndex = 50;
            this.label18.Text = "TX power:";
            // 
            // button_shutdown
            // 
            this.button_shutdown.ForeColor = System.Drawing.Color.Red;
            this.button_shutdown.Location = new System.Drawing.Point(588, 272);
            this.button_shutdown.Name = "button_shutdown";
            this.button_shutdown.Size = new System.Drawing.Size(75, 23);
            this.button_shutdown.TabIndex = 53;
            this.button_shutdown.Text = "ShutDown";
            this.button_shutdown.UseVisualStyleBackColor = true;
            this.button_shutdown.Click += new System.EventHandler(this.button_shutdown_Click);
            // 
            // cb_autosync
            // 
            this.cb_autosync.AutoSize = true;
            this.cb_autosync.Location = new System.Drawing.Point(156, 313);
            this.cb_autosync.Name = "cb_autosync";
            this.cb_autosync.Size = new System.Drawing.Size(65, 17);
            this.cb_autosync.TabIndex = 54;
            this.cb_autosync.Text = "ON / off";
            this.cb_autosync.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(12, 312);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(82, 13);
            this.label19.TabIndex = 55;
            this.label19.Text = "QSO Auto Sync";
            // 
            // Form_setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 343);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.cb_autosync);
            this.Controls.Add(this.button_shutdown);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.textBox_txpower);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.comboBox_color);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.comboBox_cpuspeed);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tb_lnboffset);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tb_plutooffset);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb_plutoip);
            this.Controls.Add(this.rb_plutoeth);
            this.Controls.Add(this.rb_plutousb);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_rxqrg);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_txqrg);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cb_audioCAP);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cb_audioPB);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form_setup";
            this.Text = "Setup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox cb_audioPB;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cb_audioCAP;
        private System.Windows.Forms.TextBox tb_txqrg;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_rxqrg;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rb_plutousb;
        private System.Windows.Forms.RadioButton rb_plutoeth;
        private System.Windows.Forms.TextBox tb_plutoip;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_plutooffset;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tb_lnboffset;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox comboBox_cpuspeed;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox comboBox_color;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBox_txpower;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button button_shutdown;
        private System.Windows.Forms.CheckBox cb_autosync;
        private System.Windows.Forms.Label label19;
    }
}