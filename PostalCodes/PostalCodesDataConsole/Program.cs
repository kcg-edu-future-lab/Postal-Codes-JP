using System;

namespace PostalCodesDataConsole
{
    class Program
    {
        public const string KenAll_Local_Path = @"..\..\..\..\..\Data\Original\201805\ken_all.zip";
        public const string Kyoto_Local_Path = @"..\..\..\..\..\Data\Original\201805\26kyouto.zip";
        public const string KenAll_JP_Uri = "http://www.post.japanpost.jp/zipcode/dl/kogaki/zip/ken_all.zip";

        public const string PostalCodesData_Path = @"..\..\..\..\..\Data\Remodeled\201805\PostalCodesData.zip";

        static void Main(string[] args)
        {
            CreateDataZipFile();
        }

        static void CreateDataZipFile()
        {
            var originalData = DataZipFile.FromOriginal(KenAll_Local_Path);
            var remodeledData = DataCreator.CreateCsvData(originalData);
            DataZipFile.SaveZipFile(PostalCodesData_Path, remodeledData);
        }

        static void CreateDatabase()
        {
            var originalData = DataZipFile.FromOriginal(KenAll_Local_Path);
            var remodeledData = DataCreator.CreateCsvData(originalData);
            DataEF.CreateDatabase(remodeledData);
        }
    }
}
