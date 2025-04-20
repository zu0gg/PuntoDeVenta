namespace SodaAntojeriaTicaApi.Models
{
    public class CategoriaModel
    {
        public long? Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
