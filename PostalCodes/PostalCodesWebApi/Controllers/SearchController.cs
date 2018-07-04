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

        [HttpGet]
        public IEnumerable<Town> Get(string q)
        {
            if (string.IsNullOrWhiteSpace(q)) return Enumerable.Empty<Town>();

            var keywords = KeywordPattern.Matches(q).Select(m => m.Value).ToArray();

            IEnumerable<Town> query = PostalCodesData.Towns;
            foreach (var keyword in keywords)
                query = query.Where(x => x.Name.Contains(keyword));
            return query;
        }
    }
}
