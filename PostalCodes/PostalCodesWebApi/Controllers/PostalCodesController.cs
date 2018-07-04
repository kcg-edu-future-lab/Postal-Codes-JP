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
        /// 市区町村コード (5 桁) を指定して、郵便番号と町域のリストを取得します。
        /// </summary>
        /// <param name="cityCode">市区町村コード (5 桁)</param>
        /// <returns>郵便番号と町域のリスト</returns>
        [HttpGet("ByCity/{cityCode:regex(^[[0-9]]{{5}}$)}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Town>))]
        [ProducesResponseType(404)]
        public IActionResult GetByCity(string cityCode)
        {
            if (!PostalCodesData.Cities.ContainsKey(cityCode)) return NotFound();

            var city = PostalCodesData.Cities[cityCode];
            return Ok(PostalCodesData.CityTownsMap[city]);
        }

        /// <summary>
        /// 郵便番号 (7 桁) を指定して、郵便番号と町域のリストを取得します。
        /// </summary>
        /// <param name="postalCode">郵便番号 (7 桁)。ハイフンの有無は問いません。</param>
        /// <returns>郵便番号と町域のリスト</returns>
        /// <remarks>一つの郵便番号に複数の町域が割り当てられている場合があるため、戻り値はリストです。</remarks>
        [HttpGet("{postalCode:regex(^[[0-9]]{{3}}-?[[0-9]]{{4}}$)}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Town>))]
        [ProducesResponseType(404)]
        public IActionResult Get(string postalCode)
        {
            postalCode = postalCode.Replace("-", "");

            if (!PostalCodesData.PostalCodes.ContainsKey(postalCode)) return NotFound();

            return Ok(PostalCodesData.PostalCodes[postalCode]);
        }

        /// <summary>
        /// 郵便番号の一部 (3 桁以上) を指定して、郵便番号と町域のリストを取得します。前方一致検索です。
        /// </summary>
        /// <param name="postalCode">郵便番号の一部 (3 桁以上)。ハイフンの有無は問いません。</param>
        /// <returns>郵便番号と町域のリスト</returns>
        /// <remarks>戻り値は郵便番号の順に並びます。</remarks>
        [HttpGet("ByPartial/{postalCode:regex(^[[0-9]]{{3}}-?[[0-9]]{{0,4}}$)}")]
        public IEnumerable<Town> GetByPartial(string postalCode)
        {
            postalCode = postalCode.Replace("-", "");

            return PostalCodesData.Towns.Where(x => x.PostalCode.StartsWith(postalCode))
                .OrderBy(x => x.PostalCode);
        }
    }
}
