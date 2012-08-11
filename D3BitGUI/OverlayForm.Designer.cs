namespace D3BitGUI
{
    partial class OverlayForm
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.cbQuality = new System.Windows.Forms.ComboBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbDps = new System.Windows.Forms.TextBox();
            this.tbStats = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lNote = new System.Windows.Forms.Label();
            this.ucStars1 = new D3BitGUI.UCStars();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(15, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(15, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Quality:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(15, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Type:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(15, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Stats:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbType);
            this.panel1.Controls.Add(this.cbQuality);
            this.panel1.Controls.Add(this.tbName);
            this.panel1.Controls.Add(this.tbDps);
            this.panel1.Controls.Add(this.tbStats);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(2, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(797, 100);
            this.panel1.TabIndex = 8;
            // 
            // cbType
            // 
            this.cbType.BackColor = System.Drawing.Color.Black;
            this.cbType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbType.ForeColor = System.Drawing.Color.White;
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "Axe",
            "Ceremonial Knife",
            "Hand Crossbow",
            "Dagger",
            "Fist Weapon",
            "Mace",
            "Mighty Weapon",
            "Spear",
            "Sword",
            "Wand",
            "Two-Handed Axe",
            "Bow",
            "Daibo",
            "Crossbow",
            "Two-Handed Mace",
            "Two-Handed Mighty Weapon",
            "Polearm",
            "Staff",
            "Two-Handed Sword",
            "Mojo",
            "Source",
            "Quiver",
            "Shield",
            "Ring",
            "Amulet",
            "Shoulders",
            "Helm",
            "Pants",
            "Gloves",
            "Chest Armor",
            "Bracers",
            "Boots",
            "Belt",
            "Cloak",
            "Mighty Belt",
            "Spirit Stone",
            "Voodoo Mask",
            "Wizard Hat"});
            this.cbType.Location = new System.Drawing.Point(90, 44);
            this.cbType.Margin = new System.Windows.Forms.Padding(0);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(121, 21);
            this.cbType.TabIndex = 14;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // cbQuality
            // 
            this.cbQuality.BackColor = System.Drawing.Color.Black;
            this.cbQuality.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbQuality.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbQuality.ForeColor = System.Drawing.Color.White;
            this.cbQuality.FormattingEnabled = true;
            this.cbQuality.Items.AddRange(new object[] {
            "Unknown",
            "Magic",
            "Rare",
            "Legendary",
            "Set"});
            this.cbQuality.Location = new System.Drawing.Point(90, 21);
            this.cbQuality.Margin = new System.Windows.Forms.Padding(0);
            this.cbQuality.Name = "cbQuality";
            this.cbQuality.Size = new System.Drawing.Size(121, 21);
            this.cbQuality.TabIndex = 13;
            this.cbQuality.SelectedIndexChanged += new System.EventHandler(this.cbQuality_SelectedIndexChanged);
            // 
            // tbName
            // 
            this.tbName.BackColor = System.Drawing.Color.Black;
            this.tbName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbName.ForeColor = System.Drawing.Color.White;
            this.tbName.Location = new System.Drawing.Point(92, 4);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(205, 13);
            this.tbName.TabIndex = 12;
            this.tbName.Text = "Hello";
            this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
            // 
            // tbDps
            // 
            this.tbDps.BackColor = System.Drawing.Color.Black;
            this.tbDps.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbDps.ForeColor = System.Drawing.Color.White;
            this.tbDps.Location = new System.Drawing.Point(92, 69);
            this.tbDps.Name = "tbDps";
            this.tbDps.Size = new System.Drawing.Size(66, 13);
            this.tbDps.TabIndex = 11;
            this.tbDps.Text = "Hello";
            this.tbDps.TextChanged += new System.EventHandler(this.tbDps_TextChanged);
            // 
            // tbStats
            // 
            this.tbStats.BackColor = System.Drawing.Color.Black;
            this.tbStats.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbStats.ForeColor = System.Drawing.Color.White;
            this.tbStats.Location = new System.Drawing.Point(92, 85);
            this.tbStats.Name = "tbStats";
            this.tbStats.Size = new System.Drawing.Size(434, 13);
            this.tbStats.TabIndex = 10;
            this.tbStats.Text = "Hello";
            this.tbStats.TextChanged += new System.EventHandler(this.tbStats_TextChanged);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(-14, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 16);
            this.label6.TabIndex = 8;
            this.label6.Text = "DPS/Armor:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(191, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "Analyzing. . .";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.progressBar1.Location = new System.Drawing.Point(37, 38);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(148, 23);
            this.progressBar1.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel2.Controls.Add(this.progressBar1);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Location = new System.Drawing.Point(805, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(331, 100);
            this.panel2.TabIndex = 10;
            // 
            // lNote
            // 
            this.lNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lNote.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lNote.ForeColor = System.Drawing.Color.White;
            this.lNote.Location = new System.Drawing.Point(1539, 82);
            this.lNote.Name = "lNote";
            this.lNote.Size = new System.Drawing.Size(394, 16);
            this.lNote.TabIndex = 10;
            this.lNote.Text = "Note";
            this.lNote.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // ucStars1
            // 
            this.ucStars1.BackColor = System.Drawing.Color.Black;
            this.ucStars1.Location = new System.Drawing.Point(1575, 3);
            this.ucStars1.Name = "ucStars1";
            this.ucStars1.Size = new System.Drawing.Size(355, 71);
            this.ucStars1.TabIndex = 11;
            this.ucStars1.Value = 0;
            // 
            // OverlayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1932, 102);
            this.Controls.Add(this.ucStars1);
            this.Controls.Add(this.lNote);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OverlayForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "OverlayForm";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OverlayForm_FormClosing);
            this.Load += new System.EventHandler(this.OverlayForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lNote;
        private System.Windows.Forms.TextBox tbStats;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TextBox tbDps;
        private System.Windows.Forms.ComboBox cbQuality;
        private System.Windows.Forms.ComboBox cbType;
        private UCStars ucStars1;




    }
}