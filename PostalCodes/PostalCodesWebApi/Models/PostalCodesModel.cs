using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PostalCodesWebApi.Models
{
    public static class PostalCodesModel
    {
        public static Prefecture[] Prefectures { get; private set; }
        public static IDictionary<string, Prefecture> PrefecturesMap { get; private set; }

        public static City[] Cities { get; private set; }

        public static void LoadData(string webRootPath)
        {
            Prefectures = GetPrefectures(Path.Combine(webRootPath, "App_Data", "Prefectures.csv"));
            PrefecturesMap = Prefectures.ToDictionary(p => p.Code);

            Cities = GetCities(Path.Combine(webRootPath, "App_Data", "Cities.csv"));
        }

        static Prefecture[] GetPrefectures(string path) =>
            CsvFile.ReadRecordsByArray(path, true)
                .Select(l => new Prefecture
                {
                    Code = l[0],
                    Name = l[1],
                    Kana = l[2],
                })
                .ToArray();

        static City[] GetCities(string path) =>
            CsvFile.ReadRecordsByArray(path, true)
                .Select(l => new City
                {
                    Code = l[1],
                    Name = l[2],
                    Kana = l[3],
                    Prefecture = PrefecturesMap[l[0]],
                })
                .ToArray();
    }

    /// <summary>
    /// 都道府県を表します。
    /// </summary>
    [DebuggerDisplay(@"\{{Code}:{Name}\}")]
    public class Prefecture
    {
        /// <summary>都道府県コード (2 桁)。</summary>
        public string Code { get; set; }
        /// <summary>名前。</summary>
        public string Name { get; set; }
        /// <summary>かな。</summary>
        public string Kana { get; set; }
    }

    /// <summary>
    /// 市区町村を表します。
    /// </summary>
    [DebuggerDisplay(@"\{{Code}:{Name}\}")]
    public class City
    {
        /// <summary>市区町村コード (5 桁)。</summary>
        public string Code { get; set; }
        /// <summary>名前。</summary>
        public string Name { get; set; }
        /// <summary>かな。</summary>
        public string Kana { get; set; }
        /// <summary>都道府県。</summary>
        public Prefecture Prefecture { get; set; }
    }
}
