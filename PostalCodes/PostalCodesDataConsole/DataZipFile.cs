using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using Bellona.IO;

namespace PostalCodesDataConsole
{
    public static class DataZipFile
    {
        public static string[][] FromOriginalFile(string originalZipPath)
        {
            using (var input = File.OpenRead(originalZipPath))
            {
                return FromOriginal(input);
            }
        }

        public static string[][] FromOriginalUri(string originalZipUri)
        {
            using (var web = new WebClient())
            using (var input = web.OpenRead(originalZipUri))
            {
                return FromOriginal(input);
            }
        }

        public static string[][] FromOriginal(Stream input)
        {
            using (var zip = new ZipArchive(input, ZipArchiveMode.Read))
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
