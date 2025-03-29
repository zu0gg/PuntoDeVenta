using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SodaAntojeriaTicaApi.Models;
using System.Data;

namespace SodaAntojeriaTicaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public VentasController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // INSERT
        [HttpPost]
        [Route("RegistrarVenta")]
        public IActionResult RegistrarVenta(VentaModel model)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                conn.Execute("RegistrarVenta",
                    new
                    {
                        model.PedidoId,
                        model.MetodoPago,
                        model.Total
                    },
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Mensaje = "Venta registrada correctamente";
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
        [Route("ListarVentas")]
        public IActionResult ListarVentas()
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                var ventas = conn.Query<VentaModel>(
                    "ListarVentas",
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Datos = ventas;
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
        [Route("ObtenerVentaPorId/{id}")]
        public IActionResult ObtenerVentaPorId(int id)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                var venta = conn.QueryFirstOrDefault<VentaModel>(
                    "ObtenerVentaPorId",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure);

                if (venta == null)
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "Venta no encontrada";
                }
                else
                {
                    respuesta.Indicador = true;
                    respuesta.Datos = venta;
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
        [Route("ActualizarVenta/{id}")]
        public IActionResult ActualizarVenta(int id, VentaModel model)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                conn.Execute("ActualizarVenta",
                    new
                    {
                        Id = id,
                        model.PedidoId,
                        model.FechaVenta,
                        model.MetodoPago,
                        model.Total
                    },
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Mensaje = "Venta actualizada correctamente";
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
        [Route("EliminarVenta/{id}")]
        public IActionResult EliminarVenta(int id)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                conn.Execute("EliminarVenta",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Mensaje = "Venta eliminada correctamente";
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
