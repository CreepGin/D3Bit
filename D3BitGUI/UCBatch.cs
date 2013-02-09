using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using D3Bit;

namespace D3BitGUI
{
    public partial class UCBatch : UserControl
    {
        public UCBatch()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
                return;
        }

        private void bOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var files = openFileDialog1.FileNames;
                if (files.Length == 0)
                {
                    lSelectedFiles.Text = "No files selected.";
                }else
                {
                    lSelectedFiles.Text = String.Format("{0} files selected", files.Length);
                    (new Thread(Process)).Start();
                }
            }
        }

        void Process()
        {
            int numProcessed = 0;
            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
            foreach (var filename in openFileDialog1.FileNames)
            {
                try
                {
                    Bitmap bitmap = Bitmap.FromFile(filename) as Bitmap;
                    var res = Screenshot.GetToolTip(bitmap);
                    Tooltip tooltip = new D3Bit.Tooltip(res);
                    string name = tooltip.ParseItemName();
                    string quality = "";
                    string type = tooltip.ParseItemType(out quality, Properties.Settings.Default.ReverseQuality);
                    double dps = tooltip.ParseDPS();
                    string socketBonuses = "";
                    var affixes = tooltip.ParseAffixes(out socketBonuses);
                    if (name.Length > 0 && quality.Length > 0 && type.Length > 0 && affixes.Keys.Count > 0)
                    {
                        Dictionary<string, string> itemDic = new Dictionary<string, string>();
                        itemDic.Add("Name", name);
                        itemDic.Add("Quality", quality);
                        itemDic.Add("Type", type);
                        itemDic.Add("DPS", dps + "");
                        itemDic.Add("Stats", String.Join(", ", affixes.Select(kv => (kv.Value + " " + kv.Key).Trim())));
                        data.Add(itemDic);
                    }
                }
                catch (Exception ex)
                {

                }
                numProcessed++;
                this.UIThread(() => this.progressBar1.Value = (int)Math.Ceiling(numProcessed / (double)openFileDialog1.FileNames.Length * 100));
            }
            Exporter.Export(data, "export." + Properties.Settings.Default.ExportFormat.ToLower(), Properties.Settings.Default.ExportFormat);
            GUI.Log("Parsed {0} out of {1}. Saved to \"export.{2}\"", data.Count, openFileDialog1.FileNames.Length, Properties.Settings.Default.ExportFormat.ToLower());
        }

        private void cbFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFormat.SelectedItem == null)
                return;
            Properties.Settings.Default.ExportFormat = cbFormat.SelectedItem.ToString();
            Properties.Settings.Default.Save();
        }
    }
}
