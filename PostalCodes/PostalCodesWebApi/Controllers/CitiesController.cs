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
        /// 都道府県コード (2 桁) を指定して、市区町村のリストを取得します。
        /// </summary>
        /// <param name="prefCode">都道府県コード (2 桁)</param>
        /// <returns>市区町村のリスト</returns>
        [HttpGet("ByPref/{prefCode:regex(^[[0-9]]{{2}}$)}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<City>))]
        [ProducesResponseType(404)]
        public IActionResult GetByPref(string prefCode)
        {
            if (!PostalCodesData.Prefs.ContainsKey(prefCode)) return NotFound();

            var pref = PostalCodesData.Prefs[prefCode];
            return Ok(PostalCodesData.PrefCitiesMap[pref]);
        }

        /// <summary>
        /// 市区町村コード (5 桁) を指定して、市区町村を取得します。
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
        /// 市区町村の名前を指定して、市区町村のリストを取得します。部分一致検索です。
        /// </summary>
        /// <param name="name">市区町村の名前</param>
        /// <returns>市区町村のリスト</returns>
        [HttpGet("ByName/{name}")]
        public IEnumerable<City> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return Enumerable.Empty<City>();

            return PostalCodesData.Cities.Values.Where(x => x.Name.Contains(name));
        }

        /// <summary>
        /// 市区町村のかなを指定して、市区町村のリストを取得します。部分一致検索です。
        /// </summary>
        /// <param name="kana">市区町村のかな</param>
        /// <returns>市区町村のリスト</returns>
        [HttpGet("ByKana/{kana}")]
        public IEnumerable<City> GetByKana(string kana)
        {
            if (string.IsNullOrWhiteSpace(kana)) return Enumerable.Empty<City>();

            return PostalCodesData.Cities.Values.Where(x => x.Kana.Contains(kana));
        }
    }
}
