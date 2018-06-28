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
        /// 郵便番号 (7 桁) を指定して、郵便番号と町域のリストを取得します。
        /// </summary>
        /// <param name="postalCode">郵便番号 (7 桁)</param>
        /// <returns>郵便番号と町域のリスト</returns>
        [HttpGet("{postalCode:regex(^[[0-9]]{{3}}-?[[0-9]]{{4}}$)}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PostalCodeEntry>))]
        [ProducesResponseType(404)]
        public IActionResult Get(string postalCode)
        {
            postalCode = postalCode.Replace("-", "");

            if (!PostalCodesData.PostalCodes.ContainsKey(postalCode)) return NotFound();

            return Ok(PostalCodesData.PostalCodes[postalCode]);
        }

        /// <summary>
        /// 市区町村コード (5 桁) を指定して、郵便番号と町域のリストを取得します。
        /// </summary>
        /// <param name="cityCode">市区町村コード (5 桁)</param>
        /// <returns>郵便番号と町域のリスト</returns>
        [HttpGet("ByCity/{cityCode:regex(^[[0-9]]{{5}}$)}")]
        public IEnumerable<PostalCodeEntry> GetByCity(string cityCode)
        {
            if (!PostalCodesData.Cities.ContainsKey(cityCode)) return Enumerable.Empty<PostalCodeEntry>();

            var city = PostalCodesData.Cities[cityCode];
            return PostalCodesData.CityPostalCodesMap[city];
        }
    }
}
