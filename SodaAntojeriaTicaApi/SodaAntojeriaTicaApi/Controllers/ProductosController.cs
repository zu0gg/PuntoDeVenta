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

        #region InsertarProducto
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
        #endregion

        #region ListarProductos
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
        #endregion

        #region ObtenerProductoPorId
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
        #endregion

        #region ActualizarProducto
        [HttpPut]
        [Route("ActualizarProducto")]
        public IActionResult ActualizarProducto(ProductoModel model)
        {
            var respuesta = new RespuestaModel();

            using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
            var result = conn.Execute(
                "ActualizarProducto",
                new
                {
                    Id = model.Id,
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion,
                    Precio = model.Precio,
                    CategoriaId = model.CategoriaId,
                    IsActive = model.IsActive
                },
                commandType: CommandType.StoredProcedure
            );

            if (result > 0)
            {
                respuesta.Indicador = true;
                respuesta.Mensaje = "Información actualizada";
            }
            else
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = "No se actualizó la información correctamente";
            }

            return Ok(respuesta);
        }
        #endregion

        #region EliminarProducto
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
        #endregion
    }
}
