using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace PostalCodesWebApi.Controllers
{
    public static class ControllerHelper
    {
        public static IActionResult OkOrNotFound(this Controller controller, object value)
        {
            if (value == null) return controller.NotFound();
            return controller.Ok(value);
        }

        public static IActionResult OkOrTooLarge<T>(this Controller controller, IEnumerable<T> value, int maxCount)
        {
            var result = value.ToArray();

            if (result.Length > maxCount) return controller.BadRequest();
            return controller.Ok(result);
        }
    }
}
