using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace D3Bit
{
    public static class Data
    {

        public static Dictionary<string, string> ItemQualities;
        public static Dictionary<string, string> WeaponTypes;
        public static Dictionary<string, string> OffHandTypes;
        public static Dictionary<string, string> FollowerTypes;
        public static Dictionary<string, string> CommonTypes;

        public static Dictionary<string, string> ItemTypes;

        public static Dictionary<string, string> affixMatches;

        public static void LoadAffixes(string languageCode)
        {
            string json = File.ReadAllText(string.Format(@"data\affixes.{0}.json", languageCode));
            affixMatches = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            json = File.ReadAllText(string.Format(@"data\strings.{0}.json", languageCode));
            var strings = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
            ItemQualities = strings["ItemQualities"];
            WeaponTypes = strings["WeaponTypes"];
            OffHandTypes = strings["OffHandTypes"];
            FollowerTypes = strings["FollowerTypes"];
            CommonTypes = strings["CommonTypes"];
            ItemTypes = WeaponTypes.Union(OffHandTypes).Union(CommonTypes).ToDictionary(a => a.Key, b => b.Value);
        }

    }
}
