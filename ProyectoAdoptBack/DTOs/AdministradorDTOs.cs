namespace ProyectoAdoptBack.DTOs
{
    public class AdministradorDTO
    {
        public int AdministradorID { get; set; }
        public int UsuarioID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
    }

    public class CreateAdministradorDTO
    {
        public int UsuarioID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
    }

    public class UpdateAdministradorDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
    }
}
