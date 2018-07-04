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
    public class PostalCodesController : Controller
    {
        /// <summary>
        /// 郵便番号の一部 (3～7 桁) を指定して、郵便番号と町域のリストを取得します。前方一致検索です。
        /// </summary>
        /// <param name="postalCode">郵便番号の一部 (3～7 桁)。ハイフンの有無は問いません。</param>
        /// <returns>郵便番号と町域のリスト</returns>
        /// <remarks>
        /// 一つの郵便番号に複数の町域が割り当てられている場合があります。
        /// 検索結果は郵便番号の順に並びます。
        /// </remarks>
        [HttpGet("{postalCode:regex(^[[0-9]]{{3}}-?[[0-9]]{{0,4}}$)}")]
        public IEnumerable<Town> Get(string postalCode)
        {
            postalCode = postalCode.Replace("-", "");
            return postalCode.Length == 7 ? GetValue7() : GetValue3();

            IEnumerable<Town> GetValue7() => PostalCodesData.PostalCodes.ContainsKey(postalCode) ? PostalCodesData.PostalCodes[postalCode] : Enumerable.Empty<Town>();
            IEnumerable<Town> GetValue3() => PostalCodesData.Towns
                .Where(x => x.PostalCode.StartsWith(postalCode))
                .OrderBy(x => x.PostalCode);
        }
    }
}
