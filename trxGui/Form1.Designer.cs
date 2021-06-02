
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
            this.gp_audio = new System.Windows.Forms.GroupBox();
            this.cb_audioagc = new System.Windows.Forms.CheckBox();
            this.gp_filter = new System.Windows.Forms.GroupBox();
            this.cb_filterRX = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_filterTX = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_compressor = new System.Windows.Forms.ComboBox();
            this.panel_info = new System.Windows.Forms.Panel();
            this.panel_setup = new System.Windows.Forms.Panel();
            this.panel_bigspec = new trxGui.DoubleBufferedPanel();
            this.panel_bigwf = new trxGui.DoubleBufferedPanel();
            this.panel_bandplan = new trxGui.DoubleBufferedPanel();
            this.panel_smallwf = new trxGui.DoubleBufferedPanel();
            this.panel_smallspec = new trxGui.DoubleBufferedPanel();
            this.panel_smallqrg = new trxGui.DoubleBufferedPanel();
            this.cb_rxmute = new System.Windows.Forms.CheckBox();
            this.gp_testmodes.SuspendLayout();
            this.gp_qrg.SuspendLayout();
            this.gp_copyqrg.SuspendLayout();
            this.gp_audio.SuspendLayout();
            this.gp_filter.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer_draw
            // 
            this.timer_draw.Interval = 60;
            this.timer_draw.Tick += new System.EventHandler(this.timer_draw_Tick);
            // 
            // gp_testmodes
            // 
            this.gp_testmodes.Controls.Add(this.cb_rfloop);
            this.gp_testmodes.Controls.Add(this.cb_audioloop);
            this.gp_testmodes.Location = new System.Drawing.Point(800, 709);
            this.gp_testmodes.Name = "gp_testmodes";
            this.gp_testmodes.Size = new System.Drawing.Size(168, 40);
            this.gp_testmodes.TabIndex = 8;
            this.gp_testmodes.TabStop = false;
            this.gp_testmodes.Text = "Testmodes";
            // 
            // cb_rfloop
            // 
            this.cb_rfloop.AutoSize = true;
            this.cb_rfloop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_rfloop.Location = new System.Drawing.Point(99, 17);
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
            this.cb_audioloop.Location = new System.Drawing.Point(13, 17);
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
            this.gp_qrg.Size = new System.Drawing.Size(126, 40);
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
            this.rb_xit.Location = new System.Drawing.Point(71, 13);
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
            this.gp_copyqrg.Location = new System.Drawing.Point(146, 713);
            this.gp_copyqrg.Name = "gp_copyqrg";
            this.gp_copyqrg.Size = new System.Drawing.Size(137, 40);
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
            this.panel1.Location = new System.Drawing.Point(14, 759);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1118, 33);
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
            // gp_audio
            // 
            this.gp_audio.Controls.Add(this.cb_rxmute);
            this.gp_audio.Controls.Add(this.label3);
            this.gp_audio.Controls.Add(this.cb_audioagc);
            this.gp_audio.Controls.Add(this.cb_compressor);
            this.gp_audio.Location = new System.Drawing.Point(296, 713);
            this.gp_audio.Name = "gp_audio";
            this.gp_audio.Size = new System.Drawing.Size(231, 40);
            this.gp_audio.TabIndex = 11;
            this.gp_audio.TabStop = false;
            this.gp_audio.Text = "Audio";
            // 
            // cb_audioagc
            // 
            this.cb_audioagc.AutoSize = true;
            this.cb_audioagc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_audioagc.Location = new System.Drawing.Point(105, 16);
            this.cb_audioagc.Name = "cb_audioagc";
            this.cb_audioagc.Size = new System.Drawing.Size(45, 17);
            this.cb_audioagc.TabIndex = 1;
            this.cb_audioagc.Text = "AGC";
            this.cb_audioagc.UseVisualStyleBackColor = true;
            this.cb_audioagc.CheckedChanged += new System.EventHandler(this.cb_audioagc_CheckedChanged);
            // 
            // gp_filter
            // 
            this.gp_filter.Controls.Add(this.label2);
            this.gp_filter.Controls.Add(this.cb_filterTX);
            this.gp_filter.Controls.Add(this.label1);
            this.gp_filter.Controls.Add(this.cb_filterRX);
            this.gp_filter.Location = new System.Drawing.Point(576, 713);
            this.gp_filter.Name = "gp_filter";
            this.gp_filter.Size = new System.Drawing.Size(195, 40);
            this.gp_filter.TabIndex = 13;
            this.gp_filter.TabStop = false;
            this.gp_filter.Text = "Filter";
            // 
            // cb_filterRX
            // 
            this.cb_filterRX.FormattingEnabled = true;
            this.cb_filterRX.Items.AddRange(new object[] {
            "1 kHz",
            "1.8 kHz",
            "2.7 kHz",
            "3.6 kHz"});
            this.cb_filterRX.Location = new System.Drawing.Point(30, 15);
            this.cb_filterRX.Name = "cb_filterRX";
            this.cb_filterRX.Size = new System.Drawing.Size(65, 21);
            this.cb_filterRX.TabIndex = 0;
            this.cb_filterRX.Text = "2.7 kHz";
            this.cb_filterRX.SelectedIndexChanged += new System.EventHandler(this.cb_filterRX_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "RX:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(99, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "TX:";
            // 
            // cb_filterTX
            // 
            this.cb_filterTX.FormattingEnabled = true;
            this.cb_filterTX.Items.AddRange(new object[] {
            "1 kHz",
            "1.8 kHz",
            "2.2 kHz",
            "2.7 kHz"});
            this.cb_filterTX.Location = new System.Drawing.Point(123, 15);
            this.cb_filterTX.Name = "cb_filterTX";
            this.cb_filterTX.Size = new System.Drawing.Size(65, 21);
            this.cb_filterTX.TabIndex = 2;
            this.cb_filterTX.Text = "2.7 kHz";
            this.cb_filterTX.SelectedIndexChanged += new System.EventHandler(this.cb_filterTX_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Compr:";
            // 
            // cb_compressor
            // 
            this.cb_compressor.FormattingEnabled = true;
            this.cb_compressor.Items.AddRange(new object[] {
            "OFF",
            "low",
            "mid",
            "high"});
            this.cb_compressor.Location = new System.Drawing.Point(46, 15);
            this.cb_compressor.Name = "cb_compressor";
            this.cb_compressor.Size = new System.Drawing.Size(48, 21);
            this.cb_compressor.TabIndex = 4;
            this.cb_compressor.Text = "OFF";
            this.cb_compressor.SelectedIndexChanged += new System.EventHandler(this.cb_compressor_SelectedIndexChanged);
            // 
            // panel_info
            // 
            this.panel_info.BackColor = System.Drawing.Color.White;
            this.panel_info.BackgroundImage = global::trxGui.Properties.Resources.info;
            this.panel_info.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel_info.Location = new System.Drawing.Point(1099, 715);
            this.panel_info.Name = "panel_info";
            this.panel_info.Size = new System.Drawing.Size(32, 32);
            this.panel_info.TabIndex = 15;
            this.panel_info.Click += new System.EventHandler(this.bt_info_click);
            // 
            // panel_setup
            // 
            this.panel_setup.BackColor = System.Drawing.Color.White;
            this.panel_setup.BackgroundImage = global::trxGui.Properties.Resources.setup;
            this.panel_setup.Location = new System.Drawing.Point(1060, 715);
            this.panel_setup.Name = "panel_setup";
            this.panel_setup.Size = new System.Drawing.Size(32, 32);
            this.panel_setup.TabIndex = 14;
            this.panel_setup.Click += new System.EventHandler(this.butto_setup_click);
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
            this.panel_bigspec.MouseLeave += new System.EventHandler(this.panel_bigspec_MouseLeave);
            this.panel_bigspec.MouseHover += new System.EventHandler(this.panel_bigspec_MouseHover);
            this.panel_bigspec.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel_bigspec_MouseMove);
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
            this.panel_bigwf.MouseLeave += new System.EventHandler(this.panel_bigwf_MouseLeave);
            this.panel_bigwf.MouseHover += new System.EventHandler(this.panel_bigwf_MouseHover);
            this.panel_bigwf.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel_bigwf_MouseMove);
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
            this.panel_smallwf.MouseLeave += new System.EventHandler(this.panel_smallwf_MouseLeave);
            this.panel_smallwf.MouseHover += new System.EventHandler(this.panel_smallwf_MouseHover);
            this.panel_smallwf.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel_smallwf_MouseMove);
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
            this.panel_smallspec.MouseLeave += new System.EventHandler(this.panel_smallspec_MouseLeave);
            this.panel_smallspec.MouseHover += new System.EventHandler(this.panel_smallspec_MouseHover);
            this.panel_smallspec.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel_smallspec_MouseMove);
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
            // cb_rxmute
            // 
            this.cb_rxmute.AutoSize = true;
            this.cb_rxmute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_rxmute.Location = new System.Drawing.Point(156, 16);
            this.cb_rxmute.Name = "cb_rxmute";
            this.cb_rxmute.Size = new System.Drawing.Size(65, 17);
            this.cb_rxmute.TabIndex = 6;
            this.cb_rxmute.Text = "RX Mute";
            this.cb_rxmute.UseVisualStyleBackColor = true;
            this.cb_rxmute.CheckedChanged += new System.EventHandler(this.cb_rxmute_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(1147, 802);
            this.Controls.Add(this.panel_info);
            this.Controls.Add(this.panel_setup);
            this.Controls.Add(this.gp_filter);
            this.Controls.Add(this.gp_audio);
            this.Controls.Add(this.panel_qrg);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel_bigspec);
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
            this.Text = "QO100 Linux - Pluto Transceiver (by DJ0ABR) V1.2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.gp_testmodes.ResumeLayout(false);
            this.gp_testmodes.PerformLayout();
            this.gp_qrg.ResumeLayout(false);
            this.gp_qrg.PerformLayout();
            this.gp_copyqrg.ResumeLayout(false);
            this.gp_copyqrg.PerformLayout();
            this.gp_audio.ResumeLayout(false);
            this.gp_audio.PerformLayout();
            this.gp_filter.ResumeLayout(false);
            this.gp_filter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer_draw;
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
        private System.Windows.Forms.GroupBox gp_audio;
        private System.Windows.Forms.CheckBox cb_audioagc;
        private System.Windows.Forms.GroupBox gp_filter;
        private System.Windows.Forms.ComboBox cb_filterRX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_filterTX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cb_compressor;
        private System.Windows.Forms.Panel panel_setup;
        private System.Windows.Forms.Panel panel_info;
        private System.Windows.Forms.CheckBox cb_rxmute;
    }
}

