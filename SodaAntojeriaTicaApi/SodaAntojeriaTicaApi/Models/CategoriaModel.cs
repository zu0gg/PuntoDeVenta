namespace SodaAntojeriaTicaApi.Models
{
    public class CategoriaModel
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
