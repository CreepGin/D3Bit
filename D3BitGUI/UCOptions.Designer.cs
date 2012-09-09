namespace D3BitGUI
{
    partial class UCOptions
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbBattletag = new System.Windows.Forms.TextBox();
            this.cbScanKey = new System.Windows.Forms.ComboBox();
            this.tbSecret = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-1, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "D3Bit Secret:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Scan Key:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Battletag:";
            // 
            // tbBattletag
            // 
            this.tbBattletag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBattletag.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::D3BitGUI.Properties.Settings.Default, "Battletag", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbBattletag.Location = new System.Drawing.Point(69, 4);
            this.tbBattletag.Name = "tbBattletag";
            this.tbBattletag.Size = new System.Drawing.Size(117, 20);
            this.tbBattletag.TabIndex = 5;
            this.tbBattletag.Text = global::D3BitGUI.Properties.Settings.Default.Battletag;
            this.tbBattletag.TextChanged += new System.EventHandler(this.tbBattletag_TextChanged);
            // 
            // cbScanKey
            // 
            this.cbScanKey.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::D3BitGUI.Properties.Settings.Default, "ScanKey", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbScanKey.FormattingEnabled = true;
            this.cbScanKey.Items.AddRange(new object[] {
            "F1",
            "F2",
            "F3",
            "F4",
            "F5",
            "F6",
            "F7",
            "F8",
            "F9",
            "F10",
            "F11",
            "F12",
            "Insert",
            "Delete",
            "Pause",
            "PrintScreen",
            "Home",
            "End",
            "PageUp",
            "PageDown"});
            this.cbScanKey.Location = new System.Drawing.Point(69, 54);
            this.cbScanKey.Name = "cbScanKey";
            this.cbScanKey.Size = new System.Drawing.Size(116, 21);
            this.cbScanKey.TabIndex = 3;
            this.cbScanKey.Text = global::D3BitGUI.Properties.Settings.Default.ScanKey;
            this.cbScanKey.SelectedIndexChanged += new System.EventHandler(this.cbScanKey_SelectedIndexChanged);
            // 
            // tbSecret
            // 
            this.tbSecret.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSecret.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::D3BitGUI.Properties.Settings.Default, "Secret", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbSecret.Location = new System.Drawing.Point(69, 29);
            this.tbSecret.Name = "tbSecret";
            this.tbSecret.Size = new System.Drawing.Size(117, 20);
            this.tbSecret.TabIndex = 1;
            this.tbSecret.Text = global::D3BitGUI.Properties.Settings.Default.Secret;
            this.tbSecret.TextChanged += new System.EventHandler(this.tbSecret_TextChanged);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 8000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            // 
            // UCOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbBattletag);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbScanKey);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbSecret);
            this.Controls.Add(this.label1);
            this.Name = "UCOptions";
            this.Size = new System.Drawing.Size(190, 150);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSecret;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbScanKey;
        private System.Windows.Forms.TextBox tbBattletag;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
