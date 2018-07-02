using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace PostalCodesDataConsole
{
    public static class DataCreator
    {
        public static readonly string[] PrefColumns = new[] { "Code", "Name", "Kana" };
        public static readonly string[] CityColumns = new[] { "PrefCode", "Code", "Name", "Kana" };
        public static readonly string[] TownColumns = new[] { "CityCode", "PostalCode", "Name", "Kana", "Remarks" };

        public static CsvDataInfo[] CreateCsvData(string[][] originalData)
        {
            return new[]
            {
                new CsvDataInfo { FileName = "Prefs.csv", Columns = PrefColumns, Records = CreatePrefs(originalData) },
                new CsvDataInfo { FileName = "Cities.csv", Columns = CityColumns, Records = CreateCities(originalData) },
                new CsvDataInfo { FileName = "Towns.csv", Columns = TownColumns, Records = CreateTowns(originalData) },
            };
        }

        public static IEnumerable<string[]> CreatePrefs(string[][] originalData)
        {
            return originalData
                .GroupBySequentially(l => l[6])
                .Select(g => g.First())
                .Select(l => new[] { l[0].Substring(0, 2), l[6], l[3].ToHiragana() });
        }

        public static IEnumerable<string[]> CreateCities(string[][] originalData)
        {
            return originalData
                .GroupBySequentially(l => l[0])
                .Select(g => g.First())
                .Select(l => new[] { l[0].Substring(0, 2), l[0], l[7], l[4].ToHiragana() });
        }

        public static IEnumerable<string[]> CreateTowns(string[][] originalData)
        {
            var remarksPattern = new Regex("（(.+)）$");

            return originalData
                .GroupBySequentially(l => l[2])
                .Select(g => (g.Count() > 1 && g.First()[8].Contains("（") && !g.First()[8].Contains("）")) ? ConcatLines(g.ToArray()) : g.ToArray())
                .SelectMany(g => g)
                .Select(l => new[] { l[0], l[2], l[8], l[5].ToHiragana(), "" })
                .Do(l =>
                {
                    if (l[2] == "以下に掲載がない場合")
                    {
                        l[4] = "お探しの町域が見つからない場合";
                        l[2] = "";
                        l[3] = "";
                    }
                })
                .Do(l =>
                {
                    if (l[2].Contains("番地がくる場合"))
                    {
                        l[4] = l[2];
                        l[2] = "";
                        l[3] = "";
                    }
                })
                .Do(l =>
                {
                    if (l[2].Length > 2 && l[2].Contains("一円"))
                    {
                        l[4] = l[2];
                        l[2] = "";
                        l[3] = "";
                    }
                })
                .Do(l =>
                {
                    var nameMatch = remarksPattern.Match(l[2]);
                    if (nameMatch.Success)
                    {
                        l[4] = nameMatch.Groups[1].Value;
                        l[2] = l[2].Remove(nameMatch.Index);
                    }
                    var kanaMatch = remarksPattern.Match(l[3]);
                    if (kanaMatch.Success)
                    {
                        l[3] = l[3].Remove(kanaMatch.Index);
                    }
                });
        }

        static string[][] ConcatLines(string[][] lines)
        {
            var line = (string[])lines[0].Clone();
            line[5] = lines.Select(l => l[5]).Aggregate((s1, s2) => (s1 == s2) ? s1 : s1 + s2);
            line[8] = lines.Select(l => l[8]).Aggregate((s1, s2) => s1 + s2);
            return new[] { line };
        }

        static string ToHiragana(this string value) =>
            Strings.StrConv(value, VbStrConv.Wide | VbStrConv.Hiragana);
    }

    public class CsvDataInfo
    {
        public string FileName { get; set; }
        public string[] Columns { get; set; }
        public IEnumerable<string[]> Records { get; set; }
    }
}
