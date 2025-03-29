using SodaAntojeriaTica.Models;

namespace SodaAntojeriaTica.Dependencias
{
    public interface IUtilitarios
    {
        HttpResponseMessage ConsultarInfoUsuarios(long Id);
    }
}
