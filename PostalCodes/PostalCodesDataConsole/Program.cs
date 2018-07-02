using System;

namespace PostalCodesDataConsole
{
    class Program
    {
        const string KenAll_Local_Path = @"..\..\..\..\..\Data\Original\201805\ken_all.zip";
        const string Kyoto_Local_Path = @"..\..\..\..\..\Data\Original\201805\26kyouto.zip";
        const string KenAll_JP_Uri = "http://www.post.japanpost.jp/zipcode/dl/kogaki/zip/ken_all.zip";

        static void Main(string[] args)
        {
            var originalData = DataZipFile.FromOriginal(KenAll_Local_Path);

            DataCreator.CreateDataCsvFiles(originalData);
        }
    }
}
