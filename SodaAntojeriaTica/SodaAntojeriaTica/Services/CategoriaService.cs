using System.Net.Http.Json;
using SodaAntojeriaTica.Models;

namespace SodaAntojeriaTica.Services
{
    public class CategoriaService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CategoriaService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            var baseUrl = _configuration["Variables:urlApi"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentNullException(nameof(baseUrl), "La URL base no puede ser nula o vacía.");
            }
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<IEnumerable<CategoriaModel>> GetCategoriasAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<CategoriaModel>>("Categoria");
        }

        public async Task<CategoriaModel> GetCategoriaByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<CategoriaModel>($"Categoria/{id}");
        }

        public async Task<CategoriaModel> CreateCategoriaAsync(CategoriaModel categoria)
        {
            var response = await _httpClient.PostAsJsonAsync("Categoria", categoria);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CategoriaModel>();
        }

        public async Task UpdateCategoriaAsync(int id, CategoriaModel categoria)
        {
            var response = await _httpClient.PutAsJsonAsync($"Categoria/{id}", categoria);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCategoriaAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Categoria/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
