
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_setup));
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
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rb_plutousb = new System.Windows.Forms.RadioButton();
            this.rb_plutoeth = new System.Windows.Forms.RadioButton();
            this.tb_plutoip = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(713, 415);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Location = new System.Drawing.Point(632, 415);
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
            this.cb_audioPB.Location = new System.Drawing.Point(154, 69);
            this.cb_audioPB.Name = "cb_audioPB";
            this.cb_audioPB.Size = new System.Drawing.Size(280, 21);
            this.cb_audioPB.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 72);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(133, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "Loadspeaker/Headphone:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 96);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(66, 13);
            this.label11.TabIndex = 12;
            this.label11.Text = "Microphone:";
            // 
            // cb_audioCAP
            // 
            this.cb_audioCAP.FormattingEnabled = true;
            this.cb_audioCAP.Location = new System.Drawing.Point(154, 93);
            this.cb_audioCAP.Name = "cb_audioCAP";
            this.cb_audioCAP.Size = new System.Drawing.Size(280, 21);
            this.cb_audioCAP.TabIndex = 13;
            // 
            // tb_txqrg
            // 
            this.tb_txqrg.Location = new System.Drawing.Point(154, 152);
            this.tb_txqrg.Name = "tb_txqrg";
            this.tb_txqrg.Size = new System.Drawing.Size(177, 20);
            this.tb_txqrg.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Transmitter Frequency:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(337, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(361, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "MHz ( enter Frequency corresponding to lower beacon at 10489.500 MHz )";
            // 
            // tb_rxqrg
            // 
            this.tb_rxqrg.Location = new System.Drawing.Point(154, 129);
            this.tb_rxqrg.Name = "tb_rxqrg";
            this.tb_rxqrg.Size = new System.Drawing.Size(177, 20);
            this.tb_rxqrg.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Receiver Frequency:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(337, 159);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(355, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "MHz ( enter Frequency corresponding to lower beacon at 2400.000 MHz )";
            // 
            // textBox3
            // 
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Location = new System.Drawing.Point(15, 211);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(435, 227);
            this.textBox3.TabIndex = 29;
            this.textBox3.Text = resources.GetString("textBox3.Text");
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(483, 211);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(300, 193);
            this.textBox1.TabIndex = 30;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 31;
            this.label4.Text = "Pluto Address:";
            // 
            // rb_plutousb
            // 
            this.rb_plutousb.AutoSize = true;
            this.rb_plutousb.Checked = true;
            this.rb_plutousb.Location = new System.Drawing.Point(154, 23);
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
            this.rb_plutoeth.Location = new System.Drawing.Point(259, 23);
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
            this.tb_plutoip.Location = new System.Drawing.Point(330, 22);
            this.tb_plutoip.Name = "tb_plutoip";
            this.tb_plutoip.Size = new System.Drawing.Size(142, 20);
            this.tb_plutoip.TabIndex = 34;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(480, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(141, 13);
            this.label5.TabIndex = 35;
            this.label5.Text = "enter IP address of the Pluto";
            // 
            // Form_setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb_plutoip);
            this.Controls.Add(this.rb_plutoeth);
            this.Controls.Add(this.rb_plutousb);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBox3);
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
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rb_plutousb;
        private System.Windows.Forms.RadioButton rb_plutoeth;
        private System.Windows.Forms.TextBox tb_plutoip;
        private System.Windows.Forms.Label label5;
    }
}