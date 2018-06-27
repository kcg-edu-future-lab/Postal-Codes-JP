using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PostalCodesWebApi.Models
{
    public static class PostalCodesData
    {
        public static IDictionary<string, Prefecture> Prefectures { get; private set; }

        public static IDictionary<string, City> Cities { get; private set; }
        public static IDictionary<Prefecture, City[]> PrefectureCitiesMap { get; private set; }

        public static void LoadData(string webRootPath)
        {
            Prefectures = GetPrefectures(Path.Combine(webRootPath, "App_Data", "Prefectures.csv"));

            Cities = GetCities(Path.Combine(webRootPath, "App_Data", "Cities.csv"));
            PrefectureCitiesMap = Cities.Values
                .GroupBySequentially(x => x.Prefecture)
                .ToDictionary(g => g.Key, g => g.ToArray());
        }

        static IDictionary<string, Prefecture> GetPrefectures(string path) =>
            CsvFile.ReadRecordsByArray(path, true)
                .Select(l => new Prefecture
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
                    Prefecture = Prefectures[l[0]],
                })
                .ToDictionary(p => p.Code);
    }
}
