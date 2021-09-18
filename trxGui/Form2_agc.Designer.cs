
namespace trxGui
{
    partial class Form2_agc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2_agc));
            this.label1 = new System.Windows.Forms.Label();
            this.cb_bass = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tb_micboostcol = new ColorSlider.ColorSlider();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mic-Boost:";
            // 
            // cb_bass
            // 
            this.cb_bass.AutoSize = true;
            this.cb_bass.Location = new System.Drawing.Point(90, 102);
            this.cb_bass.Name = "cb_bass";
            this.cb_bass.Size = new System.Drawing.Size(49, 17);
            this.cb_bass.TabIndex = 3;
            this.cb_bass.Text = "Bass";
            this.cb_bass.UseVisualStyleBackColor = true;
            this.cb_bass.CheckedChanged += new System.EventHandler(this.cb_bass_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(325, 102);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::trxGui.Properties.Resources.microphone;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel1.Location = new System.Drawing.Point(23, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(40, 33);
            this.panel1.TabIndex = 6;
            // 
            // tb_micboostcol
            // 
            this.tb_micboostcol.BackColor = System.Drawing.Color.Gray;
            this.tb_micboostcol.BarPenColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(94)))), ((int)(((byte)(110)))));
            this.tb_micboostcol.BarPenColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(60)))), ((int)(((byte)(74)))));
            this.tb_micboostcol.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.tb_micboostcol.ElapsedInnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(56)))), ((int)(((byte)(152)))));
            this.tb_micboostcol.ElapsedPenColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(130)))), ((int)(((byte)(208)))));
            this.tb_micboostcol.ElapsedPenColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(140)))), ((int)(((byte)(180)))));
            this.tb_micboostcol.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.tb_micboostcol.ForeColor = System.Drawing.Color.White;
            this.tb_micboostcol.LargeChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tb_micboostcol.Location = new System.Drawing.Point(89, 3);
            this.tb_micboostcol.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.tb_micboostcol.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tb_micboostcol.Name = "tb_micboostcol";
            this.tb_micboostcol.Padding = 5;
            this.tb_micboostcol.ScaleDivisions = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.tb_micboostcol.ScaleSubDivisions = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tb_micboostcol.ShowDivisionsText = true;
            this.tb_micboostcol.ShowSmallScale = false;
            this.tb_micboostcol.Size = new System.Drawing.Size(311, 64);
            this.tb_micboostcol.SmallChange = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tb_micboostcol.TabIndex = 0;
            this.tb_micboostcol.Text = "colorSlider1";
            this.tb_micboostcol.ThumbInnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(56)))), ((int)(((byte)(152)))));
            this.tb_micboostcol.ThumbPenColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(56)))), ((int)(((byte)(152)))));
            this.tb_micboostcol.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.tb_micboostcol.ThumbSize = new System.Drawing.Size(16, 16);
            this.tb_micboostcol.TickAdd = 0F;
            this.tb_micboostcol.TickColor = System.Drawing.Color.White;
            this.tb_micboostcol.TickDivide = 0F;
            this.tb_micboostcol.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.tb_micboostcol.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tb_micboostcol.Scroll += new System.Windows.Forms.ScrollEventHandler(this.tb_micboostcol_Scroll);
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::trxGui.Properties.Resources.note;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel2.Location = new System.Drawing.Point(23, 75);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(40, 43);
            this.panel2.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Mic-Bass:";
            // 
            // Form2_agc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(444, 148);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cb_bass);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_micboostcol);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2_agc";
            this.ShowInTaskbar = false;
            this.Text = "TX Audio";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ColorSlider.ColorSlider tb_micboostcol;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cb_bass;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
    }
}