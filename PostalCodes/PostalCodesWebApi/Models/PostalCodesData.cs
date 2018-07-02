using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace PostalCodesWebApi.Models
{
    public static class PostalCodesData
    {
        public static IDictionary<string, Pref> Prefs { get; private set; }

        public static IDictionary<string, City> Cities { get; private set; }
        public static IDictionary<Pref, City[]> PrefCitiesMap { get; private set; }

        public static Town[] Towns { get; private set; }

        public static IDictionary<string, Town[]> PostalCodes { get; private set; }
        public static IDictionary<City, Town[]> CityTownsMap { get; private set; }

        public static void LoadData(string webRootPath)
        {
            LoadDataZipFile(webRootPath);

            PrefCitiesMap = Cities.Values
                .GroupBySequentially(x => x.Pref)
                .ToDictionary(g => g.Key, g => g.ToArray());

            PostalCodes = Towns
                .GroupBy(x => x.PostalCode)
                .ToDictionary(g => g.Key, g => g.ToArray());
            CityTownsMap = Towns
                .GroupBySequentially(x => x.City)
                .ToDictionary(g => g.Key, g => g.ToArray());
        }

        static void LoadDataZipFile(string webRootPath)
        {
            var zipPath = Path.Combine(webRootPath, "App_Data", "PostalCodesData.zip");

            using (var zip = ZipFile.OpenRead(zipPath))
            {
                Prefs = GetData(zip, "Prefs.csv", GetPrefs);
                Cities = GetData(zip, "Cities.csv", GetCities);
                Towns = GetData(zip, "Towns.csv", GetTowns);
            }

            TResult GetData<TResult>(ZipArchive zip, string entryName, Func<IEnumerable<string[]>, TResult> toObjects)
            {
                var entry = zip.GetEntry(entryName);
                if (entry == null) throw new InvalidDataException("指定されたファイルが存在しません。");

                using (var stream = entry.Open())
                {
                    return toObjects(CsvFile.ReadRecordsByArray(stream, true));
                }
            }
        }

        static IDictionary<string, Pref> GetPrefs(IEnumerable<string[]> source) =>
            source
                .Select(l => new Pref
                {
                    Code = l[0],
                    Name = l[1],
                    Kana = l[2],
                })
                .ToDictionary(p => p.Code);

        static IDictionary<string, City> GetCities(IEnumerable<string[]> source) =>
            source
                .Select(l => new City
                {
                    Code = l[1],
                    Name = l[2],
                    Kana = l[3],
                    Pref = Prefs[l[0]],
                })
                .ToDictionary(p => p.Code);

        static Town[] GetTowns(IEnumerable<string[]> source) =>
            source
                .Select(l => new Town
                {
                    PostalCode = l[1],
                    Name = l[2],
                    Kana = l[3],
                    Remarks = l[4],
                    City = Cities[l[0]],
                })
                .ToArray();
    }
}
