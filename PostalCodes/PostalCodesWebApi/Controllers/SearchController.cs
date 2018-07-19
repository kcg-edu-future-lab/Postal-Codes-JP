using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostalCodesWebApi.Models;

namespace PostalCodesWebApi.Controllers
{
    /// <summary>
    /// 郵便番号と町域のデータを検索します。
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        static readonly Regex KeywordPattern = new Regex(@"(?<=^|\s)" + @"\S+" + @"(?=$|\s)");

        /// <summary>
        /// 任意のキーワードを指定して、郵便番号と町域のリストを取得します。部分一致検索です。
        /// </summary>
        /// <param name="q">検索キーワード。空白文字で区切って複数指定できます。</param>
        /// <returns>郵便番号と町域のリスト</returns>
        /// <remarks>
        /// 検索対象は、都道府県、市区町村、町域のそれぞれの名前およびかなです。
        /// 検索結果が 1000 件を超える場合、ステータスコード 400 が返されます。
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Town>))]
        [ProducesResponseType(400)]
        public IActionResult Get(string q)
        {
            return this.OkOrTooLarge(GetValue(), 1000);

            IEnumerable<Town> GetValue()
            {
                if (string.IsNullOrWhiteSpace(q)) return PostalCodesData.Towns;

                var keywords = KeywordPattern.Matches(q).Select(m => m.Value).ToArray();

                return PostalCodesData.TownFullWords
                    .Where(x => keywords.All(k => x.Value.Contains(k)))
                    .Select(x => x.Key);
            }
        }
    }
}
