namespace SpeedRunNotes
{
    partial class Editor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.FontColor = new System.Windows.Forms.Button();
            this.FColor = new System.Windows.Forms.ColorDialog();
            this.FFont = new System.Windows.Forms.FontDialog();
            this.FontChange = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ImageInsert = new System.Windows.Forms.OpenFileDialog();
            this.InsertButton = new System.Windows.Forms.Button();
            this.OKbutton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SymbolButton = new System.Windows.Forms.Button();
            this.SymbolHelp = new System.Windows.Forms.Button();
            this.controllerType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.AcceptsTab = true;
            this.richTextBox1.EnableAutoDragDrop = true;
            this.richTextBox1.HideSelection = false;
            this.richTextBox1.Location = new System.Drawing.Point(0, 40);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(800, 365);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            // 
            // FontColor
            // 
            this.FontColor.BackColor = System.Drawing.Color.Black;
            this.FontColor.Location = new System.Drawing.Point(137, 12);
            this.FontColor.Name = "FontColor";
            this.FontColor.Size = new System.Drawing.Size(24, 22);
            this.FontColor.TabIndex = 2;
            this.FontColor.UseVisualStyleBackColor = false;
            // 
            // FontChange
            // 
            this.FontChange.Location = new System.Drawing.Point(41, 11);
            this.FontChange.Name = "FontChange";
            this.FontChange.Size = new System.Drawing.Size(90, 23);
            this.FontChange.TabIndex = 3;
            this.FontChange.Text = "Change Font";
            this.FontChange.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Location = new System.Drawing.Point(0, 407);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 42);
            this.panel1.TabIndex = 4;
            // 
            // ImageInsert
            // 
            this.ImageInsert.FileName = "openFileDialog1";
            this.ImageInsert.Filter = "PNG files|*.png|GIF files|*.gif|JPG files|*.jpg|JPEG files|*.jpeg|BMP files|*.bmp" +
    "|WMF files|*.wmf";
            // 
            // InsertButton
            // 
            this.InsertButton.Location = new System.Drawing.Point(180, 11);
            this.InsertButton.Name = "InsertButton";
            this.InsertButton.Size = new System.Drawing.Size(75, 23);
            this.InsertButton.TabIndex = 5;
            this.InsertButton.Text = "Insert Image";
            this.InsertButton.UseVisualStyleBackColor = true;
            this.InsertButton.Click += new System.EventHandler(this.InsertButton_Click);
            // 
            // OKbutton
            // 
            this.OKbutton.Location = new System.Drawing.Point(632, 455);
            this.OKbutton.Name = "OKbutton";
            this.OKbutton.Size = new System.Drawing.Size(75, 23);
            this.OKbutton.TabIndex = 6;
            this.OKbutton.Text = "OK";
            this.OKbutton.UseVisualStyleBackColor = true;
            this.OKbutton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(713, 455);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 7;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SymbolButton
            // 
            this.SymbolButton.Location = new System.Drawing.Point(261, 11);
            this.SymbolButton.Name = "SymbolButton";
            this.SymbolButton.Size = new System.Drawing.Size(103, 23);
            this.SymbolButton.TabIndex = 8;
            this.SymbolButton.Text = "Turn on Symbols";
            this.SymbolButton.UseVisualStyleBackColor = true;
            this.SymbolButton.Click += new System.EventHandler(this.SymbolButton_Click);
            // 
            // SymbolHelp
            // 
            this.SymbolHelp.Location = new System.Drawing.Point(497, 10);
            this.SymbolHelp.Name = "SymbolHelp";
            this.SymbolHelp.Size = new System.Drawing.Size(75, 23);
            this.SymbolHelp.TabIndex = 9;
            this.SymbolHelp.Text = "Symbol Help";
            this.SymbolHelp.UseVisualStyleBackColor = true;
            this.SymbolHelp.Click += new System.EventHandler(this.SymbolHelp_Click);
            // 
            // controllerType
            // 
            this.controllerType.Enabled = false;
            this.controllerType.FormattingEnabled = true;
            this.controllerType.Items.AddRange(new object[] {
            "Keyboard",
            "Xbox",
            "Nintendo",
            "Playstation"});
            this.controllerType.Location = new System.Drawing.Point(370, 12);
            this.controllerType.Name = "controllerType";
            this.controllerType.Size = new System.Drawing.Size(121, 21);
            this.controllerType.TabIndex = 10;
            this.controllerType.Text = "Controller Type";
            this.controllerType.SelectedValueChanged += new System.EventHandler(this.controllerTypeChanged);
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 481);
            this.Controls.Add(this.controllerType);
            this.Controls.Add(this.SymbolHelp);
            this.Controls.Add(this.SymbolButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKbutton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.FontChange);
            this.Controls.Add(this.FontColor);
            this.Controls.Add(this.InsertButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Editor";
            this.Text = "Editor";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button FontColor;
        private System.Windows.Forms.ColorDialog FColor;
        private System.Windows.Forms.FontDialog FFont;
        private System.Windows.Forms.Button FontChange;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.OpenFileDialog ImageInsert;
        private System.Windows.Forms.Button InsertButton;
        private System.Windows.Forms.Button OKbutton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button SymbolButton;
        private System.Windows.Forms.Button SymbolHelp;
        private System.Windows.Forms.ComboBox controllerType;
    }
}