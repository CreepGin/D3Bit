namespace D3BitGUI
{
    partial class UCBatch
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.bOpen = new System.Windows.Forms.Button();
            this.lSelectedFiles = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.cbFormat = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "JPEG|*.jpg|PNG|*.png|All Files|*.*";
            this.openFileDialog1.Multiselect = true;
            // 
            // bOpen
            // 
            this.bOpen.Location = new System.Drawing.Point(3, 3);
            this.bOpen.Name = "bOpen";
            this.bOpen.Size = new System.Drawing.Size(75, 23);
            this.bOpen.TabIndex = 0;
            this.bOpen.Text = "Open File(s)";
            this.bOpen.UseVisualStyleBackColor = true;
            this.bOpen.Click += new System.EventHandler(this.bOpen_Click);
            // 
            // lSelectedFiles
            // 
            this.lSelectedFiles.AutoSize = true;
            this.lSelectedFiles.Location = new System.Drawing.Point(82, 8);
            this.lSelectedFiles.Name = "lSelectedFiles";
            this.lSelectedFiles.Size = new System.Drawing.Size(85, 13);
            this.lSelectedFiles.TabIndex = 1;
            this.lSelectedFiles.Text = "No files selected";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(3, 60);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(169, 18);
            this.progressBar1.TabIndex = 6;
            // 
            // cbFormat
            // 
            this.cbFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFormat.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::D3BitGUI.Properties.Settings.Default, "ExportFormat", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbFormat.FormattingEnabled = true;
            this.cbFormat.Items.AddRange(new object[] {
            "JSON",
            "XML",
            "CSV"});
            this.cbFormat.Location = new System.Drawing.Point(48, 32);
            this.cbFormat.Name = "cbFormat";
            this.cbFormat.Size = new System.Drawing.Size(57, 21);
            this.cbFormat.TabIndex = 7;
            this.cbFormat.Text = global::D3BitGUI.Properties.Settings.Default.ExportFormat;
            this.cbFormat.SelectedIndexChanged += new System.EventHandler(this.cbFormat_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Format:";
            // 
            // UCBatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbFormat);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lSelectedFiles);
            this.Controls.Add(this.bOpen);
            this.Name = "UCBatch";
            this.Size = new System.Drawing.Size(175, 86);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button bOpen;
        private System.Windows.Forms.Label lSelectedFiles;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ComboBox cbFormat;
        private System.Windows.Forms.Label label1;
    }
}
