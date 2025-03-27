using Microsoft.AspNetCore.Mvc;
using SodaAntojeriaTica.Models;

namespace SodaAntojeriaTica.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        public LoginController(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        #region Registrar Cuenta

        [HttpGet]
        public IActionResult RegistrarCuenta()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegistrarCuenta(UsuarioModel model)
        {
            using (var api = _httpClient.CreateClient()) 
            {
                var url = "https://localhost:7229/api/Login/RegistrarCuenta";
                var result = api.PostAsJsonAsync(url, model).Result;

                if (result.IsSuccessStatusCode) 
                {
                    return RedirectToAction("IniciarSesion", "Login");
                }
            }

            return View();
        }


        #endregion

        [HttpGet]
        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Principal()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RecuperarContrasenna()
        {
            return View();
        }
    }
}
