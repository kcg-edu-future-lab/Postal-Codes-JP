using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PostalCodesWebApi.Models
{
    public static class PostalCodesData
    {
        public static IDictionary<string, Pref> Prefs { get; private set; }

        public static IDictionary<string, City> Cities { get; private set; }
        public static IDictionary<Pref, City[]> PrefCitiesMap { get; private set; }

        public static PostalCodeEntry[] PostalCodeEntries { get; private set; }

        public static IDictionary<string, PostalCodeEntry[]> PostalCodes { get; private set; }
        public static IDictionary<City, PostalCodeEntry[]> CityPostalCodesMap { get; private set; }

        public static void LoadData(string webRootPath)
        {
            Prefs = GetPrefs(Path.Combine(webRootPath, "App_Data", "Prefs.csv"));

            Cities = GetCities(Path.Combine(webRootPath, "App_Data", "Cities.csv"));
            PrefCitiesMap = Cities.Values
                .GroupBySequentially(x => x.Pref)
                .ToDictionary(g => g.Key, g => g.ToArray());

            PostalCodeEntries = GetPostalCodeEntries(Path.Combine(webRootPath, "App_Data", "Towns.csv"));

            PostalCodes = PostalCodeEntries
                .GroupBy(x => x.PostalCode)
                .ToDictionary(g => g.Key, g => g.ToArray());
            CityPostalCodesMap = PostalCodeEntries
                .GroupBySequentially(x => x.City)
                .ToDictionary(g => g.Key, g => g.ToArray());
        }

        static IDictionary<string, Pref> GetPrefs(string path) =>
            CsvFile.ReadRecordsByArray(path, true)
                .Select(l => new Pref
                {
                    Code = l[0],
                    Name = l[1],
                    Kana = l[2],
                })
                .ToDictionary(p => p.Code);

        static IDictionary<string, City> GetCities(string path) =>
            CsvFile.ReadRecordsByArray(path, true)
                .Select(l => new City
                {
                    Code = l[1],
                    Name = l[2],
                    Kana = l[3],
                    Pref = Prefs[l[0]],
                })
                .ToDictionary(p => p.Code);

        static PostalCodeEntry[] GetPostalCodeEntries(string path) =>
            CsvFile.ReadRecordsByArray(path, true)
                .Select(l => new PostalCodeEntry
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
