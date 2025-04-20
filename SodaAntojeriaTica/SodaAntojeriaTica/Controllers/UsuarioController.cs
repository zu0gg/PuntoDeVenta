using Microsoft.AspNetCore.Mvc;
using SodaAntojeriaTica.Dependencias;
using SodaAntojeriaTica.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SodaAntojeriaTica.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IUtilitarios _utilitarios;
        public UsuarioController(IHttpClientFactory httpClient, IConfiguration configuration, IUtilitarios utilitarios)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _utilitarios = utilitarios;
        }

        #region ConsultarUsuario
        [HttpGet]
        public IActionResult ConsultarUsuario()
        {
            var response = _utilitarios.ConsultarUsuario(0);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

                if (result != null && result.Indicador)
                {
                    var datosResult = JsonSerializer.Deserialize<List<UsuarioModel>>((JsonElement)result.Datos!);
                    return View(datosResult);
                }
                else
                    ViewBag.Msj = result!.Mensaje;
            }
            else
                ViewBag.Msj = "No se pudo completar su petición";

            return View(new List<UsuarioModel>());
        }
        #endregion

        #region ActulaizarUsuario

        #endregion
    }
}
