using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SodaAntojeriaTica.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SodaAntojeriaTica.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;
        public LoginController(IHttpClientFactory httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        #region RegistrarCuenta
        [HttpGet]
        public IActionResult RegistrarCuenta()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegistrarCuenta(UsuarioModel model)
        {
            model.PasswordHash = Encrypt(model.PasswordHash!);

            using (var api = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlApi").Value + "Login/RegistrarCuenta";
                var response = api.PostAsJsonAsync(url, model).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

                    if (result != null && result.Indicador)
                        return RedirectToAction("IniciarSesion", "Login");
                    else
                        ViewBag.Msj = result!.Mensaje;
                }
                else
                    ViewBag.Msj = "No se pudo completar su petición";
            }

            return View();
        }
        #endregion

        #region IniciarSesion
        [HttpGet]
        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IniciarSesion(UsuarioModel model)
        {
            model.PasswordHash = Encrypt(model.PasswordHash!);

            using (var client = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlApi").Value + "Login/IniciarSesion";
                var response = client.PostAsJsonAsync(url, model).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadFromJsonAsync<RespuestaModel>().Result;

                    if (result != null && result.Indicador)
                    {
                        var datosResult = JsonSerializer.Deserialize<UsuarioModel>((JsonElement)result.Datos!);

                        // Verifica que HttpContext.Session no sea nulo antes de usarlo
                        if (HttpContext.Session != null)
                        {
                            HttpContext.Session.SetString("Username", datosResult!.Username!);
                            if (datosResult.Rol != null)
                            {
                                HttpContext.Session.SetString("NombrePerfil", datosResult.Rol.Name!);
                            }
                            HttpContext.Session.SetString("RoleId", datosResult.RoleId.ToString());
                        }

                        return RedirectToAction("Principal", "Login");
                    }
                    else
                    {
                        ViewBag.Msj = result!.Mensaje;
                    }
                }
                else
                {
                    ViewBag.Msj = "No se pudo completar su petición";
                }
            }

            return View();
        }
        #endregion

        #region Principal
        [HttpGet]
        public IActionResult Principal()
        {
            return View();
        }
        #endregion

        #region Encrypt/Decrypt
        private string Encrypt(string texto)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_configuration.GetSection("Variables:llaveCifrado").Value!);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(texto);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public string Decrypt(string texto)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(texto);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_configuration.GetSection("Variables:llaveCifrado").Value!);
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
        #endregion
    }
}
