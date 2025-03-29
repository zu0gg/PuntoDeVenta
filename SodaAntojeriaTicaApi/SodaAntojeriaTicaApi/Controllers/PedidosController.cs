using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SodaAntojeriaTicaApi.Models;
using System.Data;

namespace SodaAntojeriaTicaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PedidosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // INSERT
        [HttpPost]
        [Route("RealizarPedido")]
        public IActionResult RealizarPedido(PedidoModel model)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                var pedidoId = conn.ExecuteScalar<int>(
                    "RealizarPedido",
                    new
                    {
                        model.UserId,
                        model.OrderType,
                        model.Mesa,
                        model.DireccionEntrega,
                        model.Total
                    },
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Mensaje = "Pedido realizado correctamente";
                respuesta.Datos = new { PedidoId = pedidoId };
            }
            catch (Exception ex)
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        // GET ALL
        [HttpGet]
        [Route("ListarPedidos")]
        public IActionResult ListarPedidos()
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                var lista = conn.Query<PedidoModel>(
                    "ListarPedidos",
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

        // GET by ID
        [HttpGet]
        [Route("ObtenerPedidoPorId/{id}")]
        public IActionResult ObtenerPedidoPorId(int id)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                var pedido = conn.QueryFirstOrDefault<PedidoModel>(
                    "ObtenerPedidoPorId",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure);

                if (pedido == null)
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "Pedido no encontrado";
                }
                else
                {
                    respuesta.Indicador = true;
                    respuesta.Datos = pedido;
                }
            }
            catch (Exception ex)
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        // FULL UPDATE
        [HttpPut]
        [Route("ActualizarPedido/{id}")]
        public IActionResult ActualizarPedido(int id, PedidoModel model)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                conn.Execute("ActualizarPedido",
                    new
                    {
                        Id = id,
                        model.UserId,
                        model.EmployeeId,
                        model.OrderType,
                        model.Mesa,
                        model.DireccionEntrega,
                        model.Estado,
                        model.Total
                    },
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Mensaje = "Pedido actualizado correctamente";
            }
            catch (Exception ex)
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        // PARTIAL UPDATE (just Estado)
        [HttpPut]
        [Route("ActualizarEstadoPedido/{pedidoId}")]
        public IActionResult ActualizarEstadoPedido(int pedidoId, [FromBody] string nuevoEstado)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                conn.Execute("ActualizarEstadoPedido",
                    new { PedidoId = pedidoId, NuevoEstado = nuevoEstado },
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Mensaje = "Estado del pedido actualizado correctamente";
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
        [Route("EliminarPedido/{id}")]
        public IActionResult EliminarPedido(int id)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                conn.Execute("EliminarPedido",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Mensaje = "Pedido eliminado correctamente";
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
