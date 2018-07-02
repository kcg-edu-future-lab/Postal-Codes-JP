using System;
using System.Collections.Generic;
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
    }
}
