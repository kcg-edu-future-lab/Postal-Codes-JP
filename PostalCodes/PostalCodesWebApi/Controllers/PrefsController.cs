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
    /// 都道府県のデータを取得します。
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PrefsController : Controller
    {
        /// <summary>
        /// 都道府県の名前、かなを指定して、都道府県のリストを取得します。部分一致検索です。
        /// </summary>
        /// <param name="name">都道府県の名前</param>
        /// <param name="kana">都道府県のかな</param>
        /// <returns>都道府県のリスト</returns>
        [HttpGet]
        public IEnumerable<Pref> Get(string name, string kana)
        {
            IEnumerable<Pref> query = PostalCodesData.Prefs.Values;

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.Name.Contains(name));
            if (!string.IsNullOrWhiteSpace(kana))
                query = query.Where(x => x.Kana.Contains(kana));

            return query;
        }

        /// <summary>
        /// 都道府県コード (2 桁) を指定して、都道府県を取得します。
        /// </summary>
        /// <param name="code">都道府県コード (2 桁)</param>
        /// <returns>都道府県</returns>
        [HttpGet("{code:regex(^[[0-9]]{{2}}$)}")]
        [ProducesResponseType(200, Type = typeof(Pref))]
        [ProducesResponseType(404)]
        public IActionResult Get(string code)
        {
            var result = Get0(code);
            if (result == null) return NotFound();
            return Ok(result);
        }

        Pref Get0(string code) => PostalCodesData.Prefs.ContainsKey(code) ? PostalCodesData.Prefs[code] : null;
    }
}
