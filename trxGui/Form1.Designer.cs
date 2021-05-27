
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
            this.rb_rit = new System.Windows.Forms.RadioButton();
            this.rb_xit = new System.Windows.Forms.RadioButton();
            this.bt_info = new System.Windows.Forms.Button();
            this.panel_bandplan = new trxGui.DoubleBufferedPanel();
            this.panel_smallwf = new trxGui.DoubleBufferedPanel();
            this.panel_smallspec = new trxGui.DoubleBufferedPanel();
            this.panel_smallqrg = new trxGui.DoubleBufferedPanel();
            this.panel_bigwf = new trxGui.DoubleBufferedPanel();
            this.panel_bigspec = new trxGui.DoubleBufferedPanel();
            this.SuspendLayout();
            // 
            // timer_draw
            // 
            this.timer_draw.Interval = 60;
            this.timer_draw.Tick += new System.EventHandler(this.timer_draw_Tick);
            // 
            // rb_rit
            // 
            this.rb_rit.Appearance = System.Windows.Forms.Appearance.Button;
            this.rb_rit.AutoSize = true;
            this.rb_rit.BackColor = System.Drawing.Color.Silver;
            this.rb_rit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rb_rit.Location = new System.Drawing.Point(13, 677);
            this.rb_rit.Name = "rb_rit";
            this.rb_rit.Size = new System.Drawing.Size(40, 27);
            this.rb_rit.TabIndex = 2;
            this.rb_rit.TabStop = true;
            this.rb_rit.Text = "RIT";
            this.rb_rit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rb_rit.UseVisualStyleBackColor = false;
            // 
            // rb_xit
            // 
            this.rb_xit.Appearance = System.Windows.Forms.Appearance.Button;
            this.rb_xit.AutoSize = true;
            this.rb_xit.BackColor = System.Drawing.Color.Silver;
            this.rb_xit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rb_xit.Location = new System.Drawing.Point(54, 677);
            this.rb_xit.Name = "rb_xit";
            this.rb_xit.Size = new System.Drawing.Size(39, 27);
            this.rb_xit.TabIndex = 3;
            this.rb_xit.TabStop = true;
            this.rb_xit.Text = "XIT";
            this.rb_xit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rb_xit.UseVisualStyleBackColor = false;
            // 
            // bt_info
            // 
            this.bt_info.BackColor = System.Drawing.Color.Silver;
            this.bt_info.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_info.Location = new System.Drawing.Point(1058, 676);
            this.bt_info.Name = "bt_info";
            this.bt_info.Size = new System.Drawing.Size(75, 23);
            this.bt_info.TabIndex = 4;
            this.bt_info.Text = "Info";
            this.bt_info.UseVisualStyleBackColor = false;
            // 
            // panel_bandplan
            // 
            this.panel_bandplan.BackColor = System.Drawing.Color.Silver;
            this.panel_bandplan.Location = new System.Drawing.Point(15, 169);
            this.panel_bandplan.Name = "panel_bandplan";
            this.panel_bandplan.Size = new System.Drawing.Size(1120, 12);
            this.panel_bandplan.TabIndex = 1;
            // 
            // panel_smallwf
            // 
            this.panel_smallwf.BackColor = System.Drawing.Color.Black;
            this.panel_smallwf.Location = new System.Drawing.Point(13, 517);
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
            this.panel_smallspec.Location = new System.Drawing.Point(13, 344);
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
            this.panel_smallqrg.Location = new System.Drawing.Point(15, 499);
            this.panel_smallqrg.Name = "panel_smallqrg";
            this.panel_smallqrg.Size = new System.Drawing.Size(1120, 12);
            this.panel_smallqrg.TabIndex = 2;
            this.panel_smallqrg.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_smallqrg_Paint);
            // 
            // panel_bigwf
            // 
            this.panel_bigwf.BackColor = System.Drawing.Color.Black;
            this.panel_bigwf.Location = new System.Drawing.Point(13, 188);
            this.panel_bigwf.Name = "panel_bigwf";
            this.panel_bigwf.Size = new System.Drawing.Size(1120, 150);
            this.panel_bigwf.TabIndex = 1;
            this.panel_bigwf.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_bigwf_Paint);
            this.panel_bigwf.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_bigwf_MouseClick);
            this.panel_bigwf.MouseHover += new System.EventHandler(this.panel_bigwf_MouseHover);
            // 
            // panel_bigspec
            // 
            this.panel_bigspec.BackColor = System.Drawing.Color.LightGray;
            this.panel_bigspec.Location = new System.Drawing.Point(13, 13);
            this.panel_bigspec.Name = "panel_bigspec";
            this.panel_bigspec.Size = new System.Drawing.Size(1120, 150);
            this.panel_bigspec.TabIndex = 0;
            this.panel_bigspec.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_bigspec_Paint);
            this.panel_bigspec.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_bigwf_MouseClick);
            this.panel_bigspec.MouseHover += new System.EventHandler(this.panel_bigspec_MouseHover);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(1147, 711);
            this.Controls.Add(this.panel_smallqrg);
            this.Controls.Add(this.bt_info);
            this.Controls.Add(this.rb_xit);
            this.Controls.Add(this.rb_rit);
            this.Controls.Add(this.panel_bandplan);
            this.Controls.Add(this.panel_smallwf);
            this.Controls.Add(this.panel_smallspec);
            this.Controls.Add(this.panel_bigwf);
            this.Controls.Add(this.panel_bigspec);
            this.Name = "Form1";
            this.Text = "QO100 Linux - Pluto Transceiver (by DJ0ABR) V1.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DoubleBufferedPanel panel_bigspec;
        private System.Windows.Forms.Timer timer_draw;
        private DoubleBufferedPanel panel_bigwf;
        private DoubleBufferedPanel panel_smallspec;
        private DoubleBufferedPanel panel_smallwf;
        private DoubleBufferedPanel panel_bandplan;
        private System.Windows.Forms.RadioButton rb_rit;
        private System.Windows.Forms.RadioButton rb_xit;
        private System.Windows.Forms.Button bt_info;
        private DoubleBufferedPanel panel_smallqrg;
    }
}

