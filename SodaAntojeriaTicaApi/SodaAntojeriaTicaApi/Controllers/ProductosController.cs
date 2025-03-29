using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SodaAntojeriaTicaApi.Models;
using System.Data;

namespace SodaAntojeriaTicaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ProductosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("InsertarProducto")]
        public IActionResult InsertarProducto(ProductoModel model)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                conn.Execute("InsertarProducto",
                    new
                    {
                        model.Nombre,
                        model.Descripcion,
                        model.Precio,
                        model.CategoriaId
                    },
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Mensaje = "Producto insertado correctamente";
            }
            catch (Exception ex)
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        [HttpGet]
        [Route("ListarProductos")]
        public IActionResult ListarProductos()
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                var lista = conn.Query(
                    "ListarProductos",
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
        [Route("ObtenerProductoPorId/{id}")]
        public IActionResult ObtenerProductoPorId(int id)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                var producto = conn.QueryFirstOrDefault<ProductoModel>(
                    "ObtenerProductoPorId",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure);

                if (producto == null)
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "Producto no encontrado";
                }
                else
                {
                    respuesta.Indicador = true;
                    respuesta.Datos = producto;
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
        [Route("ActualizarProducto/{id}")]
        public IActionResult ActualizarProducto(int id, ProductoModel model)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                conn.Execute("ActualizarProducto",
                    new
                    {
                        Id = id,
                        model.Nombre,
                        model.Descripcion,
                        model.Precio,
                        model.CategoriaId,
                        model.IsActive
                    },
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Mensaje = "Producto actualizado correctamente";
            }
            catch (Exception ex)
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        [HttpDelete]
        [Route("EliminarProducto/{id}")]
        public IActionResult EliminarProducto(int id)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                conn.Execute("EliminarProducto",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Mensaje = "Producto eliminado correctamente";
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
