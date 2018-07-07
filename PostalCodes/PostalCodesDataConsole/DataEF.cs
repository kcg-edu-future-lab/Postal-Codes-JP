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
                // Makes AddRange method faster.
                db.Configuration.AutoDetectChangesEnabled = false;

                db.Prefs.AddRange(CreatePrefs(remodeledData));
                Console.WriteLine("Added Prefs.");

                db.Cities.AddRange(CreateCities(remodeledData, db));
                Console.WriteLine("Added Cities.");

                db.Towns.AddRange(CreateTowns(remodeledData, db));
                Console.WriteLine("Added Towns.");

                db.SaveChanges();
                Console.WriteLine("Saved changes.");
            }
        }

        static IEnumerable<Pref> CreatePrefs(CsvDataInfo[] remodeledData)
        {
            return remodeledData[0].Records
                .Select(l => new Pref
                {
                    Code = l[0],
                    Name = l[1],
                    Kana = l[2],
                });
        }

        static IEnumerable<City> CreateCities(CsvDataInfo[] remodeledData, PostalCodesDb db)
        {
            return remodeledData[1].Records
                .Select(l => new City
                {
                    Code = l[1],
                    Name = l[2],
                    Kana = l[3],
                    Pref = db.Prefs.Find(l[0]),
                });
        }

        static IEnumerable<Town> CreateTowns(CsvDataInfo[] remodeledData, PostalCodesDb db)
        {
            return remodeledData[2].Records
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
        }
    }
}
