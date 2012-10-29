using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using D3Bit;
using Newtonsoft.Json;

namespace D3BitConsole
{
    class Program
    {
        [DllImport("shell32.dll", SetLastError = true)]
        static extern IntPtr CommandLineToArgvW(
            [MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);

        static void Main(string[] args)
        {
            
            var cmds = CommandLineToArgs(Environment.CommandLine);
            if (cmds.Length == 4)
            {
                try
                {
                    Tesseract.language_code = cmds[3];
                    Bitmap bitmap = Bitmap.FromFile(cmds[1]) as Bitmap;
                    var res = Screenshot.GetToolTip(bitmap);
                    Tooltip tooltip = new D3Bit.Tooltip(res);
                    string name = tooltip.ParseItemName();
                    string quality = "";
                    string type = tooltip.ParseItemType(out quality);
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
                        var format = "JSON";
                        Console.WriteLine(Path.GetExtension(cmds[2]));
                        if (Path.GetExtension(cmds[2]).ToLower() == ".xml")
                            format = "XML";
                        else if (Path.GetExtension(cmds[2]).ToLower() == ".csv")
                            format = "CSV";
                        Export(itemDic, cmds[2], format);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        public static void Export(Dictionary<string, string> data, string savepath, string format)
        {
            if (format == "JSON")
            {
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(savepath, json);
            }
            else if (format == "XML")
            {
                XElement root = new XElement("Items");
                XElement itemEle = new XElement("Item");
                itemEle.Add(data.Select(kv => new XElement(kv.Key, kv.Value)));
                root.Add(itemEle);
                root.Save(savepath);
            }
            else if (format == "CSV")
            {
                string res = "";
                res += String.Join(", ", data.Select(kv => string.Format("\"{0}\"", kv.Value))) + "\n";
                File.WriteAllText(savepath, res);
            }
        }

        public static string[] CommandLineToArgs(string commandLine)
        {
            int argc;
            var argv = CommandLineToArgvW(commandLine, out argc);
            if (argv == IntPtr.Zero)
                throw new System.ComponentModel.Win32Exception();
            try
            {
                var args = new string[argc];
                for (var i = 0; i < args.Length; i++)
                {
                    var p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                    args[i] = Marshal.PtrToStringUni(p);
                }

                return args;
            }
            finally
            {
                Marshal.FreeHGlobal(argv);
            }
        }

    }
}
