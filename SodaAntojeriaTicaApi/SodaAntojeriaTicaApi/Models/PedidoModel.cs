namespace SodaAntojeriaTicaApi.Models
{
    public class PedidoModel
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public int? EmployeeId { get; set; }
        public string OrderType { get; set; } // 'Mesa' or 'Domicilio'
        public int? Mesa { get; set; }
        public string DireccionEntrega { get; set; }
        public DateTime? FechaPedido { get; set; } = DateTime.Now;
        public string Estado { get; set; }     // 'Pendiente','En preparacion','Entregado','Pagado', etc.
        public decimal? Total { get; set; }
    }
}
