using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Bellona.IO;

namespace PostalCodesDataConsole
{
    public static class DataEFTest
    {
        const string OutputDirPath = @"..\..\..\..\..\Data\Test\DataEFTest";

        public static void ExecuteTest()
        {
            Directory.CreateDirectory(OutputDirPath);

            SaveResult(nameof(TownNames_Empty), TownNames_Empty());
            SaveResult(nameof(MultiTowns_Max), MultiTowns_Max());
            SaveResult(nameof(Kana_Number), Kana_Number());
            SaveResult(nameof(SingleChars), SingleChars());
        }

        static void SaveResult(string fileName, IEnumerable<string> result) =>
            TextFile.WriteLines(Path.Combine(OutputDirPath, $"{fileName}.txt"), result);

        static string Hyphenate(this string postalCode) =>
            postalCode.Insert(3, "-");

        static IEnumerable<string> TownNames_Empty()
        {
            using (var db = new PostalCodesDb())
            {
                var towns = db.Towns.Include("City.Pref")
                    .Where(x => x.Name == "")
                    .ToArray();

                return towns
                    .GroupBy(x => x.PostalCode)
                    .Where(g => g.Count() > 1 || g.First().Remarks != "お探しの町域が見つからない場合")
                    .SelectMany(g => g)
                    .Select(x => $"{x.PostalCode.Hyphenate()} {x.Index:D2}: {x.City.Pref.Name} {x.City.Name} {x.Remarks}");
            }
        }

        static IEnumerable<string> MultiTowns_Max()
        {
            using (var db = new PostalCodesDb())
            {
                var maxIndex = db.Towns.Max(x => x.Index);
                var maxIndexTowns = db.Towns.Where(x => x.Index == maxIndex).ToArray();

                return maxIndexTowns
                    .SelectMany(mt => db.Towns.Include("City.Pref").Where(x => x.PostalCode == mt.PostalCode))
                    .Select(x => $"{x.PostalCode.Hyphenate()} {x.Index:D2}: {x.City.Pref.Name} {x.City.Name} {x.Name}")
                    .ToArray();
            }
        }

        static IEnumerable<string> Kana_Number()
        {
            var numberPattern = new Regex("[０-９]");

            using (var db = new PostalCodesDb())
            {
                var towns = db.Towns.Include("City.Pref").ToArray();

                return towns
                    .Where(x => numberPattern.IsMatch(x.Kana))
                    .Where(x => !numberPattern.IsMatch(x.Name))
                    .Select(x => $"{x.City.Pref.Name} {x.City.Name} {x.Name} ({x.Kana})");
            }
        }

        static IEnumerable<string> SingleChars()
        {
            using (var db = new PostalCodesDb())
            {
                var towns = db.Towns.Include("City.Pref").ToArray();

                var texts = towns
                    .Select(x => new { name = x.Name, text = $"{x.City.Pref.Name} {x.City.Name} {x.Name} ({x.Kana})" })
                    .GroupBy(_ => _.text)
                    .Select(g => g.First())
                    .ToArray();

                var chars = texts
                    .SelectMany(_ => _.name.Select(c => new { c, _.text }))
                    .GroupBy(_ => _.c)
                    .Where(g => g.Count() <= 1)
                    .SelectMany(g => g)
                    .ToArray();

                return chars
                    .GroupBy(_ => _.text)
                    .Select(g => $"{string.Concat(g.Select(_ => _.c))}: {g.Key}");
            }
        }
    }
}
