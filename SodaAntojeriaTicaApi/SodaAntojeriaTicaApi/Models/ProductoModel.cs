namespace SodaAntojeriaTicaApi.Models
{
    public class ProductoModel
    {
        public long? Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal? Precio { get; set; }
        public int CategoriaId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
