namespace ProyectoAdoptBack.DTOs
{
    public class AnimalesDTO
    {
        public int AnimalID { get; set; }
        public int RefugioID { get; set; }
        public int RazaID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Sexo { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string Estatus { get; set; } = string.Empty;
        public DateTime? FechaRegistro { get; set; }
        public string? FotoUrl { get; set; }
    }

    public class CreateAnimalesDTO
    {
        public int RefugioID { get; set; }
        public int RazaID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Sexo { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string Estatus { get; set; } = string.Empty;
    }

    public class UpdateAnimalesDTO
    {
        public int RazaID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Sexo { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string Estatus { get; set; } = string.Empty;
    }
}
