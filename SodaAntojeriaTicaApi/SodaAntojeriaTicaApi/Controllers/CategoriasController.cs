using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SodaAntojeriaTicaApi.Models;
using System.Data;

namespace SodaAntojeriaTicaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public CategoriaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region InsertarCategoria
        [HttpPost]
        [Route("InsertarCategoria")]
        public IActionResult InsertarCategoria(CategoriaModel model)
        {
            var respuesta = new RespuestaModel();

            try
            {
                using (var connection = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
                {
                    // Verificar si ya existe una categoría con el mismo nombre (búsqueda insensible a mayúsculas)
                    var categorias = connection.Query<CategoriaModel>("ListarCategorias", commandType: CommandType.StoredProcedure).ToList();
                    if (categorias.Any(c => c.Nombre.Equals(model.Nombre, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        respuesta.Indicador = false;
                        respuesta.Mensaje = "La categoría que intenta ingresar ya existe.";
                        return Ok(respuesta);
                    }

                    // Configurar los parámetros para el stored procedure usando DynamicParameters
                    var parameters = new DynamicParameters();
                    parameters.Add("@Nombre", model.Nombre, DbType.String, size: 50);
                    parameters.Add("@Descripcion", model.Descripcion, DbType.String, size: 255);
                    parameters.Add("@NuevoId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    // Ejecutar el SP InsertarCategoria
                    var result = connection.Execute("InsertarCategoria", parameters, commandType: CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        respuesta.Indicador = false;
                        respuesta.Mensaje = "Error al insertar la categoría.";
                    }
                    else
                    {
                        respuesta.Indicador = true;
                        respuesta.Mensaje = "Categoría agregada exitosamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }
        #endregion

        #region ListarCategorias
        [HttpGet]
        [Route("ListarCategorias")]
        public IActionResult ListarCategorias()
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                var lista = conn.Query(
                    "ListarCategorias",
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Datos = lista;
            }
            catch (Exception ex)
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }
        #endregion

        #region ObtenerCategoriaId
        [HttpGet]
        [Route("ObtenerCategoriaPorId/{id}")]
        public IActionResult ObtenerCategoriaPorId(int id)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                var categoria = conn.QueryFirstOrDefault<CategoriaModel>(
                    "ObtenerCategoriaPorId",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure);

                if (categoria == null)
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "Catgeoria no encontrado";
                }
                else
                {
                    respuesta.Indicador = true;
                    respuesta.Datos = categoria;
                }
            }
            catch (Exception ex)
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }
        #endregion

        #region ActualziarCategoria
        [HttpPut]
        [Route("ActualizarCategoria")]
        public IActionResult ActualizarCategoria(CategoriaModel model)
        {
            var respuesta = new RespuestaModel();

            using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
            var result = conn.Execute("ActualizarCategoria",
                new
                {
                    Id = model.Id,
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion
                },
                commandType: CommandType.StoredProcedure);

            if (result > 0)
            {
                respuesta.Indicador = true;
                respuesta.Mensaje = "No se actualizó la información correctamente";
            }
            else
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = "Información actualizada";
            }

            return Ok(respuesta);
        }
        #endregion

        #region EliminarCategoria
        [HttpDelete]
        [Route("EliminarCategoria/{id}")]
        public IActionResult EliminarUsuario(int id)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                conn.Execute("EliminarCategoria",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Mensaje = "Catgeoria eliminada correctamente";
            }
            catch (Exception ex)
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }
        #endregion
    }
}

