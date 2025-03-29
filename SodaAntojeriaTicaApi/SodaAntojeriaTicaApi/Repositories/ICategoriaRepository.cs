using SodaAntojeriaTicaApi.Models;

namespace SodaAntojeriaTicaApi.Repositories
{
    public interface ICategoriaRepository
    {
        Task<int> InsertarCategoria(CategoriaModel categoria);
        Task<IEnumerable<CategoriaModel>> ListarCategorias();
        Task<CategoriaModel?> ObtenerCategoriaPorId(int id);
        Task ActualizarCategoria(CategoriaModel categoria);
        Task EliminarCategoria(int id);
    }
}
