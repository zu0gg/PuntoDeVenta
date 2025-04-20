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

        #region InsertarUusario
        [HttpPost]
        [Route("InsertarUsuario")]
        public IActionResult InsertarUsuario(UsuarioModel model)
        {
            var respuesta = new RespuestaModel();

            try
            {
                using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
                {
                    var usuarios = context.Query<UsuarioModel>("ListarUsuarios", commandType: CommandType.StoredProcedure).ToList();

                    if (usuarios.Any(u => u.Email == model.Email))
                    {
                        respuesta.Indicador = false;
                        respuesta.Mensaje = "El correo ya está registrado.";
                        return Ok(respuesta);
                    }

                    var result = context.Execute("InsertarUsuario",
                        new
                        {
                            model.Username,
                            model.Email,
                            model.PasswordHash,
                            model.RoleId
                        },
                        commandType: CommandType.StoredProcedure);

                    if (result > 0)
                    {
                        respuesta.Indicador = true;
                        respuesta.Mensaje = "Usuario agregado exitosamente.";
                    }
                    else
                    {
                        respuesta.Indicador = false;
                        respuesta.Mensaje = "Error al insertar el usuario.";
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

        #region ListarUsuarios
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
        #endregion

        #region ObtenerUsuarios
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

        [HttpGet]
        [Route("ObtenerUsuarioPorNombre/{username}")]
        public IActionResult ObtenerUsuarioPorNombre(string username)
        {
            var respuesta = new RespuestaModel();
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));

                var usuarios = conn.Query<UsuarioModel>(
                    "ObtenerUsuarioPorNombre",
                    new { Username = username },
                    commandType: CommandType.StoredProcedure
                ).ToList();

                if (usuarios.Any())
                {
                    respuesta.Indicador = true;
                    respuesta.Mensaje = "Usuarios encontrados.";
                    respuesta.Datos = usuarios;
                }
                else
                {
                    respuesta.Indicador = false;
                    respuesta.Mensaje = "No se encontraron usuarios con ese nombre.";
                }
            }
            catch (Exception ex)
            {
                respuesta.Indicador = false;
                respuesta.Mensaje = "Error inesperado: " + ex.Message;
            }

            return Ok(respuesta);
        }
        #endregion

        #region ActualizarUsuario
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
        #endregion

        #region EliminarUsuario
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
        #endregion
    }
}
