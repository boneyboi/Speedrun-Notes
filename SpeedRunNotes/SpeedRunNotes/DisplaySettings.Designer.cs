namespace LiveSplit.UI.Components
{
    partial class DisplaySettings
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TWidth = new System.Windows.Forms.TextBox();
            this.THeight = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.BGcolor = new System.Windows.Forms.ColorDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Import = new System.Windows.Forms.Button();
            this.Export = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.ImportD = new System.Windows.Forms.FolderBrowserDialog();
            this.ExportD = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TWidth);
            this.groupBox1.Controls.Add(this.THeight);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 127);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(355, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Display Settings";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // TWidth
            // 
            this.TWidth.Location = new System.Drawing.Point(95, 69);
            this.TWidth.Name = "TWidth";
            this.TWidth.Size = new System.Drawing.Size(48, 20);
            this.TWidth.TabIndex = 5;
            // 
            // THeight
            // 
            this.THeight.Location = new System.Drawing.Point(95, 43);
            this.THeight.Name = "THeight";
            this.THeight.Size = new System.Drawing.Size(48, 20);
            this.THeight.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(119, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 24);
            this.button1.TabIndex = 3;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Window Width:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Window Height:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Background Color:";
            // 
            // BGcolor
            // 
            this.BGcolor.Color = System.Drawing.Color.White;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.Import);
            this.groupBox2.Controls.Add(this.Export);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(353, 116);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Note Editor";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(6, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(147, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "export here to save your work";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(6, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(292, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Warning: Live Split does not save your notes with the layout,";
            // 
            // Import
            // 
            this.Import.Location = new System.Drawing.Point(179, 80);
            this.Import.Name = "Import";
            this.Import.Size = new System.Drawing.Size(57, 23);
            this.Import.TabIndex = 4;
            this.Import.Text = "Import";
            this.Import.UseVisualStyleBackColor = true;
            // 
            // Export
            // 
            this.Export.Location = new System.Drawing.Point(116, 80);
            this.Export.Name = "Export";
            this.Export.Size = new System.Drawing.Size(57, 23);
            this.Export.TabIndex = 3;
            this.Export.Text = "Export";
            this.Export.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Export/Import Notes:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(116, 16);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 21);
            this.button2.TabIndex = 1;
            this.button2.Text = "Open Note Editor";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Note Editor:";
            // 
            // ImportD
            // 
            this.ImportD.ShowNewFolderButton = false;
            // 
            // DisplaySettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "DisplaySettings";
            this.Size = new System.Drawing.Size(361, 445);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColorDialog BGcolor;
        private System.Windows.Forms.TextBox TWidth;
        private System.Windows.Forms.TextBox THeight;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Import;
        private System.Windows.Forms.Button Export;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.FolderBrowserDialog ImportD;
        public System.Windows.Forms.FolderBrowserDialog ExportD;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
    }
}
