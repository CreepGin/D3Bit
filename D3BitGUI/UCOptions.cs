using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace D3BitGUI
{
    public partial class UCOptions : UserControl
    {
        public UCOptions()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
                return;
            //Tooltips
            toolTip.SetToolTip(tbBattletag, "Will be used for fetching your BNet profile in the future.");
            toolTip.SetToolTip(tbSecret, "Your account secret can be obtained after logging in on d3bit.com");
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

        private void tbBattletag_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Battletag = tbBattletag.Text;
            Properties.Settings.Default.Save();
        }



    }
}
