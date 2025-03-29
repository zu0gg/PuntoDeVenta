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

        #region ConsultarUsuarios
        [HttpGet]
        public IActionResult ConsultarUsuarios()
        {
            var response = _utilitarios.ConsultarInfoUsuarios(0);

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
        [HttpGet]
        public IActionResult ActualizarDatos()
        {
            // Se obtiene el Id del usuario actual desde la sesión (asegúrate de haberlo almacenado al iniciar sesión)
            var userIdString = HttpContext.Session.GetString("UserId");
            int userId = 0;
            if (!string.IsNullOrEmpty(userIdString))
            {
                userId = int.Parse(userIdString);
            }
            else
            {
                ViewBag.Msj = "No se encontró el usuario en sesión.";
                return View(new UsuarioModel());
            }

            using (var client = _httpClient.CreateClient())
            {
                // Se construye la URL para llamar al SP ObtenerUsuarioPorId (ejemplo: .../Usuarios/ObtenerUsuarioPorId/1)
                var url = _configuration.GetSection("Variables:urlApi").Value + "Usuarios/ObtenerUsuarioPorId/" + userId;

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));

                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;
                    if (result != null && result.Indicador)
                    {
                        var datos = JsonSerializer.Deserialize<UsuarioModel>((JsonElement)result.Datos!)!;
                        return View(datos);
                    }
                    else
                    {
                        ViewBag.Msj = result?.Mensaje;
                    }
                }
                else
                {
                    ViewBag.Msj = "No se pudo completar su petición";
                }
            }
            return View(new UsuarioModel());
        }

        [HttpPost]
        public IActionResult ActualizarDatos(UsuarioModel model)
        {
            // Se asume que model.Id contiene el Id del usuario a actualizar
            if (model.Id == null)
            {
                ViewBag.Msj = "No se especificó el usuario a actualizar.";
                return View(model);
            }

            using (var client = _httpClient.CreateClient())
            {
                // Se construye la URL para llamar al SP ActualizarUsuario, incluyendo el Id en la ruta (por ejemplo: .../Usuarios/ActualizarUsuario/1)
                var url = _configuration.GetSection("Variables:urlApi").Value + "Usuarios/ActualizarUsuario/" + model.Id;

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));

                var response = client.PutAsJsonAsync(url, model).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;
                    if (result != null && result.Indicador)
                    {
                        // Actualizamos el nombre del usuario en sesión
                        HttpContext.Session.SetString("Username", model.Username);
                        return RedirectToAction("ActualizarDatos", "Usuarios");
                    }
                    else
                    {
                        ViewBag.Msj = result?.Mensaje;
                    }
                }
                else
                {
                    ViewBag.Msj = "No se pudo completar su petición";
                }
            }
            return View(model);
        }


        #endregion
    }
}
