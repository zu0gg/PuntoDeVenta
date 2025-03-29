namespace SodaAntojeriaTicaApi.Models
{
    public class VentaModel
    {
        public int? Id { get; set; }
        public int PedidoId { get; set; }
        public DateTime? FechaVenta { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public string MetodoPago { get; set; }  // e.g. 'Sinpe', 'Tarjeta', 'Efectivo'
    }
}
