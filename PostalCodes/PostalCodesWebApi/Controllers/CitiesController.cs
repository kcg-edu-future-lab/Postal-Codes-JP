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
    /// 市区町村のデータを取得します。
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CitiesController : Controller
    {
        /// <summary>
        /// 市区町村コードを指定して、市区町村を取得します。
        /// </summary>
        /// <param name="code">市区町村コード (5 桁)</param>
        /// <returns>市区町村</returns>
        [HttpGet("{code:regex(^[[0-9]]{{5}}$)}")]
        [ProducesResponseType(200, Type = typeof(City))]
        [ProducesResponseType(404)]
        public IActionResult Get(string code)
        {
            if (!PostalCodesData.Cities.ContainsKey(code)) return NotFound();

            return Ok(PostalCodesData.Cities[code]);
        }

        /// <summary>
        /// 名前を指定して、市区町村のリストを取得します。
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns>市区町村のリスト</returns>
        [HttpGet("ByName/{name}")]
        public IEnumerable<City> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return Enumerable.Empty<City>();

            return PostalCodesData.Cities.Values.Where(x => x.Name.Contains(name));
        }

        /// <summary>
        /// かなを指定して、市区町村のリストを取得します。
        /// </summary>
        /// <param name="kana">かな</param>
        /// <returns>市区町村のリスト</returns>
        [HttpGet("ByKana/{kana}")]
        public IEnumerable<City> GetByKana(string kana)
        {
            if (string.IsNullOrWhiteSpace(kana)) return Enumerable.Empty<City>();

            return PostalCodesData.Cities.Values.Where(x => x.Kana.Contains(kana));
        }

        /// <summary>
        /// 都道府県コードを指定して、市区町村のリストを取得します。
        /// </summary>
        /// <param name="prefectureCode">都道府県コード (2 桁)</param>
        /// <returns>市区町村のリスト</returns>
        [HttpGet("ByPrefecture/{prefectureCode:regex(^[[0-9]]{{2}}$)}")]
        public IEnumerable<City> GetByPrefecture(string prefectureCode)
        {
            if (!PostalCodesData.Prefectures.ContainsKey(prefectureCode)) return Enumerable.Empty<City>();

            var prefecture = PostalCodesData.Prefectures[prefectureCode];
            return PostalCodesData.PrefectureCitiesMap[prefecture];
        }
    }
}
