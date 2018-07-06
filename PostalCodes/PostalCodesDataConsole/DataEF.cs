using System;
using System.Collections.Generic;
using System.Linq;

namespace PostalCodesDataConsole
{
    public static class DataEF
    {
        public static void CreateDatabase(CsvDataInfo[] remodeledData)
        {
            using (var db = new PostalCodesDb())
            {
                var prefs = remodeledData[0].Records
                    .Select(l => new Pref
                    {
                        Code = l[0],
                        Name = l[1],
                        Kana = l[2],
                    });
                db.Prefs.AddRange(prefs);

                var cities = remodeledData[1].Records
                    .Select(l => new City
                    {
                        Code = l[1],
                        Name = l[2],
                        Kana = l[3],
                        Pref = db.Prefs.Find(l[0]),
                    });
                db.Cities.AddRange(cities);

                var towns = remodeledData[2].Records
                    .GroupBy(l => l[1])
                    .SelectMany(g => g
                        .Select((l, i) => new Town
                        {
                            PostalCode = l[1],
                            Index = i,
                            Name = l[2],
                            Kana = l[3],
                            Remarks = l[4] != "" ? l[4] : null,
                            City = db.Cities.Find(l[0]),
                        }));
                db.Towns.AddRange(towns);

                db.SaveChanges();
            }
        }
    }
}
