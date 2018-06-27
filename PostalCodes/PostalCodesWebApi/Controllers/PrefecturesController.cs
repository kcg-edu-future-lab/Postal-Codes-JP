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
    public class PrefecturesController : Controller
    {
        /// <summary>
        /// すべての都道府県のリストを取得します。
        /// </summary>
        /// <returns>都道府県のリスト。</returns>
        [HttpGet]
        public IEnumerable<Prefecture> Get()
        {
            return PostalCodesData.Prefectures.Values;
        }

        /// <summary>
        /// 都道府県コードを指定して、都道府県を取得します。
        /// </summary>
        /// <param name="code">都道府県コード (2 桁)。</param>
        /// <returns>都道府県。</returns>
        [HttpGet("{code:regex(^[[0-9]]{{2}}$)}")]
        [ProducesResponseType(200, Type = typeof(Prefecture))]
        [ProducesResponseType(404)]
        public IActionResult Get(string code)
        {
            if (!PostalCodesData.Prefectures.ContainsKey(code)) return NotFound();

            return Ok(PostalCodesData.Prefectures[code]);
        }

        /// <summary>
        /// 名前を指定して、都道府県のリストを取得します。
        /// </summary>
        /// <param name="name">名前。</param>
        /// <returns>都道府県のリスト。</returns>
        [HttpGet("ByName/{name}")]
        public IEnumerable<Prefecture> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return Enumerable.Empty<Prefecture>();

            return PostalCodesData.Prefectures.Values.Where(x => x.Name.Contains(name));
        }

        /// <summary>
        /// かなを指定して、都道府県のリストを取得します。
        /// </summary>
        /// <param name="kana">かな。</param>
        /// <returns>都道府県のリスト。</returns>
        [HttpGet("ByKana/{kana}")]
        public IEnumerable<Prefecture> GetByKana(string kana)
        {
            if (string.IsNullOrWhiteSpace(kana)) return Enumerable.Empty<Prefecture>();

            return PostalCodesData.Prefectures.Values.Where(x => x.Kana.Contains(kana));
        }
    }
}
