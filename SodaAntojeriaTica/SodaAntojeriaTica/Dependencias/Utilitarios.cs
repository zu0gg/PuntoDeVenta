using Microsoft.Extensions.Configuration;
using SodaAntojeriaTica.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;

namespace SodaAntojeriaTica.Dependencias
{
    public class Utilitarios : IUtilitarios
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _accessor;
        public Utilitarios(IHttpClientFactory httpClient, IConfiguration configuration, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _accessor = accessor;
        }

        public HttpResponseMessage ConsultarUsuario(long Id)
        {
            using (var api = _httpClient.CreateClient())
            {
                var url = _configuration.GetSection("Variables:urlApi").Value + "Usuarios/ListarUsuarios?Id=" + Id;
                var response = api.GetAsync(url).Result;

                return response;
            }
        }
    }
}
