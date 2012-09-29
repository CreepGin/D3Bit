using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using D3Bit;
using Newtonsoft.Json.Linq;
using Point = System.Drawing.Point;

namespace D3BitGUI
{
    public partial class OverlayForm : Form
    {
        public bool Loaded { get; private set; }
        public bool Uploading { get; private set; }

        private const string TOOLTIP_FILEPATH = "tooltip.png";
        private const int MAX_PROGRESS = 5;
        private int _progress = 0;
        private Bitmap _bitmap;
        private Bitmap tooltipBitmap;

        private Thread _thread;

        private string name;
        private string quality;
        private string type;
        private double dps;
        private Dictionary<string, string> affixes; 
        private string stats; 

        public OverlayForm()
        {
            InitializeComponent();
            Size = new Size(Screen.PrimaryScreen.Bounds.Width, Size.Height);
            Location = new Point(0, 0);
            panel1.Location = new Point(0, 400);
            lNote.Text = String.Format("Press [{0}] to dismiss, [{1}] to Upload.",
                                       Properties.Settings.Default.ScanKey, Properties.Settings.Default.UploadKey);
            Uploading = false;
            Loaded = false;
        }

        public OverlayForm(Bitmap snapshot) : this()
        {
            _bitmap = snapshot;
            _progress = 0;
            _thread = new Thread(Process);
            _thread.Start();
        }

        public void Upload()
        {
            Uploading = true;
            this.UIThread(() => panel1.Location = new Point(0, 400));
            Label uploadingLabel = new Label();
            uploadingLabel.Font = new Font("Arial", 20);
            uploadingLabel.Text = "Uploading. . .";
            uploadingLabel.ForeColor = Color.White;
            uploadingLabel.Location = new Point(30, 35);
            uploadingLabel.Size = new Size(800, 100);
            this.UIThread(() => Controls.Add(uploadingLabel));

            byte[] data = File.ReadAllBytes(TOOLTIP_FILEPATH);

            if (Properties.Settings.Default.UploadTo == "D3Bit")
            {
                Dictionary<string, object> postParameters = new Dictionary<string, object>();
                postParameters.Add("filename", Path.GetFileName(TOOLTIP_FILEPATH));
                postParameters.Add("fileformat", Path.GetExtension(TOOLTIP_FILEPATH));
                postParameters.Add("xyz", "placeholder");
                postParameters.Add("n", name);
                postParameters.Add("q", quality);
                postParameters.Add("d", dps);
                postParameters.Add("t", type);
                postParameters.Add("a", stats);
                postParameters.Add("s", Properties.Settings.Default.Secret.Trim());
                postParameters.Add("uploadedfile", data);

                /*
                Func<string, string> u = System.Uri.EscapeDataString;
                string url = string.Format("http://d3bit.com/ajax/uploaditem/?n={0}&q={1}&d={2}&t={3}&a={4}&s={5}",
                                            u(name),
                                            u(quality), u(dps + ""), u(type),
                                            u(String.Join(", ", affixes.Select(kv => (kv.Value + " " + kv.Key).Trim()))),
                                            u(Properties.Settings.Default.Secret));
                string json = Util.GetPageSource(url);
                */

                // Create request and receive response
                string postURL = "http://d3bit.com/ajax/uploaditem/";
                string userAgent =
                    "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.3) Gecko/20100401 Firefox/3.6.3";
                HttpWebResponse webResponse = Util.MultipartFormDataPost(postURL, userAgent, postParameters);
                StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                string res = responseReader.ReadToEnd();
                webResponse.Close();


                JObject o = JObject.Parse(res);
                if (o["status"] != null && o["status"].ToString() == "success" && o["link"] != null)
                {
                    this.UIThread(() => Clipboard.SetText(o["link"].ToString()));
                    this.UIThread(
                        () => uploadingLabel.Text = string.Format("Success! Link {0} copied to Clipboard.", o["link"]));
                    GUI.Log("Uploaded. {0}", o["link"]);
                }
                else if (o["msg"] != null)
                {
                    this.UIThread(() => uploadingLabel.Text = o["msg"].ToString());
                }
                else
                {
                    this.UIThread(() => uploadingLabel.Text = "Error Uploading.");
                }
            }
            else if (Properties.Settings.Default.UploadTo == "Imgur")
            {
                Dictionary<string, object> postParameters = new Dictionary<string, object>();
                postParameters.Add("filename", Path.GetFileName(TOOLTIP_FILEPATH));
                postParameters.Add("fileformat", Path.GetExtension(TOOLTIP_FILEPATH));
                postParameters.Add("key", "4c379d346aaf18a942734de377c4cda0");
                postParameters.Add("title", "Uploaded from D3Bit.com");
                postParameters.Add("caption", string.Format("Name: {4}, Quality: {0} Type: {1} DPS/Armor: {2} Stats: {3}", quality, type, dps, stats, name));
                postParameters.Add("image", data);

                string postURL = "http://api.imgur.com/2/upload.json";
                string userAgent =
                    "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.3) Gecko/20100401 Firefox/3.6.3";
                HttpWebResponse webResponse = Util.MultipartFormDataPost(postURL, userAgent, postParameters);
                StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                string res = responseReader.ReadToEnd();
                webResponse.Close();

                JObject o = JObject.Parse(res);
                if (o["upload"] != null && o["upload"]["links"] != null && o["upload"]["links"]["imgur_page"] != null)
                {
                    string link = o["upload"]["links"]["imgur_page"].ToString();
                    this.UIThread(() => Clipboard.SetText(link));
                    this.UIThread(
                        () => uploadingLabel.Text = string.Format("Success! Link {0} copied to Clipboard.", link));
                    GUI.Log("Uploaded. {0}", link);
                }
                else
                {
                    this.UIThread(() => uploadingLabel.Text = "Error Uploading.");
                }
            }

            Thread.Sleep(5000);
            this.UIThread(() => Controls.Remove(uploadingLabel));
            this.UIThread(() => panel1.Location = new Point(2, 1));
            Uploading = false;
        }

        void Process()
        {
            tooltipBitmap = Screenshot.GetToolTip(_bitmap);
            if (tooltipBitmap == null)
            {
                Abort();
                return;
            }
            try
            {
                /*
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                myEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 70L);
                tooltipBitmap.Save(TOOLTIP_FILEPATH, ImageCodecInfo.GetImageEncoders().Where(c => c.FormatID == ImageFormat.Jpeg.Guid).First(), myEncoderParameters);
                */
                tooltipBitmap.Save(TOOLTIP_FILEPATH, ImageFormat.Png);
                Tooltip tooltip = new Tooltip(tooltipBitmap);
                _progress++;
                name = tooltip.ParseItemName();
                _progress++;
                quality = "";
                type = tooltip.ParseItemType(out quality);
                _progress++;
                dps = tooltip.ParseDPS();
                _progress++;
                string socketBonuses = "";
                affixes = tooltip.ParseAffixes(out socketBonuses);
                stats = String.Join(", ", affixes.Select(kv => (kv.Value + " " + kv.Key).Trim()));
                _progress++;
                this.UIThread(() => panel2.Location = new Point(0, 400));
                this.UIThread(() => panel1.Location = new Point(2, 1));
                tooltip.Processed.Save("s.png", ImageFormat.Png);
                
                this.UIThread(() => tbName.Text = name);
                this.UIThread(() => cbQuality.Text = quality);
                this.UIThread(() => cbType.Text = type);
                this.UIThread(() => tbDps.Text = dps+"");
                this.UIThread(() => tbStats.Text = stats);

                //Max Stats
                ShowMaxStats(type, affixes);

                GUI.SoundFeedback(true);
                Loaded = true;
            }catch
            {
                Abort();
                return;
            }
        }
        void ShowMaxStats(string type, Dictionary<string, string> affixes)
        {
            var oldProgresses = panel1.Controls.OfType<UCStatProgress>();
            while (oldProgresses.Count() > 0)
                this.UIThread(() => panel1.Controls.Remove(oldProgresses.First()));
            string generalType = type;
            if (D3Bit.Data.WeaponTypes.Contains(type))
                generalType = "Weapon";
            if (D3Bit.Data.OffHandTypes.Contains(type))
                generalType = "Off-Hand";
            int stars = 0;
            if (Data.MaxStats.Where(t => t.EquipmentType == generalType).Count() > 0)
            {
                Dictionary<StatTriplet, double> processedStats = new Dictionary<StatTriplet, double>();
                int c = 0;
                foreach (var stat in affixes)
                {
                    string statName = stat.Key;
                    string value = stat.Value;
                    double statValue = 0;
                    double maxValue = 0;
                    if (double.TryParse(value, out statValue))
                    {
                        var triplet =
                                Data.MaxStats.Where(
                                    t => t.EquipmentType == generalType && t.StatName.ToLower() == statName.ToLower()).
                                    FirstOrDefault();
                        if (triplet != default(StatTriplet))
                        {
                            maxValue = triplet.StatValue;
                            processedStats.Add(triplet, statValue);
                        }
                    }
                    UCStatProgress progressControl = new UCStatProgress(statName, statValue, maxValue);
                    progressControl.Location = new Point(320 + (c / 4) * 240, (c % 4) * 18 + 6);
                    this.UIThread(() => panel1.Controls.Add(progressControl));
                    c++;
                }
                stars = GetStatStars(processedStats, generalType);
            }
            this.UIThread(() => ucStars1.Value = stars);
        }

        int GetStatStars(Dictionary<StatTriplet, double> processedStats, string generalType)
        {
            double total = 0;
            Func<string, double> v = o => processedStats.Where(s => s.Key.StatName.ToLower() == o).Select(s => s.Value).FirstOrDefault();
            Func<string, double> m = o => processedStats.Where(s => s.Key.StatName.ToLower() == o).Select(s => s.Key.StatValue).FirstOrDefault();
            Func<double, double, double> d = (d1, d2) => d2 == 0 ? 0 : d1/d2;

            double n = 0;
            if (v("str") > 0 && v("str") >= v("dex") && v("str") >= v("int"))
                n =d(v("str") + v("vit"), m("str") + m("vit"));
            else if (v("dex") > 0 && v("dex") >= v("str") && v("dex") >= v("int"))
                n = d(v("dex") + v("vit"), m("dex") + m("vit"));
            else
                n = d(v("int") + v("vit"), m("int") + m("vit"));
            if (v("vit") < 1)
                n = n/2.0;
            n = 6.4 * n * n;
            total += n;
            foreach (var weight in Data.starWeights)
            {
                if (v(weight.Key) > 0)
                {
                    n = d(v(weight.Key), m(weight.Key));
                    total += n*n*weight.Value;
                }
            }

            if (generalType == "Weapon" && dps > 0)
            {
                n = 0;
                if (type.ToLower().Contains("two") || type == "Daibo" || type == "Dow" || type == "Crossbow" || type == "Polearm" || type == "Staff")
                    n = dps/1800;
                else
                    n = dps / 1400;
                if (n > 1)
                    n = 1;
                if (n > 0.60)
                    total += n * n * 6;
            }
            

            return (int)Math.Min(10, total);
        }

        void Abort()
        {
            GUI.SoundFeedback(false);
            _progress = -1;
            this.UIThread(() => panel1.Location = new Point(0, 400));
            this.UIThread(() => panel2.Location = new Point(0, 400));
            Font f = new Font("Arial", 30);
            Label label = new Label();
            label.Font = f;
            label.Text = "Error Scanning.";
            label.ForeColor = Color.White;
            label.Location = new Point(30, 30);
            label.Size = new Size(400, 100);
            this.UIThread(() => Controls.Add(label));
        }

        private void OverlayForm_Load(object sender, EventArgs e)
        {
            
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            
            base.OnPaint(pevent);
        }

        private void OverlayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_thread!= null && _thread.ThreadState == ThreadState.Running)
                _thread.Abort();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_progress == -1)
                return;
            if (_progress < MAX_PROGRESS)
            {
                progressBar1.Value = (int)Math.Ceiling(_progress/(double) MAX_PROGRESS*100);
            }
        }

        private void cbQuality_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbQuality.SelectedItem == null)
                return;
            quality = cbQuality.SelectedItem.ToString();
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbType.SelectedItem == null)
                return;
            type = cbType.SelectedItem.ToString();
            ShowMaxStats(type, affixes);
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            name = tbName.Text;
        }

        private void tbDps_TextChanged(object sender, EventArgs e)
        {
            double tmp;
            if (double.TryParse(tbDps.Text, out tmp))
                dps = tmp;
            ShowMaxStats(type, affixes);
        }

        private void tbStats_TextChanged(object sender, EventArgs e)
        {
            stats = tbStats.Text.Trim();
            var exploded = stats.Replace(", ", ",").Split(new char[] {','});
            affixes.Clear();
            foreach (var s in exploded)
            {
                string trimmed = s.Trim();
                if (!trimmed.Contains(" "))
                {
                    affixes.Add(trimmed, "");
                    continue;
                }
                var parts = trimmed.Split(new char[] {' '});
                if (!affixes.ContainsKey(parts[1]))
                    affixes.Add(parts[1], parts[0]);
            }
            ShowMaxStats(type, affixes);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
