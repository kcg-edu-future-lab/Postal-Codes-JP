using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics;

namespace PostalCodesDataConsole
{
    public class PostalCodesDb : DbContext
    {
        // Sets the value of |DataDirectory| for an attached DB.
        static PostalCodesDb() =>
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);

        public DbSet<Pref> Prefs { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Town> Towns { get; set; }
    }

    [DebuggerDisplay(@"\{{Name}\}")]
    public class Pref
    {
        [Column(TypeName = "nchar")]
        [Key, StringLength(2, MinimumLength = 2)]
        public string Code { get; set; }

        [Required, MaxLength(64)]
        public string Name { get; set; }
        [Required, MaxLength(64)]
        public string Kana { get; set; }

        public ICollection<City> Cities { get; set; }
    }

    [DebuggerDisplay(@"\{{Name}\}")]
    public class City
    {
        [Column(TypeName = "nchar")]
        [Key, StringLength(5, MinimumLength = 5)]
        public string Code { get; set; }

        [Required, MaxLength(64)]
        public string Name { get; set; }
        [Required, MaxLength(64)]
        public string Kana { get; set; }

        [Required]
        public Pref Pref { get; set; }
        public ICollection<Town> Towns { get; set; }
    }

    [DebuggerDisplay(@"\{{Name}\}")]
    public class Town
    {
        [Column(TypeName = "nchar", Order = 0)]
        [Key, StringLength(7, MinimumLength = 7)]
        public string PostalCode { get; set; }
        [Column(Order = 1)]
        [Key]
        public int Index { get; set; }

        [Required(AllowEmptyStrings = true), MaxLength(64)]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = true), MaxLength(64)]
        public string Kana { get; set; }
        public string Remarks { get; set; }

        [Required]
        public City City { get; set; }
    }
}
