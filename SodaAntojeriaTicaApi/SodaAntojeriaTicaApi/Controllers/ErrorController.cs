using Microsoft.AspNetCore.Mvc;

namespace SodaAntojeriaTicaApi.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult CapturarError()
        {
            return View("Error");
        }
    }
}
