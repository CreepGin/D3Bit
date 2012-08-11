using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace D3Bit
{
    public static class Tesseract
    {
        public static string GetTextFromBitmap(Bitmap bitmap)
        {
            return GetTextFromBitmap(bitmap, @"nobatch tesseract\d3letters");
        }

        public static string GetTextFromBitmap(Bitmap bitmap, string extraParams)
        {
            //StopWatch sw = new StopWatch();
            bitmap.Save(@"tesseract\x.png", ImageFormat.Png);
            //sw.Lap("File Save");
            ProcessStartInfo info = new ProcessStartInfo(@"tesseract\tesseract.exe", @"tesseract\x.png tesseract\x " + extraParams);
            info.WindowStyle = ProcessWindowStyle.Hidden;
            Process p = Process.Start(info);
            p.WaitForExit();
            //sw.Lap("Tesseract");
            TextReader tr = new StreamReader(@"tesseract\x.txt");
            string res = tr.ReadToEnd();
            tr.Close();
            //File.Delete(@"tesseract\x.png");
            //File.Delete(@"tesseract\x.txt");
            return res.Trim();
        }
    }
}
