using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Bellona.IO;

namespace PostalCodesDataConsole
{
    public static class DataZipFile
    {
        public static string[][] FromOriginal(string originalZipPath)
        {
            using (var zip = ZipFile.OpenRead(originalZipPath))
            {
                var entry = zip.GetEntry("KEN_ALL.CSV") ?? zip.Entries.FirstOrDefault();
                if (entry == null) return new string[0][];

                using (var stream = entry.Open())
                {
                    return CsvFile.ReadRecordsByArray(stream, false, TextFile.ShiftJIS).ToArray();
                }
            }
        }

        public static void SaveZipFile(string remodeledZipPath, CsvDataInfo[] csvData)
        {
            File.Delete(remodeledZipPath);

            using (var zip = ZipFile.Open(remodeledZipPath, ZipArchiveMode.Create))
            {
                foreach (var _ in csvData)
                {
                    var entry = zip.CreateEntry(_.FileName);
                    using (var stream = entry.Open())
                    {
                        CsvFile.WriteRecordsByArray(stream, _.Records, _.Columns);
                    }
                }
            }
        }

        public static void SaveCsvFiles(CsvDataInfo[] csvData)
        {
            foreach (var _ in csvData)
                CsvFile.WriteRecordsByArray(_.FileName, _.Records, _.Columns);
        }
    }
}
