using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SodaAntojeriaTicaApi.Models;
using System.Data;

namespace SodaAntojeriaTicaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public CategoriasController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("InsertarCategoria")]
        public IActionResult InsertarCategoria(CategoriaModel model)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                conn.Execute("InsertarCategoria",
                    new { model.Nombre, model.Descripcion },
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Mensaje = "Categoria insertada correctamente";
            }
            catch (Exception ex)
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        [HttpGet]
        [Route("ListarCategorias")]
        public IActionResult ListarCategorias()
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                var lista = conn.Query<CategoriaModel>(
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

        [HttpGet]
        [Route("ObtenerCategoriaPorId/{id}")]
        public IActionResult ObtenerCategoriaPorId(int id)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                var cat = conn.QueryFirstOrDefault<CategoriaModel>(
                    "ObtenerCategoriaPorId",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure);

                if (cat == null)
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "Categoría no encontrada";
                }
                else
                {
                    respuesta.Indicador = true;
                    respuesta.Datos = cat;
                }
            }
            catch (Exception ex)
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        [HttpPut]
        [Route("ActualizarCategoria/{id}")]
        public IActionResult ActualizarCategoria(int id, CategoriaModel model)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                conn.Execute("ActualizarCategoria",
                    new
                    {
                        Id = id,
                        model.Nombre,
                        model.Descripcion
                    },
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Mensaje = "Categoría actualizada correctamente";
            }
            catch (Exception ex)
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        [HttpDelete]
        [Route("EliminarCategoria/{id}")]
        public IActionResult EliminarCategoria(int id)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                conn.Execute("EliminarCategoria",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Mensaje = "Categoría eliminada correctamente";
            }
            catch (Exception ex)
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }
    }
}
