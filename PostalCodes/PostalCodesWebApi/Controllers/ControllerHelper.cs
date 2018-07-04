using System;
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
    }
}
