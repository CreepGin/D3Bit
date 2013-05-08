using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using D3Bit;
using Newtonsoft.Json.Linq;

namespace D3BitGUI
{
    public partial class UCOptions : UserControl
    {
        private Thread t;
        private Dictionary<string, string> _builds = new Dictionary<string, string>();

        public UCOptions()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
                return;
            //Tooltips
            toolTip.SetToolTip(tbLanguage, "Localization code. Please refer to the online User Guide for setting up for languages other than English.");
            toolTip.SetToolTip(tbSecret, "Your account secret can be obtained after logging in on d3bit.com");
            toolTip.SetToolTip(bReloadBuilds, "Re-load your builds from D3Up");

            Tesseract.language_code = Properties.Settings.Default.ScanLanguage;
            Tesseract.language_code = "eng";

            t = new Thread(ReloadBuilds);
            t.Start();
        }

        void ReloadBuilds()
        {
            try
            {
                Func<string, string> u = System.Uri.EscapeDataString;
                string res = Util.GetPageSource("http://d3up.com/ajax/builds?username=" + u(Properties.Settings.Default.D3UpUsername));
                JObject o = JObject.Parse(res);
                if (o["builds"] != null)
                {
                    var builds = o["builds"];
                    _builds = builds.ToObject<Dictionary<string, string>>();
                    var buildNames = _builds.Select(b => b.Value).ToArray();
                    this.UIThread(() =>
                    {
                        cbDefaultBuild.Items.Clear();
                        cbDefaultBuild.Items.AddRange(buildNames);
                    });
                    GUI.Log(_builds.Count + " Builds loaded from D3Up.com");
                }
                else
                {
                    throw new Exception("No builds");
                }
            }
            catch (Exception ex)
            {
                GUI.Log("Cannot fetch default build. Please check your d3up username.");
            }
        }

        private void tbSecret_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Secret = tbSecret.Text;
            Properties.Settings.Default.Save();
        }

        private void cbScanKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ScanKey = cbScanKey.SelectedItem.ToString();
            Properties.Settings.Default.Save();
        }

        private void tbD3UpUsername_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.D3UpUsername = tbD3UpUsername.Text;
            Properties.Settings.Default.Save();
        }

        private void cbDefaultBuild_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.D3UpDefaultBuild = cbDefaultBuild.SelectedItem.ToString();
            Properties.Settings.Default.Save();
            Properties.Settings.Default.D3UpDefaultBuildNumber = int.Parse(_builds.ToArray()[cbDefaultBuild.SelectedIndex].Key);
        }

        private void bReloadBuilds_Click(object sender, EventArgs e)
        {
            t = new Thread(ReloadBuilds);
            t.Start();
        }

        private void tbLanguage_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ScanLanguage = tbLanguage.Text;
            Properties.Settings.Default.Save();
        }

        private void cbReverseQuality_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ReverseQuality = cbReverseQuality.Checked;
            Properties.Settings.Default.Save();
        }



    }
}
