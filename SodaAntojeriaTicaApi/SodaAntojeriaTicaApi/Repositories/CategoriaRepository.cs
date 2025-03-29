using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using SodaAntojeriaTicaApi.Models;

namespace SodaAntojeriaTicaApi.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly IDbConnection _db;

        public CategoriaRepository(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("BDConnection");
            _db = new SqlConnection(connectionString);
        }

        public async Task<int> InsertarCategoria(CategoriaModel categoria)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Nombre", categoria.Nombre);
            parameters.Add("@Descripcion", categoria.Descripcion);
            parameters.Add("@NuevoId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _db.ExecuteAsync("InsertarCategoria", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@NuevoId");
        }

        public async Task<IEnumerable<CategoriaModel>> ListarCategorias()
        {
            return await _db.QueryAsync<CategoriaModel>("ListarCategorias", commandType: CommandType.StoredProcedure);
        }

        public async Task<CategoriaModel?> ObtenerCategoriaPorId(int id)
        {
            var parameters = new { Id = id };
            return await _db.QueryFirstOrDefaultAsync<CategoriaModel>("ObtenerCategoriaPorId", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task ActualizarCategoria(CategoriaModel categoria)
        {
            var parameters = new
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion
            };

            await _db.ExecuteAsync("ActualizarCategoria", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task EliminarCategoria(int id)
        {
            var parameters = new { Id = id };
            await _db.ExecuteAsync("EliminarCategoria", parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
