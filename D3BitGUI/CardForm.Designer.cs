namespace D3BitGUI
{
    partial class CardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CardForm));
            this.browser = new System.Windows.Forms.WebBrowser();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.updater = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // browser
            // 
            this.browser.Location = new System.Drawing.Point(14, 14);
            this.browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.browser.Name = "browser";
            this.browser.ScriptErrorsSuppressed = true;
            this.browser.ScrollBarsEnabled = false;
            this.browser.Size = new System.Drawing.Size(360, 460);
            this.browser.TabIndex = 0;
            this.browser.Url = new System.Uri("http://d3bit.com/c/", System.UriKind.Absolute);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(104, 232);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(180, 24);
            this.progressBar.TabIndex = 1;
            // 
            // updater
            // 
            this.updater.Enabled = true;
            this.updater.Tick += new System.EventHandler(this.updater_Tick);
            // 
            // CardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(388, 488);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.browser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Item Card";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Maroon;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CardForm_FormClosing);
            this.Load += new System.EventHandler(this.CardForm_Load);
            this.LocationChanged += new System.EventHandler(this.CardForm_LocationChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CardForm_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CardForm_MouseDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Timer updater;
        public System.Windows.Forms.WebBrowser browser;
    }
}