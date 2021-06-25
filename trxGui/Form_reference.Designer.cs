
namespace trxGui
{
    partial class Form_reference
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
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button_cal439 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label_cal439 = new System.Windows.Forms.Label();
            this.button_fin = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button_lnbfinished = new System.Windows.Forms.Button();
            this.label_caliblnb = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button_caliblnb = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.button_close = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(234, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "PLUTO clock (TCXO) calibration";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "1. click the CALIB439 button: ";
            // 
            // button_cal439
            // 
            this.button_cal439.Location = new System.Drawing.Point(208, 47);
            this.button_cal439.Name = "button_cal439";
            this.button_cal439.Size = new System.Drawing.Size(75, 23);
            this.button_cal439.TabIndex = 3;
            this.button_cal439.Text = "CALIB 439";
            this.button_cal439.UseVisualStyleBackColor = true;
            this.button_cal439.Click += new System.EventHandler(this.button_cal439_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(284, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "2. transmit a 439,000 MHz carrier using a 70cm transceiver";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(266, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "4. when you are done, click the 439 FINISHED button:";
            // 
            // label_cal439
            // 
            this.label_cal439.AutoSize = true;
            this.label_cal439.ForeColor = System.Drawing.Color.Red;
            this.label_cal439.Location = new System.Drawing.Point(289, 52);
            this.label_cal439.Name = "label_cal439";
            this.label_cal439.Size = new System.Drawing.Size(16, 13);
            this.label_cal439.TabIndex = 6;
            this.label_cal439.Text = "...";
            // 
            // button_fin
            // 
            this.button_fin.Location = new System.Drawing.Point(340, 113);
            this.button_fin.Name = "button_fin";
            this.button_fin.Size = new System.Drawing.Size(98, 23);
            this.button_fin.TabIndex = 7;
            this.button_fin.Text = "439 FINISHED";
            this.button_fin.UseVisualStyleBackColor = true;
            this.button_fin.Click += new System.EventHandler(this.button_fin_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 152);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "LNB calibration:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 97);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(367, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "3. Click the peak with the left mouse button to move it to the center 0 Hz line";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 212);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(440, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "2. Click the center of the BPSK beacon with the left mouse button to move it to t" +
    "he 0 Hz line";
            // 
            // button_lnbfinished
            // 
            this.button_lnbfinished.Location = new System.Drawing.Point(334, 366);
            this.button_lnbfinished.Name = "button_lnbfinished";
            this.button_lnbfinished.Size = new System.Drawing.Size(114, 23);
            this.button_lnbfinished.TabIndex = 15;
            this.button_lnbfinished.Text = "LNB FINISHED";
            this.button_lnbfinished.UseVisualStyleBackColor = true;
            this.button_lnbfinished.Click += new System.EventHandler(this.button_lnbfinished_Click);
            // 
            // label_caliblnb
            // 
            this.label_caliblnb.AutoSize = true;
            this.label_caliblnb.ForeColor = System.Drawing.Color.Red;
            this.label_caliblnb.Location = new System.Drawing.Point(294, 191);
            this.label_caliblnb.Name = "label_caliblnb";
            this.label_caliblnb.Size = new System.Drawing.Size(16, 13);
            this.label_caliblnb.TabIndex = 14;
            this.label_caliblnb.Text = "...";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 370);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(269, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "3. when you are done, click the LNB FINISHED button:";
            // 
            // button_caliblnb
            // 
            this.button_caliblnb.Location = new System.Drawing.Point(213, 186);
            this.button_caliblnb.Name = "button_caliblnb";
            this.button_caliblnb.Size = new System.Drawing.Size(75, 23);
            this.button_caliblnb.TabIndex = 11;
            this.button_caliblnb.Text = "CALIB LNB";
            this.button_caliblnb.UseVisualStyleBackColor = true;
            this.button_caliblnb.Click += new System.EventHandler(this.button_caliblnb_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 191);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(155, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "1. click the CALIB LNB button: ";
            // 
            // button_close
            // 
            this.button_close.Location = new System.Drawing.Point(378, 409);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(75, 23);
            this.button_close.TabIndex = 17;
            this.button_close.Text = "Close";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::trxGui.Properties.Resources.bcn_center;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Location = new System.Drawing.Point(20, 245);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(155, 114);
            this.panel1.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 229);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "Example:";
            // 
            // Form_reference
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 440);
            this.ControlBox = false;
            this.Controls.Add(this.label8);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button_lnbfinished);
            this.Controls.Add(this.label_caliblnb);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.button_caliblnb);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_fin);
            this.Controls.Add(this.label_cal439);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button_cal439);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_reference";
            this.ShowIcon = false;
            this.Text = "Referenz Frequency Calibration";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_cal439;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label_cal439;
        private System.Windows.Forms.Button button_fin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button_lnbfinished;
        private System.Windows.Forms.Label label_caliblnb;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button_caliblnb;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button_close;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label8;
    }
}