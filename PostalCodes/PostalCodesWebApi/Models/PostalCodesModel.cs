using System;
using System.Collections.Generic;
using System.Linq;

namespace PostalCodesWebApi.Models
{
    public class Prefecture
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Kana { get; set; }
    }

    public class City
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Kana { get; set; }
        public Prefecture Prefecture { get; set; }
    }
}
