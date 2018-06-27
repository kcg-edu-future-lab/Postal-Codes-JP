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
        public Prefecture Get(string code)
        {
            throw new NotImplementedException();
        }

        [HttpGet("ByName/{name}")]
        public IEnumerable<Prefecture> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        [HttpGet("ByKana/{kana}")]
        public IEnumerable<Prefecture> GetByKana(string kana)
        {
            throw new NotImplementedException();
        }
    }
}
