namespace ProyectoAdoptBack.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;
        public string Estatus { get; set; } = string.Empty;
        public DateTime? FechaRegistro { get; set; }
    }
}
