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
        }

        static void SaveResult(string fileName, IEnumerable<string[]> result) =>
            CsvFile.WriteRecordsByArray(Path.Combine(nameof(OriginalDataTest), $"{fileName}.csv"), result, Encoding.UTF8);

        static IEnumerable<string[]> MultiPostalCodes() => OriginalData
            .Where(l => l[9] != "0");

        static IEnumerable<string[]> MultiTowns() => OriginalData
            .Where(l => l[12] != "0");
    }
}
