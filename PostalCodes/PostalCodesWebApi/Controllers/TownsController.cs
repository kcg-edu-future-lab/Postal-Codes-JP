using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostalCodesWebApi.Models;

namespace PostalCodesWebApi.Controllers
{
    /// <summary>
    /// 郵便番号と町域のデータを取得します。
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TownsController : Controller
    {
        /// <summary>
        /// 町域の名前、かなを指定して、郵便番号と町域のリストを取得します。部分一致検索です。
        /// </summary>
        /// <param name="name">町域の名前</param>
        /// <param name="kana">町域のかな</param>
        /// <returns>郵便番号と町域のリスト</returns>
        [HttpGet]
        public IEnumerable<Town> Get(string name, string kana)
        {
            IEnumerable<Town> query = PostalCodesData.Towns;

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.Name.Contains(name));
            if (!string.IsNullOrWhiteSpace(kana))
                query = query.Where(x => x.Kana.Contains(kana));

            return query;
        }
    }
}
