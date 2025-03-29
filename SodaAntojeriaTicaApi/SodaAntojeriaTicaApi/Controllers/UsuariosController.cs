using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SodaAntojeriaTicaApi.Models;
using System.Data;

namespace SodaAntojeriaTicaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public UsuariosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("InsertarUsuario")]
        public IActionResult InsertarUsuario(UsuarioModel model)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                conn.Execute("InsertarUsuario",
                    new
                    {
                        model.Username,
                        model.Email,
                        model.PasswordHash,
                        model.RoleId
                    },
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Mensaje = "Usuario insertado correctamente";
            }
            catch (Exception ex)
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        [HttpGet]
        [Route("ListarUsuarios")]
        public IActionResult ListarUsuarios()
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                var lista = conn.Query(
                    "ListarUsuarios",
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
        [Route("ObtenerUsuarioPorId/{id}")]
        public IActionResult ObtenerUsuarioPorId(int id)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                var usuario = conn.QueryFirstOrDefault<UsuarioModel>(
                    "ObtenerUsuarioPorId",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure);

                if (usuario == null)
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "Usuario no encontrado";
                }
                else
                {
                    respuesta.Indicador = true;
                    respuesta.Datos = usuario;
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
        [Route("ActualizarUsuario/{id}")]
        public IActionResult ActualizarUsuario(int id, UsuarioModel model)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                conn.Execute("ActualizarUsuario",
                    new
                    {
                        Id = id,
                        model.Username,
                        model.Email,
                        model.PasswordHash,
                        model.RoleId
                    },
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Mensaje = "Usuario actualizado correctamente";
            }
            catch (Exception ex)
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        [HttpDelete]
        [Route("EliminarUsuario/{id}")]
        public IActionResult EliminarUsuario(int id)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
                conn.Execute("EliminarUsuario",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure);

                respuesta.Indicador = true;
                respuesta.Mensaje = "Usuario eliminado correctamente";
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
