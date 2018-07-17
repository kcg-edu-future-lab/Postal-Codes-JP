using System;

namespace PostalCodesDataConsole
{
    class Program
    {
        public const string KenAll_Local_Path = @"..\..\..\..\..\Data\Original\201805\ken_all.zip";
        public const string Kyoto_Local_Path = @"..\..\..\..\..\Data\Original\201805\26kyouto.zip";
        public const string KenAll_JP_Uri = "http://www.post.japanpost.jp/zipcode/dl/kogaki/zip/ken_all.zip";

        public const string DataZip_Output_Path = @"..\..\..\..\..\Data\Remodeled\201805\PostalCodesData.zip";
        public const string DataZip_Default_Path = @"PostalCodesData.zip";

        static void Main(string[] args)
        {
            CreateDataZipFile();
        }

        static void CreateDataZipFile()
        {
            var originalData = DataZipFile.FromOriginalUri(KenAll_JP_Uri);
            var remodeledData = DataCreator.CreateCsvData(originalData);
            DataZipFile.SaveZipFile(DataZip_Default_Path, remodeledData);
        }

        static void CreateDatabase()
        {
            var originalData = DataZipFile.FromOriginalFile(KenAll_Local_Path);
            var remodeledData = DataCreator.CreateCsvData(originalData);
            DataEF.CreateDatabase(remodeledData);
        }
    }
}
