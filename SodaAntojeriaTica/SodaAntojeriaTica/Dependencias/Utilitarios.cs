namespace SodaAntojeriaTica.Dependencias
{
    public class Utilitarios
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

        public HttpResponseMessage ConsultarInfoUsuarios(int id)
        {
            var url = _configuration["UrlApi"] + "api/Usuarios/ListarUsuarios";
            var client = _httpClient.CreateClient();
            return client.GetAsync(url).Result;
        }
    }
}
