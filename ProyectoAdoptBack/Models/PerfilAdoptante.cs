namespace ProyectoAdoptBack.Models
{
    public class PerfilAdoptante
    {
        public int PerfilAdoptanteID { get; set; }
        public int AdoptanteUsuarioID { get; set; }
        public string DescripcionCasa { get; set; } = string.Empty;
        public string DescripcionMascotas { get; set; } = string.Empty;
        public string DescripcionExperienciaConMascotas { get; set; } = string.Empty;
    }
}
