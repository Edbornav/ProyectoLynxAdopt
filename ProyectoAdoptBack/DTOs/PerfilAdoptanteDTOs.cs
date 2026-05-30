namespace ProyectoAdoptBack.DTOs
{
    public class PerfilAdoptanteDTO
    {
        public int PerfilAdoptanteID { get; set; }
        public int AdoptanteUsuarioID { get; set; }
        public string DescripcionCasa { get; set; } = string.Empty;
        public string DescripcionMascotas { get; set; } = string.Empty;
        public string DescripcionExperienciaConMascotas { get; set; } = string.Empty;
    }

    public class CreatePerfilAdoptanteDTO
    {
        public int AdoptanteUsuarioID { get; set; }
        public string DescripcionCasa { get; set; } = string.Empty;
        public string DescripcionMascotas { get; set; } = string.Empty;
        public string DescripcionExperienciaConMascotas { get; set; } = string.Empty;
    }

    public class UpdatePerfilAdoptanteDTO
    {
        public string DescripcionCasa { get; set; } = string.Empty;
        public string DescripcionMascotas { get; set; } = string.Empty;
        public string DescripcionExperienciaConMascotas { get; set; } = string.Empty;
    }
}
