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

        private void cbUploadKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.UploadKey = cbUploadKey.SelectedItem.ToString();
            Properties.Settings.Default.Save();
        }

        private void cbUploadTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.UploadTo = cbUploadTo.SelectedItem.ToString();
            Properties.Settings.Default.Save();
        }



    }
}
