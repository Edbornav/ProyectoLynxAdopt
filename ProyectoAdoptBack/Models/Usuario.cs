namespace ProyectoAdoptBack.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string Email { get; set; } = string.Empty;
        public string ContrasenaHash { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
    }
}
