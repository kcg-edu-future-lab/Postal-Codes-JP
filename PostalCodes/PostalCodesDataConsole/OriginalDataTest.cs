using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bellona.IO;

namespace PostalCodesDataConsole
{
    public static class OriginalDataTest
    {
        static string[][] OriginalData;

        public static void ExecuteTest()
        {
            OriginalData = DataZipFile.FromOriginal(Program.KenAll_Local_Path);
            Directory.CreateDirectory(nameof(OriginalDataTest));

            SaveResult(nameof(MultiPostalCodes), MultiPostalCodes());
            SaveResult(nameof(MultiTowns), MultiTowns());
            SaveResult(nameof(ConsecutiveData), ConsecutiveData());
            SaveResult(nameof(ConsecutiveData_Reason), ConsecutiveData_Reason());
            SaveResult(nameof(ConsecutiveData_Kana), ConsecutiveData_Kana());
        }

        static void SaveResult(string fileName, IEnumerable<string[]> result) =>
            CsvFile.WriteRecordsByArray(Path.Combine(nameof(OriginalDataTest), $"{fileName}.csv"), result, Encoding.UTF8);

        static IEnumerable<string[]> MultiPostalCodes() => OriginalData
            .Where(l => l[9] != "0");

        static IEnumerable<string[]> MultiTowns() => OriginalData
            .Where(l => l[12] != "0");

        // 郵便番号が連続しているデータのうち、その理由が文字数超過かつ複数町域のものを取得します。
        static IEnumerable<string[]> ConsecutiveData() => OriginalData
            .GroupBySequentially(l => l[2])
            .Where(g => g.Count() > 1)
            .Where(g => g.First()[8].Contains("（") && !g.First()[8].Contains("）"))
            .Where(g => g.First()[12] != "0")
            .SelectMany(g => g);

        // 郵便番号が連続している理由 (「複数町域」か、「単町域かつ文字数超過」か) が混在して連続しないことを保証します。
        static IEnumerable<string[]> ConsecutiveData_Reason() => OriginalData
            .GroupBySequentially(l => l[2])
            .Where(g => g.Count() > 1)
            .Where(g => g.Select(l => l[12]).Distinct().Count() > 1)
            .SelectMany(g => g);

        // 文字数超過により郵便番号が連続している場合のかなを検証します。
        // 各行で名前とかなは対応しているようです。
        static IEnumerable<string[]> ConsecutiveData_Kana() => OriginalData
            .GroupBySequentially(l => l[2])
            .Where(g => g.Count() > 1)
            .Where(g => g.First()[8].Contains("（") && !g.First()[8].Contains("）"))
            .Where(g => g.Select(l => l[5]).Distinct().Count() > 1)
            .SelectMany(g => g);
    }
}
