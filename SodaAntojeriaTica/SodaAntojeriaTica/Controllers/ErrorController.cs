using Microsoft.AspNetCore.Mvc;

namespace SodaAntojeriaTica.Controllers
{
    public class ErrorController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult CapturarError()
        {
            return View("Error");
        }
    }
}
