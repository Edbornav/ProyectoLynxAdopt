namespace ProyectoAdoptBack.DTOs
{
    public class UsuarioDTO
    {
        public int UsuarioID { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;
        public string Estatus { get; set; } = string.Empty;
        public DateTime? FechaRegistro { get; set; }
    }

    public class CreateUsuarioDTO
    {
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;
        public string Estatus { get; set; } = string.Empty;
    }

    public class UpdateUsuarioDTO
    {
        public string Correo { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;
        public string Estatus { get; set; } = string.Empty;
    }

    public class LoginDTO
    {
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
