using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Linq.Expressions;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using D3Bit;
using Gma.UserActivityMonitor;
using Newtonsoft.Json.Linq;
using Point = D3Bit.Point;

namespace D3BitGUI
{
    public partial class GUI : Form
    {
        private static string version = "1.1.0";
#if DEBUG
        private static bool debugMode = true;
#else
        private static bool debugMode = false;
#endif
        private static string debugStr = "";
        private static bool needToUpdateDebugStr = false;
        private Thread t;
        private OverlayForm _overlay;

        public GUI()
        {
            InitializeComponent();
            HookManager.KeyUp += OnKeyUp;
            t = new Thread(()=>
                               {
                                   try
                                   {
                                       string res = Util.GetPageSource("http://d3bit.com/data/json/info.json");
                                       JObject o = JObject.Parse(res);
                                       if (o["version"] != null)
                                       {
                                           if (o["version"].ToString() == version)
                                               Log("Your version of D3Bit is up-to-date.");
                                           else
                                               Log("There's a new version of D3Bit, available at http://d3bit.com/");
                                           if (o["msg"] != null && o["msg"].ToString().Length > 0)
                                               Log("{0}", o["msg"]);
                                           return;
                                       }
                                   }
                                   catch { }
                                   Log("Cannot fetch version info. Please check your connection.");
                               });
            t.Start();
            Text += " " + version;
            /*
            var s = new StopWatch();
            Bitmap bitmap = new Bitmap("x.jpg");
            var res = Screenshot.GetToolTip(bitmap);
            res.Save("z.png", ImageFormat.Png);
            Tooltip tooltip = new D3Bit.Tooltip(res);
            string name = tooltip.ParseItemName();
            string quality = "";
            string type = tooltip.ParseItemType(out quality);
            double dps = tooltip.ParseDPS();
            var affixes = tooltip.ParseAffixes();
            tooltip.Processed.Save("s.png", ImageFormat.Png);
            Debug(name);
            Debug(quality);
            Debug(type);
            Debug(dps + "");
            Debug(String.Join(", ", affixes.Select(kv => kv.Value + " " + kv.Key)));
            s.Lap("Parsing");
            /**/
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == (Keys)Enum.Parse(typeof(Keys), Properties.Settings.Default.ScanKey))
            {
                if (_overlay == null || _overlay.IsDisposed)
                {
                    var procs = Process.GetProcessesByName("Diablo III");
                    if (procs.Length > 0)
                    {
                        Bitmap bitmap = Screenshot.GetSnapShot(procs[0]);
                        bitmap.Save("yy.png", ImageFormat.Png);
                        _overlay = new OverlayForm(bitmap);
                        _overlay.Show();
                    }
                }
                else
                {
                    _overlay.Close();
                    _overlay = null;
                    e.Handled = true;
                }
            }
            else if (e.KeyCode == (Keys)Enum.Parse(typeof(Keys), Properties.Settings.Default.UploadKey) && _overlay != null && _overlay.Loaded && !_overlay.Uploading)
            {
                (new Thread(_overlay.Upload)).Start();
            }
        }

        public static void Log(string text, params object[] objs)
        {
            text = string.Format(text, objs);
            debugStr = String.Format("{1}\r\n{0}", text, debugStr).Trim();
            needToUpdateDebugStr = true;
        }

        public static void Debug(string text, params object[] objs)
        {
            if (debugMode)
                Log(text, objs);
        }

        public static void SoundFeedback(bool good)
        {
            if (good)
            {
                SoundPlayer simpleSound = new SoundPlayer("notify.wav");
                simpleSound.Play();
            }
            else
            {
                SoundPlayer simpleSound = new SoundPlayer("ringout.wav");
                simpleSound.Play();
            }
        }

        private void tUpdater_Tick(object sender, EventArgs e)
        {
            if (needToUpdateDebugStr)
            {
                rtbDebug.Text = debugStr;
                rtbDebug.SelectionStart = rtbDebug.Text.Length;
                rtbDebug.ScrollToCaret();
                needToUpdateDebugStr = false;
            }
        }

        private void GUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (t != null && t.ThreadState == System.Threading.ThreadState.Running)
                t.Abort();
        }

        private void rtbDebug_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void ucOptions1_Load(object sender, EventArgs e)
        {

        }

        

    }
}
