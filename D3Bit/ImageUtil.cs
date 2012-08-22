using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Drawing;

namespace D3Bit
{
    public static class ImageUtil
    {

        public static bool ColorWithInRange(Color c, int[] ranges)
        {
            return c.R >= ranges[0] && c.R <= ranges[1] && c.G >= ranges[2] && c.G <= ranges[3] && c.B >= ranges[4] && c.B <= ranges[5];
        }

        public static List<Line> FindHorizontalLines(Bitmap bitmap, int min, int max, int[] ranges)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            List<Line> res = new List<Line>();
            int start = -1;
            for (int y = 0; y < height; y += 2)
            {
                for (int x = 0; x < width; x += 1)
                {
                    Color c = bitmap.GetPixel(x, y);
                    if (ColorWithInRange(c, ranges))
                    {
                        if (start == -1)
                            start = x;
                    }
                    else if (start != -1)
                    {
                        int w = x - start;
                        if (w > min && w < max)
                        {
                            /*
                            int s = start;
                            while (s-- != 0)
                            {
                                if (!ColorWithInRange(bitmap.GetPixel(s, y), ranges))
                                    break;
                            }
                            s++;
                            int e = x;
                            while (e-- != 0)
                            {
                                if (ColorWithInRange(bitmap.GetPixel(e, y), ranges))
                                    break;
                            }
                            res.Add(new Line(new Point(s, y), new Point(e, y)));
                            */
                            res.Add(new Line(new Point(start, y), new Point(x, y)));
                        }
                        start = -1;
                    }
                }
                start = -1;
            }
            return res;
        }

        public static List<Bound> GetTextLineBounds(Bitmap bitmap, Bound bound, int lineHeight, Func<Color, bool> colorFunc, int xScanStart, int xScanEnd)
        {
            List<Bound> res = new List<Bound>();
            for (int y = bound.P1.Y; y < bound.P2.Y; y++)
            {
                bool found = false;
                for (int x = xScanStart; x < xScanEnd; x += 1)
                {
                    Color c = bitmap.GetPixel(x, y);
                    if (colorFunc(c))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found && res.Count > 0 && y - res[res.Count - 1].P2.Y > lineHeight * 2)
                    break;
                if (!found)
                    continue;
                res.Add(GetBlockBounding(bitmap, new Bound(new Point(bound.P1.X, y - 2), new Point(bound.P2.X, y + lineHeight)), colorFunc));
                y += lineHeight;
            }
            return res;
        } 

        public static int GetGrayValue(Color color)
        {
            return Math.Max(color.R, Math.Max(color.G, color.B));
        }

        public static Bound GetBlockBounding(Bitmap bitmap, Bound bound)
        {
            return GetBlockBounding(bitmap, bound, c => GetGrayValue(c) > 180);
        }

        public static Bound GetBlockBounding(Bitmap bitmap, Bound bound, Func<Color, bool> func)
        {
            int xMin = 0;
            int yMin = 0;
            int xMax = 0;
            int yMax = 0;
            for (int x = bound.P1.X; x <= bound.P2.X; x++)
            {
                var range = Enumerable.Range(bound.P1.Y, bound.Height);
                var r = range.Where(y => func(bitmap.GetPixel(x, y)));
                if (r.Count() > 0)
                {
                    xMin = x;
                    break;
                }
            }
            for (int x = bound.P2.X; x >= bound.P1.X; x--)
            {
                var range = Enumerable.Range(bound.P1.Y, bound.Height);
                var r = range.Where(y => func(bitmap.GetPixel(x, y)));
                if (r.Count() > 0)
                {
                    xMax = x;
                    break;
                }
            }
            for (int y = bound.P1.Y; y <= bound.P2.Y; y++)
            {
                var range = Enumerable.Range(bound.P1.X, bound.Width);
                var r = range.Where(x => func(bitmap.GetPixel(x, y)));
                if (r.Count() > 0)
                {
                    yMin = y;
                    break;
                }
            }
            for (int y = bound.P2.Y; y >= bound.P1.Y; y--)
            {
                var range = Enumerable.Range(bound.P1.X, bound.Width);
                var r = range.Where(x => func(bitmap.GetPixel(x, y)));
                if (r.Count() > 0)
                {
                    yMax = y;
                    break;
                }
            }
            return new Bound(new Point(xMin, yMin), new Point(xMax, yMax));
        }

        public static void DrawBlockBounding(Bitmap bitmap, Bound bound)
        {
            DrawBlockBounding(bitmap, bound, Color.Red);
        }

        public static void DrawBlockBounding(Bitmap bitmap, Bound bound, Color color)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawRectangle(new Pen(color), bound.ToRectangle());
            }
        }

        public static Bitmap ImproveForOCR(Bitmap bitmap)
        {
            Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color c = bitmap.GetPixel(x, y);
                    int v = GetGrayValue(c);
                    if (v > 160)
                        newBitmap.SetPixel(x, y, Color.FromArgb(255 - v, 255 - v, 255 - v));
                    else
                        newBitmap.SetPixel(x, y, Color.White);
                }
            }
            return newBitmap;
        }

        public static Bitmap DpsFix(Bitmap bitmap)
        {
            Bitmap largeBitmap = new Bitmap(400, 200);
            int i = 75;
            int j = 70;
            using (Graphics g = Graphics.FromImage(largeBitmap))
            {
                g.FillRectangle(new SolidBrush(Color.White), 0, 0, largeBitmap.Width, largeBitmap.Height);
                g.DrawImage(bitmap, i, j, bitmap.Width, bitmap.Height);
            }
            return largeBitmap;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            //a holder for the result
            Bitmap result = new Bitmap(width, height);

            //use a graphics object to draw the resized image into the bitmap
            using (Graphics graphics = Graphics.FromImage(result))
            {
                //set the resize quality modes to high quality
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //draw the image into the target bitmap
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            //return the resulting bitmap
            return result;
        }

        public static Bitmap Sharpen(Bitmap image)
        {
            Bitmap sharpenImage = (Bitmap)image.Clone();

            int filterWidth = 3;
            int filterHeight = 3;
            int width = image.Width;
            int height = image.Height;

            // Create sharpening filter.
            double[,] filter = new double[filterWidth, filterHeight];
            filter[0, 0] = filter[0, 1] = filter[0, 2] = filter[1, 0] = filter[1, 2] = filter[2, 0] = filter[2, 1] = filter[2, 2] = -1;
            filter[1, 1] = 9;

            double factor = 1.0;
            double bias = 0.0;

            Color[,] result = new Color[image.Width, image.Height];

            // Lock image bits for read/write.
            BitmapData pbits = sharpenImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Declare an array to hold the bytes of the bitmap.
            int bytes = pbits.Stride * height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(pbits.Scan0, rgbValues, 0, bytes);

            int rgb;
            // Fill the color array with the new sharpened color values.
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    double red = 0.0, green = 0.0, blue = 0.0;

                    for (int filterX = 0; filterX < filterWidth; filterX++)
                    {
                        for (int filterY = 0; filterY < filterHeight; filterY++)
                        {
                            int imageX = (x - filterWidth / 2 + filterX + width) % width;
                            int imageY = (y - filterHeight / 2 + filterY + height) % height;

                            rgb = imageY * pbits.Stride + 3 * imageX;

                            red += rgbValues[rgb + 2] * filter[filterX, filterY];
                            green += rgbValues[rgb + 1] * filter[filterX, filterY];
                            blue += rgbValues[rgb + 0] * filter[filterX, filterY];
                        }
                        int r = Math.Min(Math.Max((int)(factor * red + bias), 0), 255);
                        int g = Math.Min(Math.Max((int)(factor * green + bias), 0), 255);
                        int b = Math.Min(Math.Max((int)(factor * blue + bias), 0), 255);

                        result[x, y] = Color.FromArgb(r, g, b);
                    }
                }
            }

            // Update the image with the sharpened pixels.
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    rgb = y * pbits.Stride + 3 * x;

                    rgbValues[rgb + 2] = result[x, y].R;
                    rgbValues[rgb + 1] = result[x, y].G;
                    rgbValues[rgb + 0] = result[x, y].B;
                }
            }

            // Copy the RGB values back to the bitmap.
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, pbits.Scan0, bytes);
            // Release image bits.
            sharpenImage.UnlockBits(pbits);

            return sharpenImage;
        }

    }

    public class Bound
    {
        public Point P1 { get; private set; }
        public Point P2 { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Bound(Point p1, Point p2)
        {
            P1 = p1;
            P2 = p2;
            Width = p2.X - p1.X + 1;
            Height = p2.Y - p1.Y + 1;
        }

        public bool Contains(Point point)
        {
            return point.X >= P1.X && point.Y >= P1.Y && point.X <= P2.X && point.Y <= P2.Y;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}) ({2}, {3})", P1.X, P1.Y, P2.X, P2.Y);
        }

        public override bool Equals(object obj)
        {
            var b = (Bound)obj;
            return P1.X == b.P1.X && P1.Y == b.P1.Y && P2.X == b.P2.X && P2.Y == b.P2.Y;
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle(P1.X, P1.Y, Width, Height);
        }

        public Bound Expand(int amount)
        {
            return new Bound(new Point(P1.X - amount, P1.Y - amount), new Point(P2.X + amount, P2.Y + amount));
        }
    }

    public class Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return X + ", " + Y;
        }

    }

    public class Line
    {
        public Point P1 { get; private set; }
        public Point P2 { get; private set; }
        public int XLength { get; private set; }

        public Line(Point p1, Point p2)
        {
            P1 = p1;
            P2 = p2;
            XLength = Math.Abs(p2.X - p1.X);
        }

        public override string ToString()
        {
            return P1 + " - " + P2;
        }

    }

}
