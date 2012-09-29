using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace D3Bit
{
    public class Tooltip
    {
        public Bitmap Original { get; private set; }
        public Bitmap Resized { get; private set; }
        public Bitmap Processed { get; private set; }

        private int _affixStartY;

        public Tooltip(Bitmap bitmap)
        {
            Original = bitmap;
            Resized = ImageUtil.ResizeImage(bitmap, 400, (int)(400.0 / bitmap.Width * bitmap.Height));
            Processed = (Bitmap)Resized.Clone();   //Modifiable, used for improvement and drawing
            _affixStartY = s(88/320.0);
        }

        //Convinience scaling function
        int s(double scale)
        {
            return (int)Math.Round(scale * Resized.Width);
        }

        public string ParseItemName()
        {
            string itemName = "Unknown";
            Bound itemNamebound = ImageUtil.GetBlockBounding(Resized,
                                                                 new Bound(new Point(s(15/410.0), s(15/410.0)), new Point(s(395/410.0), s(52/410.0)))).Expand(4);
            if (itemNamebound.Width < 20)
                return itemName;
            ImageUtil.DrawBlockBounding(Processed, itemNamebound);
            Bitmap itemNameBlock = Resized.Clone(itemNamebound.ToRectangle(), Processed.PixelFormat);
            //itemNameBlock = ImageUtil.ResizeImage(itemNameBlock, itemNameBlock.Width * 10, itemNameBlock.Height * 10);
            itemNameBlock = ImageUtil.ResizeImage(itemNameBlock, (int)(90.0 / itemNameBlock.Height * itemNameBlock.Width), 90);
            itemName = Tesseract.GetTextFromBitmap(itemNameBlock).Replace("\r", "").Replace("\n", " ").Replace("GB", "O").Replace("G3", "O").Replace("EB", "O").Replace("G9", "O");
            itemName = Tesseract.CorrectSpelling(itemName);
            //itemName = Regex.Replace(itemName, "([AEIOUM]|^)ITI", "$1M");
            return itemName;
        }

        public string ParseItemType(out string quality)
        {
            string itemType = "Unknown";
            quality = "Unknown";
            try
            {
                Func<Color, bool> colorFunc =
                    c =>
                    ImageUtil.GetGrayValue(c) > 130 && !(Math.Abs(c.R - c.G) < 30 && Math.Abs(c.G - c.B) < 30);
                Bound itemTypebound = ImageUtil.GetBlockBounding(Resized,
                                                                 new Bound(new Point(s(82 / 410.0), s(55 / 410.0)),
                                                                           new Point(s(270 / 410.0), s(92 / 400.0))),
                                                                 colorFunc).Expand(6);
                if (itemTypebound.Height > s(46 / 400.0))
                    itemTypebound = new Bound(new Point(itemTypebound.P1.X, itemTypebound.P1.Y),
                                              new Point(itemTypebound.P2.X, itemTypebound.P1.Y + s(30 / 411.0)));
                if (itemTypebound.Width < 5)
                    return itemType;
                ImageUtil.DrawBlockBounding(Processed, itemTypebound);
                Bitmap itemTypeBlock = Resized.Clone(itemTypebound.ToRectangle(), Resized.PixelFormat);
                itemTypeBlock = ImageUtil.ResizeImage(itemTypeBlock, itemTypeBlock.Width * 8, itemTypeBlock.Height * 8);
                //itemTypeBlock = ImageUtil.ResizeImage(itemTypeBlock, (int)(80.0 / itemTypeBlock.Height * itemTypeBlock.Width), 80);
                string text =
                    Tesseract.GetTextFromBitmap(ImageUtil.Sharpen(itemTypeBlock)).Replace("\r", "").Replace("\n", " ");
                var words = text.Split(new[] { ' ' });
                if (words.Length > 1)
                {
                    string qualityString = words[0];
                    quality =
                        Data.ItemQualities.OrderByDescending(i => qualityString.DiceCoefficient(i)).FirstOrDefault();
                    itemType = String.Join(" ", words.Skip(1));
                    itemType = Data.ItemTypes.OrderByDescending(i => itemType.DiceCoefficient(i)).FirstOrDefault();
                    return itemType;
                }
            }
            catch { }
            return itemType;
        }

        public string ParseMeta()
        {
            Func<Color, bool> whiteFunc =
                c => c.B > 150 && Math.Abs(c.R - c.G) < 8 && Math.Abs(c.R - c.B) < 8;
            var bounds = ImageUtil.GetTextLineBounds(Resized,
                                                     new Bound(new Point(80, 158 < Resized.Height ? 158 : Resized.Height),
                                                               new Point(200, 218 < Resized.Height ? 218 : Resized.Height)),
                                                     13,
                                                     whiteFunc, 80, 200);
            if (bounds.Count != 2)
                return "";
            List<string> res = new List<string>();
            foreach (var bound in bounds)
            {
                if (bound.Width < 20)
                    continue;
                ImageUtil.DrawBlockBounding(Processed, bound);
                var newBound = new Bound(new Point(bound.P1.X - 6, bound.P1.Y - 4),
                                         new Point(bound.P2.X + 4, bound.P2.Y + 4));
                var block = Resized.Clone(bound.Expand(3).ToRectangle(), Resized.PixelFormat);
                block = ImageUtil.ResizeImage(block, block.Width * 6, block.Height * 6);
                string text = Tesseract.GetTextFromBitmap(block, @"-psm 7 nobatch tesseract\d3meta");
                text = text.Replace(" ", "").Replace("+", "");
                if (text.StartsWith("4-") && text.EndsWith("70"))
                    text = text.Replace("4-", "").Replace("70", "%");
                res.Add(text);
            }
            return string.Join(",", res.ToArray());
        }

        
        public double ParseDPS()
        {
            double dps = 0;
            Func<Color, bool> colorFunc =
                c =>
                ImageUtil.GetGrayValue(c) > 240 && (Math.Abs(c.R - c.G) < 5 && Math.Abs(c.G - c.B) < 5);
            Bound dpsBound = ImageUtil.GetBlockBounding(Resized,
                                                                 new Bound(new Point(s(82 / 410.0), s(86 / 410.0)), new Point(s(238 / 410.0), s(160 / 410.0))), colorFunc).Expand(s(10/410.0));
            if (dpsBound.Width < 24)
                return dps;
            _affixStartY = dpsBound.P2.Y;
            ImageUtil.DrawBlockBounding(Processed, dpsBound);
            //dpsBound = new Bound(new Point(dpsBound.P1.X, dpsBound.P1.Y), new Point(dpsBound.P2.X + 150, dpsBound.P2.Y));
            Bitmap dpsBlock = Resized.Clone(dpsBound.ToRectangle(), Resized.PixelFormat);
            dpsBlock = ImageUtil.ResizeImage(dpsBlock, (int)(22.0 / dpsBlock.Height * dpsBlock.Width), 22);
            //dpsBlock = ImageUtil.ImproveForOCR(dpsBlock);
            //dpsBlock = ImageUtil.DpsFix(dpsBlock);
            string text = Tesseract.GetTextFromBitmap(dpsBlock, @"-psm 7 nobatch tesseract\d3digits");
            double.TryParse(text, out dps);
            return dps;
        }

        public Dictionary<string, string> ParseAffixes(out string socketBonuses)
        {
            Func<Color, bool> whiteFunc =
                c => c.B > 160 && Math.Abs(c.R - c.G) < 1 && Math.Abs(c.R - c.B) < 1;
            Func<Color, bool> colorFunc =
                c => c.B > 150 && c.G < 120 && c.R < 120 || whiteFunc(c);
            List<string> affixStrings = new List<string>();
            socketBonuses = "";

            for (int y = _affixStartY; y < _affixStartY + 80; y++)
            {
                bool found = false;
                for (int x = s(58 / 320.0); x < s(74 / 320.0); x += 1)
                {
                    Color c = Resized.GetPixel(x, y);
                    if (c.B > 150 && c.G < 120 && c.R < 120)
                    {
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    _affixStartY = y;
                    break;
                }
            }

            var bounds = ImageUtil.GetTextLineBounds(Resized,
                                                     new Bound(new Point(s(6 / 320.0), _affixStartY),
                                                               new Point(s(300/320.0), Resized.Height - s(54/320.0))),
                                                     s(10/320.0),
                                                     colorFunc, s(58 / 320.0), s(74 / 320.0));
            Dictionary<string, string> res = new Dictionary<string, string>();
            foreach (var bound in bounds)
            {
                if (bound.Width < 20)
                    continue;
                ImageUtil.DrawBlockBounding(Processed, bound);
                var block = Resized.Clone(bound.Expand(4).ToRectangle(), Resized.PixelFormat);
                block = ImageUtil.ResizeImage(block, block.Width*6, block.Height*6);
                //block = ImageUtil.ResizeImage(block, (int)(80.0 / block.Height * block.Width), 80);
                string text = Tesseract.GetTextFromBitmap(block);
                if (Enumerable.Range(s(58 / 320.0), 15).Where(x => whiteFunc(Resized.GetPixel(x, bound.P1.Y + 4))).Count() > 0)
                {
                    affixStrings.Add("Empty Socket");
                    var pair = Data.affixMatches.Select(p => new { p.Key, p.Value, Coe = text.DiceCoefficient(p.Value) }).OrderByDescending(o => o.Coe).First();
                    Match m;
                    if (pair.Coe > 0.20 && (m = Regex.Match(text, "([0-9]+-[0-9]+|[0-9\\.]+)")).Success)
                    {
                        string v = m.Value + " " + pair.Key;
                        socketBonuses += socketBonuses == "" ? v : ", " + v;
                    }
                }
                else if (bound.P1.X < s(16 / 320.0))
                    affixStrings[affixStrings.Count - 1] += " " + text;
                else
                    affixStrings.Add(text);
            }

            foreach (var affixString in affixStrings)
            {
                string text = affixString;
                while (text.Contains("1 1"))
                    text = text.Replace("1 1", "11");
                var pair = Data.affixMatches.Select(p => new { p.Key, p.Value, Coe = text.DiceCoefficient(p.Value) }).OrderByDescending(o => o.Coe).First();
                if (pair.Coe < 0.20)
                    continue;
                List<string> values = new List<string>();
                Match m = Regex.Match(text, "[0-9]+-[0-9]+");
                if (m.Success)
                {
                    if (pair.Key == "Dmg")
                    {
                        var parts = m.Value.Split(new char[] {'-'});
                        res.Add("MinD", parts[0]);
                        res.Add("MaxD", parts[1]);
                        continue;
                    }
                    values.Add(m.Value);
                }
                else if ((m = Regex.Match(text, "[0-9\\.]+")).Success)
                    values.Add(m.Value);

                if (values.Count == 0 && pair.Key != "Soc" && pair.Key != "Ind")
                    continue;
                if (!res.ContainsKey(pair.Key) && pair.Key == "Soc")
                    res.Add(pair.Key, "1");
                else if (!res.ContainsKey(pair.Key))
                    res.Add(pair.Key, String.Join("*", values));
                else if (pair.Key == "Soc")
                    res[pair.Key] = (int.Parse(res[pair.Key]) + 1).ToString();
            }

            //Corrections (need to extract this to another class/method)
            Dictionary<string, string> finalRes = new Dictionary<string, string>();
            foreach (var r in res)
            {
                string key = r.Key;
                string value = r.Value;
                if (key == "Life%")
                {
                    int n;
                    if (int.TryParse(value, out n))
                    {
                        if (n > 20)
                            n = int.Parse(n.ToString().Substring(0, 1));
                        value = n.ToString();
                    }
                }
                finalRes.Add(key, value);
            }

            return finalRes;
        }

    }
}
