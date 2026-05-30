namespace ProyectoAdoptBack.Models
{
    public class Adoptante
    {
        public int AdoptanteID { get; set; }
        public int UsuarioID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }
    }
}
