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
        /// すべての都道府県のリストを取得します。
        /// </summary>
        /// <returns>都道府県のリスト</returns>
        [HttpGet]
        public IEnumerable<Pref> Get()
        {
            return PostalCodesData.Prefs.Values;
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
            if (!PostalCodesData.Prefs.ContainsKey(code)) return NotFound();

            return Ok(PostalCodesData.Prefs[code]);
        }

        /// <summary>
        /// 都道府県の名前を指定して、都道府県のリストを取得します。部分一致検索です。
        /// </summary>
        /// <param name="name">都道府県の名前</param>
        /// <returns>都道府県のリスト</returns>
        [HttpGet("ByName/{name}")]
        public IEnumerable<Pref> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return Enumerable.Empty<Pref>();

            return PostalCodesData.Prefs.Values.Where(x => x.Name.Contains(name));
        }

        /// <summary>
        /// 都道府県のかなを指定して、都道府県のリストを取得します。部分一致検索です。
        /// </summary>
        /// <param name="kana">都道府県のかな</param>
        /// <returns>都道府県のリスト</returns>
        [HttpGet("ByKana/{kana}")]
        public IEnumerable<Pref> GetByKana(string kana)
        {
            if (string.IsNullOrWhiteSpace(kana)) return Enumerable.Empty<Pref>();

            return PostalCodesData.Prefs.Values.Where(x => x.Kana.Contains(kana));
        }
    }
}
