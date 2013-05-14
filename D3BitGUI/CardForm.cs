using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using D3Bit;
using Point = System.Drawing.Point;
using ThreadState = System.Threading.ThreadState;

namespace D3BitGUI
{
    public partial class CardForm : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private const int CS_DROPSHADOW = 0x00020000;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern System.IntPtr CreateRoundRectRgn
        (
         int nLeftRect, // x-coordinate of upper-left corner
         int nTopRect, // y-coordinate of upper-left corner
         int nRightRect, // x-coordinate of lower-right corner
         int nBottomRect, // y-coordinate of lower-right corner
         int nWidthEllipse, // height of ellipse
         int nHeightEllipse // width of ellipse
        );
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        private static extern bool DeleteObject(System.IntPtr hObject);

        public string TooltipPath { get; private set; }

        private int MAX_PROGRESS_STEPS = 6;

        private Bitmap _bitmap;
        private Bitmap _tooltipBitmap;
        private int _progressStep = 0;
        private Thread _thread;
        private bool _needToClose = false;

        private Dictionary<string, string> _info;
        private Dictionary<string, string> _affixes;

        public CardForm()
        {
            InitializeComponent();
            Location = Properties.Settings.Default.ItemCardStartPosition;
            this.Cursor = Cursors.SizeAll;

            browser.ObjectForScripting = new ScriptInterface(this);
            string bgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bg.jpg");
            browser.DocumentText = "<body style=\"background:url(" + bgPath + ")\"></body>";
            _info = new Dictionary<string, string>();
            _info.Add("name", "");
            _info.Add("quality", "");
            _info.Add("type", "");
            _info.Add("meta", "");
            _info.Add("dps", "");
            _info.Add("stats", "");
        }

        public CardForm(Bitmap snapshot)
            : this()
        {
            _bitmap = snapshot;
            _progressStep = 0;
            _thread = new Thread(Process);
            _thread.Start();
        }

        void Process()
        {
            _tooltipBitmap = Screenshot.GetToolTip(_bitmap);
            if (_tooltipBitmap == null)
            {
                this.UIThread(Abort);
                return;
            }
            try
            {
                TooltipPath = string.Format("tmp/{0}.png", DateTime.Now.Ticks);
                _tooltipBitmap.Save(TooltipPath, ImageFormat.Png);
                Tooltip tooltip = new Tooltip(_tooltipBitmap);
                _progressStep++;
                _info["name"] = tooltip.ParseItemName();
                _progressStep++;
                string quality = "Unknown";
                _info["type"] = tooltip.ParseItemType(out quality, Properties.Settings.Default.ReverseQuality);
                _info["quality"] = quality;
                _progressStep++;
                _info["meta"] = tooltip.ParseMeta();
                _progressStep++;
                _info["dps"] = tooltip.ParseDPS()+"";
                _progressStep++;
                string socketBonuses = "";
                _affixes = tooltip.ParseAffixes(out socketBonuses);
                if (socketBonuses != "")
                    _info["meta"] += _info["meta"] == "" ? socketBonuses : "," + socketBonuses;
                _info["stats"] = String.Join(", ", _affixes.Select(kv => (kv.Value + " " + kv.Key).Trim()));
                _progressStep++;
                tooltip.Processed.Save("s.png", ImageFormat.Png);
                this.UIThread(() => progressBar.Location = new Point(800, 800));

                Func<string, string> u = System.Uri.EscapeDataString;
                string url = String.Format("http://d3bit.com/c/?image={0}&battletag={1}&build={2}&auctionrName={3}&secret={4}&{5}&test=1",
                                           u(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TooltipPath)),
                                           u(Properties.Settings.Default.Battletag), Properties.Settings.Default.D3UpDefaultBuildNumber,
                                           u(Properties.Settings.Default.AuctionrName),
                                           u(Properties.Settings.Default.Secret.Trim()), Util.FormGetString(_info));
                browser.Url = new Uri(url);
                //GUI.Log(url);

                GUI.SoundFeedback(true);
                this.UIThread(BringToFront);
                //this.UIThread(() => TopMost = false);
            }
            catch (Exception ex)
            {
                GUI.Log(ex.Message);
                GUI.Log(ex.StackTrace);
                this.UIThread(Abort);
                return;
            }
        }

        void Abort()
        {
            GUI.SoundFeedback(false);
            GUI.Log("Error Scanning...");
            Close();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape) this.UIThread(() => _needToClose = true);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void CardForm_Paint(object sender, PaintEventArgs e)
        {
            System.IntPtr ptr = CreateRoundRectRgn(0, 0, this.Width, this.Height, 14, 14);
            this.Region = System.Drawing.Region.FromHrgn(ptr);
            DeleteObject(ptr);
            var hb = new HatchBrush(HatchStyle.Percent50, this.TransparencyKey);
            e.Graphics.FillRectangle(hb, this.DisplayRectangle);
        }

        private void CardForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void CardForm_Load(object sender, EventArgs e)
        {
            
        }

        private void CardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_thread != null && _thread.ThreadState == ThreadState.Running)
                _thread.Abort();
        }

        private void updater_Tick(object sender, EventArgs e)
        {
            if (_progressStep < MAX_PROGRESS_STEPS)
            {
                progressBar.Value = (int)Math.Ceiling(_progressStep / (double)MAX_PROGRESS_STEPS * 100);
            }
            if (_needToClose)
                Close();
        }

        private void CardForm_LocationChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ItemCardStartPosition = Location;
            Properties.Settings.Default.Save();
        }
    }
}
