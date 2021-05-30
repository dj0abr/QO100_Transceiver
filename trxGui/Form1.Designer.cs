
namespace trxGui
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer_draw = new System.Windows.Forms.Timer(this.components);
            this.bt_info = new System.Windows.Forms.Button();
            this.gp_testmodes = new System.Windows.Forms.GroupBox();
            this.cb_rfloop = new System.Windows.Forms.CheckBox();
            this.cb_audioloop = new System.Windows.Forms.CheckBox();
            this.gp_qrg = new System.Windows.Forms.GroupBox();
            this.rb_rit = new System.Windows.Forms.RadioButton();
            this.rb_xit = new System.Windows.Forms.RadioButton();
            this.gp_copyqrg = new System.Windows.Forms.GroupBox();
            this.cb_txtorx = new System.Windows.Forms.CheckBox();
            this.cb_rxtotx = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel_qrg = new System.Windows.Forms.Panel();
            this.button_setup = new System.Windows.Forms.Button();
            this.gp_audio = new System.Windows.Forms.GroupBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.panel_bigspec = new trxGui.DoubleBufferedPanel();
            this.panel_bigwf = new trxGui.DoubleBufferedPanel();
            this.panel_bandplan = new trxGui.DoubleBufferedPanel();
            this.panel_smallwf = new trxGui.DoubleBufferedPanel();
            this.panel_smallspec = new trxGui.DoubleBufferedPanel();
            this.panel_smallqrg = new trxGui.DoubleBufferedPanel();
            this.gp_testmodes.SuspendLayout();
            this.gp_qrg.SuspendLayout();
            this.gp_copyqrg.SuspendLayout();
            this.gp_audio.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer_draw
            // 
            this.timer_draw.Interval = 60;
            this.timer_draw.Tick += new System.EventHandler(this.timer_draw_Tick);
            // 
            // bt_info
            // 
            this.bt_info.BackColor = System.Drawing.Color.Silver;
            this.bt_info.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_info.Location = new System.Drawing.Point(1057, 713);
            this.bt_info.Name = "bt_info";
            this.bt_info.Size = new System.Drawing.Size(75, 27);
            this.bt_info.TabIndex = 4;
            this.bt_info.Text = "Info";
            this.bt_info.UseVisualStyleBackColor = false;
            this.bt_info.Click += new System.EventHandler(this.bt_info_Click);
            // 
            // gp_testmodes
            // 
            this.gp_testmodes.Controls.Add(this.cb_rfloop);
            this.gp_testmodes.Controls.Add(this.cb_audioloop);
            this.gp_testmodes.Location = new System.Drawing.Point(788, 709);
            this.gp_testmodes.Name = "gp_testmodes";
            this.gp_testmodes.Size = new System.Drawing.Size(180, 40);
            this.gp_testmodes.TabIndex = 8;
            this.gp_testmodes.TabStop = false;
            this.gp_testmodes.Text = "Testmodes";
            // 
            // cb_rfloop
            // 
            this.cb_rfloop.AutoSize = true;
            this.cb_rfloop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_rfloop.Location = new System.Drawing.Point(106, 17);
            this.cb_rfloop.Name = "cb_rfloop";
            this.cb_rfloop.Size = new System.Drawing.Size(64, 17);
            this.cb_rfloop.TabIndex = 1;
            this.cb_rfloop.Text = "RF Loop";
            this.cb_rfloop.UseVisualStyleBackColor = true;
            this.cb_rfloop.CheckedChanged += new System.EventHandler(this.cb_rfloop_CheckedChanged);
            // 
            // cb_audioloop
            // 
            this.cb_audioloop.AutoSize = true;
            this.cb_audioloop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_audioloop.Location = new System.Drawing.Point(11, 17);
            this.cb_audioloop.Name = "cb_audioloop";
            this.cb_audioloop.Size = new System.Drawing.Size(77, 17);
            this.cb_audioloop.TabIndex = 0;
            this.cb_audioloop.Text = "Audio Loop";
            this.cb_audioloop.UseVisualStyleBackColor = true;
            this.cb_audioloop.CheckedChanged += new System.EventHandler(this.cb_audioloop_CheckedChanged);
            // 
            // gp_qrg
            // 
            this.gp_qrg.Controls.Add(this.rb_rit);
            this.gp_qrg.Controls.Add(this.rb_xit);
            this.gp_qrg.Location = new System.Drawing.Point(14, 713);
            this.gp_qrg.Name = "gp_qrg";
            this.gp_qrg.Size = new System.Drawing.Size(131, 40);
            this.gp_qrg.TabIndex = 9;
            this.gp_qrg.TabStop = false;
            this.gp_qrg.Text = "Mouse Wheel";
            // 
            // rb_rit
            // 
            this.rb_rit.AutoSize = true;
            this.rb_rit.BackColor = System.Drawing.Color.Gray;
            this.rb_rit.Checked = true;
            this.rb_rit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rb_rit.Location = new System.Drawing.Point(16, 13);
            this.rb_rit.Name = "rb_rit";
            this.rb_rit.Size = new System.Drawing.Size(48, 21);
            this.rb_rit.TabIndex = 2;
            this.rb_rit.TabStop = true;
            this.rb_rit.Text = "RIT";
            this.rb_rit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rb_rit.UseVisualStyleBackColor = false;
            // 
            // rb_xit
            // 
            this.rb_xit.AutoSize = true;
            this.rb_xit.BackColor = System.Drawing.Color.Gray;
            this.rb_xit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rb_xit.Location = new System.Drawing.Point(75, 13);
            this.rb_xit.Name = "rb_xit";
            this.rb_xit.Size = new System.Drawing.Size(47, 21);
            this.rb_xit.TabIndex = 3;
            this.rb_xit.Text = "XIT";
            this.rb_xit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rb_xit.UseVisualStyleBackColor = false;
            // 
            // gp_copyqrg
            // 
            this.gp_copyqrg.Controls.Add(this.cb_txtorx);
            this.gp_copyqrg.Controls.Add(this.cb_rxtotx);
            this.gp_copyqrg.Location = new System.Drawing.Point(151, 713);
            this.gp_copyqrg.Name = "gp_copyqrg";
            this.gp_copyqrg.Size = new System.Drawing.Size(144, 40);
            this.gp_copyqrg.TabIndex = 10;
            this.gp_copyqrg.TabStop = false;
            this.gp_copyqrg.Text = "QRG copy";
            // 
            // cb_txtorx
            // 
            this.cb_txtorx.AutoSize = true;
            this.cb_txtorx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_txtorx.Location = new System.Drawing.Point(71, 15);
            this.cb_txtorx.Name = "cb_txtorx";
            this.cb_txtorx.Size = new System.Drawing.Size(58, 17);
            this.cb_txtorx.TabIndex = 1;
            this.cb_txtorx.Text = "TX>RX";
            this.cb_txtorx.UseVisualStyleBackColor = true;
            this.cb_txtorx.CheckedChanged += new System.EventHandler(this.cb_txtorx_CheckedChanged);
            // 
            // cb_rxtotx
            // 
            this.cb_rxtotx.AutoSize = true;
            this.cb_rxtotx.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_rxtotx.Location = new System.Drawing.Point(7, 15);
            this.cb_rxtotx.Name = "cb_rxtotx";
            this.cb_rxtotx.Size = new System.Drawing.Size(58, 17);
            this.cb_rxtotx.TabIndex = 0;
            this.cb_rxtotx.Text = "RX>TX";
            this.cb_rxtotx.UseVisualStyleBackColor = true;
            this.cb_rxtotx.CheckedChanged += new System.EventHandler(this.cb_rxtotx_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGreen;
            this.panel1.Location = new System.Drawing.Point(433, 716);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(345, 33);
            this.panel1.TabIndex = 11;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseClick);
            // 
            // panel_qrg
            // 
            this.panel_qrg.BackColor = System.Drawing.Color.Gray;
            this.panel_qrg.Location = new System.Drawing.Point(12, 3);
            this.panel_qrg.Name = "panel_qrg";
            this.panel_qrg.Size = new System.Drawing.Size(1120, 41);
            this.panel_qrg.TabIndex = 5;
            this.panel_qrg.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_qrg_Paint);
            // 
            // button_setup
            // 
            this.button_setup.BackColor = System.Drawing.Color.Silver;
            this.button_setup.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_setup.Location = new System.Drawing.Point(976, 713);
            this.button_setup.Name = "button_setup";
            this.button_setup.Size = new System.Drawing.Size(75, 27);
            this.button_setup.TabIndex = 12;
            this.button_setup.Text = "SETUP";
            this.button_setup.UseVisualStyleBackColor = false;
            this.button_setup.Click += new System.EventHandler(this.button_setup_Click);
            // 
            // gp_audio
            // 
            this.gp_audio.Controls.Add(this.checkBox2);
            this.gp_audio.Location = new System.Drawing.Point(301, 713);
            this.gp_audio.Name = "gp_audio";
            this.gp_audio.Size = new System.Drawing.Size(89, 40);
            this.gp_audio.TabIndex = 11;
            this.gp_audio.TabStop = false;
            this.gp_audio.Text = "Audio";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBox2.Location = new System.Drawing.Point(7, 16);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(69, 17);
            this.checkBox2.TabIndex = 0;
            this.checkBox2.Text = "Compress";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // panel_bigspec
            // 
            this.panel_bigspec.BackColor = System.Drawing.Color.LightGray;
            this.panel_bigspec.Location = new System.Drawing.Point(12, 50);
            this.panel_bigspec.Name = "panel_bigspec";
            this.panel_bigspec.Size = new System.Drawing.Size(1120, 150);
            this.panel_bigspec.TabIndex = 0;
            this.panel_bigspec.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_bigspec_Paint);
            this.panel_bigspec.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_bigwf_MouseClick);
            this.panel_bigspec.MouseHover += new System.EventHandler(this.panel_bigspec_MouseHover);
            // 
            // panel_bigwf
            // 
            this.panel_bigwf.BackColor = System.Drawing.Color.Black;
            this.panel_bigwf.Location = new System.Drawing.Point(12, 225);
            this.panel_bigwf.Name = "panel_bigwf";
            this.panel_bigwf.Size = new System.Drawing.Size(1120, 150);
            this.panel_bigwf.TabIndex = 1;
            this.panel_bigwf.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_bigwf_Paint);
            this.panel_bigwf.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_bigwf_MouseClick);
            this.panel_bigwf.MouseHover += new System.EventHandler(this.panel_bigwf_MouseHover);
            // 
            // panel_bandplan
            // 
            this.panel_bandplan.BackColor = System.Drawing.Color.Silver;
            this.panel_bandplan.Location = new System.Drawing.Point(14, 206);
            this.panel_bandplan.Name = "panel_bandplan";
            this.panel_bandplan.Size = new System.Drawing.Size(1120, 12);
            this.panel_bandplan.TabIndex = 1;
            this.panel_bandplan.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_bandplan_Paint);
            // 
            // panel_smallwf
            // 
            this.panel_smallwf.BackColor = System.Drawing.Color.Black;
            this.panel_smallwf.Location = new System.Drawing.Point(12, 554);
            this.panel_smallwf.Name = "panel_smallwf";
            this.panel_smallwf.Size = new System.Drawing.Size(1120, 150);
            this.panel_smallwf.TabIndex = 1;
            this.panel_smallwf.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_smallwf_Paint);
            this.panel_smallwf.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_smallwf_MouseClick);
            this.panel_smallwf.MouseHover += new System.EventHandler(this.panel_smallwf_MouseHover);
            // 
            // panel_smallspec
            // 
            this.panel_smallspec.BackColor = System.Drawing.Color.LightGray;
            this.panel_smallspec.Location = new System.Drawing.Point(12, 381);
            this.panel_smallspec.Name = "panel_smallspec";
            this.panel_smallspec.Size = new System.Drawing.Size(1120, 150);
            this.panel_smallspec.TabIndex = 1;
            this.panel_smallspec.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_smallspec_Paint);
            this.panel_smallspec.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_smallwf_MouseClick);
            this.panel_smallspec.MouseHover += new System.EventHandler(this.panel_smallspec_MouseHover);
            // 
            // panel_smallqrg
            // 
            this.panel_smallqrg.BackColor = System.Drawing.Color.Silver;
            this.panel_smallqrg.Location = new System.Drawing.Point(14, 536);
            this.panel_smallqrg.Name = "panel_smallqrg";
            this.panel_smallqrg.Size = new System.Drawing.Size(1120, 12);
            this.panel_smallqrg.TabIndex = 2;
            this.panel_smallqrg.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_smallqrg_Paint);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(1147, 761);
            this.Controls.Add(this.gp_audio);
            this.Controls.Add(this.button_setup);
            this.Controls.Add(this.panel_qrg);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel_bigspec);
            this.Controls.Add(this.bt_info);
            this.Controls.Add(this.gp_copyqrg);
            this.Controls.Add(this.panel_bigwf);
            this.Controls.Add(this.panel_bandplan);
            this.Controls.Add(this.gp_qrg);
            this.Controls.Add(this.panel_smallwf);
            this.Controls.Add(this.panel_smallspec);
            this.Controls.Add(this.gp_testmodes);
            this.Controls.Add(this.panel_smallqrg);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Form1";
            this.Text = "QO100 Linux - Pluto Transceiver (by DJ0ABR) V1.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.gp_testmodes.ResumeLayout(false);
            this.gp_testmodes.PerformLayout();
            this.gp_qrg.ResumeLayout(false);
            this.gp_qrg.PerformLayout();
            this.gp_copyqrg.ResumeLayout(false);
            this.gp_copyqrg.PerformLayout();
            this.gp_audio.ResumeLayout(false);
            this.gp_audio.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer_draw;
        private System.Windows.Forms.Button bt_info;
        private DoubleBufferedPanel panel_smallqrg;
        private DoubleBufferedPanel panel_bandplan;
        private DoubleBufferedPanel panel_smallwf;
        private System.Windows.Forms.GroupBox gp_testmodes;
        private System.Windows.Forms.CheckBox cb_rfloop;
        private System.Windows.Forms.CheckBox cb_audioloop;
        private DoubleBufferedPanel panel_smallspec;
        private System.Windows.Forms.GroupBox gp_qrg;
        private System.Windows.Forms.RadioButton rb_rit;
        private System.Windows.Forms.RadioButton rb_xit;
        private DoubleBufferedPanel panel_bigwf;
        private System.Windows.Forms.GroupBox gp_copyqrg;
        private System.Windows.Forms.CheckBox cb_txtorx;
        private System.Windows.Forms.CheckBox cb_rxtotx;
        private DoubleBufferedPanel panel_bigspec;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel_qrg;
        private System.Windows.Forms.Button button_setup;
        private System.Windows.Forms.GroupBox gp_audio;
        private System.Windows.Forms.CheckBox checkBox2;
    }
}

