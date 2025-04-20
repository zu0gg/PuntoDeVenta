namespace SodaAntojeriaTica.Models
{
    public class UsuarioModel
    {
        public long? Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public long? RoleId { get; set; }
        public RolModel? Rol { get; set; }
        public DateTime? DayCreate { get; set; } = DateTime.Now;
    }
}
