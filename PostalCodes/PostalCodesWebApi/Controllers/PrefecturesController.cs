using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostalCodesWebApi.Models;

namespace PostalCodesWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PrefecturesController : Controller
    {
        [HttpGet]
        public IEnumerable<Prefecture> Get()
        {
            return PostalCodesModel.Prefectures;
        }

        [HttpGet("{code:regex(^[[0-9]]{{2}}$)}")]
        [ProducesResponseType(200, Type = typeof(Prefecture))]
        [ProducesResponseType(404)]
        public IActionResult Get(string code)
        {
            if (!PostalCodesModel.PrefecturesMap.ContainsKey(code)) return NotFound();

            return Ok(PostalCodesModel.PrefecturesMap[code]);
        }

        [HttpGet("ByName/{name}")]
        public IEnumerable<Prefecture> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return Enumerable.Empty<Prefecture>();

            return PostalCodesModel.Prefectures.Where(x => x.Name.Contains(name));
        }

        [HttpGet("ByKana/{kana}")]
        public IEnumerable<Prefecture> GetByKana(string kana)
        {
            if (string.IsNullOrWhiteSpace(kana)) return Enumerable.Empty<Prefecture>();

            return PostalCodesModel.Prefectures.Where(x => x.Kana.Contains(kana));
        }
    }
}
