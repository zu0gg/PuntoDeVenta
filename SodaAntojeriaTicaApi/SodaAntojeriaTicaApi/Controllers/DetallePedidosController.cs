using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SodaAntojeriaTicaApi.Models;
using System.Data;

namespace SodaAntojeriaTicaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallePedidosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DetallePedidosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // CREATE
        [HttpPost]
        [Route("InsertarDetallePedido")]
        public IActionResult InsertarDetallePedido(DetallePedidoModel model)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                conn.Execute("InsertarDetallePedido",
                    new
                    {
                        model.PedidoId,
                        model.ProductoId,
                        model.Cantidad,
                        model.PrecioUnitario
                    },
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Mensaje = "Detalle insertado correctamente";
            }
            catch (Exception ex)
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        // READ ALL
        [HttpGet]
        [Route("ListarDetallePedidos")]
        public IActionResult ListarDetallePedidos()
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                var detalles = conn.Query<DetallePedidoModel>(
                    "ListarDetallePedidos",
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Datos = detalles;
            }
            catch (Exception ex)
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        // READ by ID
        [HttpGet]
        [Route("ObtenerDetallePedidoPorId/{id}")]
        public IActionResult ObtenerDetallePedidoPorId(int id)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                var detalle = conn.QueryFirstOrDefault<DetallePedidoModel>(
                    "ObtenerDetallePedidoPorId",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure);

                if (detalle == null)
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "Detalle de pedido no encontrado";
                }
                else
                {
                    respuesta.Indicador = true;
                    respuesta.Datos = detalle;
                }
            }
            catch (Exception ex)
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        // UPDATE
        [HttpPut]
        [Route("ActualizarDetallePedido/{id}")]
        public IActionResult ActualizarDetallePedido(int id, DetallePedidoModel model)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                conn.Execute("ActualizarDetallePedido",
                    new
                    {
                        Id = id,
                        model.PedidoId,
                        model.ProductoId,
                        model.Cantidad,
                        model.PrecioUnitario
                    },
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Mensaje = "Detalle actualizado correctamente";
            }
            catch (Exception ex)
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        // DELETE
        [HttpDelete]
        [Route("EliminarDetallePedido/{id}")]
        public IActionResult EliminarDetallePedido(int id)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                conn.Execute("EliminarDetallePedido",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Mensaje = "Detalle eliminado correctamente";
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
