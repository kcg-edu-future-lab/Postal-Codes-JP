using System;
using System.Diagnostics;

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
        /// <summary>都道府県の名前</summary>
        public string Name { get; set; }
        /// <summary>都道府県のかな</summary>
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
        /// <summary>市区町村の名前</summary>
        public string Name { get; set; }
        /// <summary>市区町村のかな</summary>
        public string Kana { get; set; }
        /// <summary>都道府県</summary>
        public Prefecture Prefecture { get; set; }
    }

    /// <summary>
    /// 郵便番号と町域の対応を表します。
    /// </summary>
    [DebuggerDisplay(@"\{{PostalCode}:{TownName}\}")]
    public class PostalCodeEntry
    {
        /// <summary>郵便番号 (7 桁)</summary>
        public string PostalCode { get; set; }
        /// <summary>町域の名前</summary>
        public string TownName { get; set; }
        /// <summary>町域のかな</summary>
        public string TownKana { get; set; }
        /// <summary>備考</summary>
        public string Remarks { get; set; }
        /// <summary>市区町村</summary>
        public City City { get; set; }
    }
}
