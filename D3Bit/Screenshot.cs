using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace D3Bit
{
    public static class Screenshot
    {
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            private int _Left;
            private int _Top;
            private int _Right;
            private int _Bottom;

            public RECT(RECT Rectangle)
                : this(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom)
            {
            }
            public RECT(int Left, int Top, int Right, int Bottom)
            {
                _Left = Left;
                _Top = Top;
                _Right = Right;
                _Bottom = Bottom;
            }

            public int X
            {
                get { return _Left; }
                set { _Left = value; }
            }
            public int Y
            {
                get { return _Top; }
                set { _Top = value; }
            }
            public int Left
            {
                get { return _Left; }
                set { _Left = value; }
            }
            public int Top
            {
                get { return _Top; }
                set { _Top = value; }
            }
            public int Right
            {
                get { return _Right; }
                set { _Right = value; }
            }
            public int Bottom
            {
                get { return _Bottom; }
                set { _Bottom = value; }
            }
            public int Height
            {
                get { return _Bottom - _Top; }
                set { _Bottom = value + _Top; }
            }
            public int Width
            {
                get { return _Right - _Left; }
                set { _Right = value + _Left; }
            }
            public Point Location
            {
                get { return new Point(Left, Top); }
                set
                {
                    _Left = value.X;
                    _Top = value.Y;
                }
            }
            public Size Size
            {
                get { return new Size(Width, Height); }
                set
                {
                    _Right = value.Width + _Left;
                    _Bottom = value.Height + _Top;
                }
            }

            public static implicit operator Rectangle(RECT Rectangle)
            {
                return new Rectangle(Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height);
            }
            public static implicit operator RECT(Rectangle Rectangle)
            {
                return new RECT(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom);
            }
            public static bool operator ==(RECT Rectangle1, RECT Rectangle2)
            {
                return Rectangle1.Equals(Rectangle2);
            }
            public static bool operator !=(RECT Rectangle1, RECT Rectangle2)
            {
                return !Rectangle1.Equals(Rectangle2);
            }

            public override string ToString()
            {
                return "{Left: " + _Left + "; " + "Top: " + _Top + "; Right: " + _Right + "; Bottom: " + _Bottom + "}";
            }

            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }

            public bool Equals(RECT Rectangle)
            {
                return Rectangle.Left == _Left && Rectangle.Top == _Top && Rectangle.Right == _Right && Rectangle.Bottom == _Bottom;
            }

            public override bool Equals(object Object)
            {
                if (Object is RECT)
                {
                    return Equals((RECT)Object);
                }
                else if (Object is Rectangle)
                {
                    return Equals(new RECT((Rectangle)Object));
                }

                return false;
            }
        }

        public static Bitmap GetSnapShot(Process d3Proc)
        {
            if (d3Proc != null)
            {
                RECT rc;
                GetWindowRect(d3Proc.MainWindowHandle, out rc);

                Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format24bppRgb);
                Graphics gfxBmp = Graphics.FromImage(bmp);
                //IntPtr hdcBitmap = gfxBmp.GetHdc();

                //PrintWindow(d3Proc.MainWindowHandle, hdcBitmap, 0);
                gfxBmp.CopyFromScreen(rc.X, rc.Y, 0, 0, new Size(rc.Width, rc.Height), CopyPixelOperation.SourceCopy);


                //gfxBmp.ReleaseHdc(hdcBitmap);
                gfxBmp.Dispose();

                return bmp;
            }
            return null;
        }

        public static Bitmap GetToolTip(Bitmap bitmap)
        {
            var lines = ImageUtil.FindHorizontalLines(bitmap, 260, 650, new int[] { 0, 10, 0, 10, 0, 10 });
            lines = lines.OrderBy(l => l.P1.X).ToList();
            var groups =
                lines.GroupBy(l => l.P1.X).Where(
                    l =>
                    l.Last().P1.Y - l.First().P1.Y > 200 &&
                    l.Count() > 4 &&
                    l.Count() == l.Where(i => Math.Abs(i.XLength - l.First().XLength) < i.XLength*0.1).Count()).OrderByDescending(
                        l => l.First().XLength).ThenByDescending(l => l.Count());
            int x = groups.Count();
            if (groups.Count() > 0)
            {
                lines = groups.ElementAt(0).ToList();
                //Count line clusters
                int clusterCount = 0;
                int lastY = lines.ElementAt(0).P1.Y;
                foreach (var line in lines)
                {
                    if (line.P1.Y - lastY > 5)
                        clusterCount++;
                    lastY = line.P1.Y;
                }

                var min = new Point(bitmap.Width, bitmap.Height);
                var max = new Point(0, 0);
                foreach (var line in groups.ElementAt(0))
                {
                    if (line.P1.X <= min.X && line.P1.Y <= min.Y)
                        min = line.P1;
                    else if (line.P2.X >= max.X && line.P2.Y >= max.Y)
                        max = line.P2;
                }
                Bound bound = new Bound(min, max);
                if (clusterCount==2)
                    bound = new Bound(new Point(min.X, min.Y - (int)Math.Round((42/410.0)*(max.X-min.X))), max);
                return bitmap.Clone(bound.ToRectangle(), bitmap.PixelFormat);
            }
            return null;
        }
    }
}
