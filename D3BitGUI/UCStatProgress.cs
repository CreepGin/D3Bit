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
    public partial class UCStatProgress : UserControl
    {
        public UCStatProgress(string statName, double statValue, double maxValue)
        {
            InitializeComponent();
            label1.Text = statName;
            label2.Text = string.Format("{0}/{1}", statValue > 0 ? (object) statValue : "- -", maxValue > 0 ? (object) maxValue : "- -");
            if (maxValue > 0)
                progressBar1.Value = Math.Min((int) Math.Round(statValue/maxValue*100), 100);
        }
    }
}
