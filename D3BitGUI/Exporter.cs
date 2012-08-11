using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace D3BitGUI
{
    public static class Exporter
    {
        public static void Export(List<Dictionary<string, string>> data, string savepath, string format)
        {
            if (format == "JSON")
            {
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(savepath, json);
            }
            else if (format == "XML")
            {
                XElement root = new XElement("Items");
                foreach (var item in data)
                {
                    XElement itemEle = new XElement("Item");
                    itemEle.Add(item.Select(kv => new XElement(kv.Key, kv.Value)));
                    root.Add(itemEle);
                }
                root.Save(savepath);
            }
            else if (format == "CSV")
            {
                string res = "";
                foreach (var item in data)
                {
                    res += String.Join(", ",item.Select(kv => string.Format("\"{0}\"", kv.Value)))+"\n";
                }
                File.WriteAllText(savepath, res);
            }
        }
    }
}
