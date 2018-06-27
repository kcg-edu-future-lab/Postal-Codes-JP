using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PostalCodesWebApi.Models
{
    /// <summary>
    /// 都道府県を表します。
    /// </summary>
    [DebuggerDisplay(@"\{{Code}:{Name}\}")]
    public class Prefecture
    {
        /// <summary>都道府県コード (2 桁)</summary>
        public string Code { get; set; }
        /// <summary>名前</summary>
        public string Name { get; set; }
        /// <summary>かな</summary>
        public string Kana { get; set; }
    }

    /// <summary>
    /// 市区町村を表します。
    /// </summary>
    [DebuggerDisplay(@"\{{Code}:{Name}\}")]
    public class City
    {
        /// <summary>市区町村コード (5 桁)</summary>
        public string Code { get; set; }
        /// <summary>名前</summary>
        public string Name { get; set; }
        /// <summary>かな</summary>
        public string Kana { get; set; }
        /// <summary>都道府県</summary>
        public Prefecture Prefecture { get; set; }
    }
}
