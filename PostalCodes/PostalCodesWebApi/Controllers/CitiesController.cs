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
        /// 市区町村の名前、かなを指定して、市区町村のリストを取得します。部分一致検索です。
        /// </summary>
        /// <param name="name">市区町村の名前</param>
        /// <param name="kana">市区町村のかな</param>
        /// <returns>市区町村のリスト</returns>
        [HttpGet]
        public IEnumerable<City> Get(string name, string kana)
        {
            IEnumerable<City> query = PostalCodesData.Cities.Values;

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.Name.Contains(name));
            if (!string.IsNullOrWhiteSpace(kana))
                query = query.Where(x => x.Kana.Contains(kana));

            return query;
        }

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
            var result = Get0(code);
            if (result == null) return NotFound();
            return Ok(result);
        }

        City Get0(string code) => PostalCodesData.Cities.ContainsKey(code) ? PostalCodesData.Cities[code] : null;
    }
}
