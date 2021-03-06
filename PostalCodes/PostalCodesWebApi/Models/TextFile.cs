﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PostalCodesWebApi.Models
{
    public static class TextFile
    {
        public static readonly Encoding UTF8N = new UTF8Encoding();
        // Install System.Text.Encoding.CodePages, if the Shift_JIS is needed.
        //public static readonly Encoding ShiftJIS = Encoding.GetEncoding("shift_jis");

        public static IEnumerable<string> ReadLines(this Stream stream, Encoding encoding = null)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            using (var reader = new StreamReader(stream, encoding ?? UTF8N))
            {
                while (!reader.EndOfStream)
                    yield return reader.ReadLine();
            }
        }

        public static IEnumerable<string> ReadLines(string path, Encoding encoding = null) =>
            File.ReadLines(path, encoding ?? UTF8N);

        public static void WriteLines(this Stream stream, IEnumerable<string> lines, Encoding encoding = null)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (lines == null) throw new ArgumentNullException(nameof(lines));

            using (var writer = new StreamWriter(stream, encoding ?? UTF8N))
            {
                foreach (var line in lines)
                    writer.WriteLine(line);
            }
        }

        public static void WriteLines(string path, IEnumerable<string> lines, Encoding encoding = null) =>
            File.WriteAllLines(path, lines, encoding ?? UTF8N);
    }
}
