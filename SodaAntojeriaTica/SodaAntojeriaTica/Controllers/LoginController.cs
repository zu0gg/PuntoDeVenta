using Microsoft.AspNetCore.Mvc;

namespace SodaAntojeriaTica.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult IniciarSesion()
        {
            return View();
        }

        public IActionResult Principal()
        {
            return View();
        }
    }
}
