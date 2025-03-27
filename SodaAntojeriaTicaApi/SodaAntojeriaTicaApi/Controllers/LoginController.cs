using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SodaAntojeriaTicaApi.Models;

namespace SodaAntojeriaTicaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        [Route("RegistrarCuenta")]
        public IActionResult RegistrarCuenta(UsuarioModel model) 
        {
            using (var context = new SqlConnection("Server=ZU;Database=AntojeriaTicaDB;Trusted_Connection=True;TrustServerCertificate=True")) 
            {
                var result = context.Execute("RegistrarCuenta",
                    new { model.Identificacion, model.Contrasenna, model.Nombre, model.Correo });
            }

                return Ok();
        }

    }
}
