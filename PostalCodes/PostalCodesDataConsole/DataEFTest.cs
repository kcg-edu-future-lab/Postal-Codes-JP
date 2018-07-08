using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bellona.IO;

namespace PostalCodesDataConsole
{
    public static class DataEFTest
    {
        public static void ExecuteTest()
        {
            Directory.CreateDirectory(nameof(DataEFTest));

            SaveResult(nameof(MultiTowns_Max), MultiTowns_Max());
        }

        static void SaveResult(string fileName, IEnumerable<string> result) =>
            TextFile.WriteLines(Path.Combine(nameof(DataEFTest), $"{fileName}.txt"), result);

        static string Hyphenate(this string postalCode) =>
            postalCode.Insert(3, "-");

        static IEnumerable<string> MultiTowns_Max()
        {
            using (var db = new PostalCodesDb())
            {
                var maxIndex = db.Towns.Max(x => x.Index);
                var maxIndexTowns = db.Towns.Where(x => x.Index == maxIndex).ToArray();

                return maxIndexTowns
                    .SelectMany(mt => db.Towns.Include("City.Pref").Where(x => x.PostalCode == mt.PostalCode))
                    .Select(x => $"{x.PostalCode.Hyphenate()} {x.Index.ToString("D2")}: {x.City.Pref.Name} {x.City.Name} {x.Name}")
                    .ToArray();
            }
        }
    }
}
