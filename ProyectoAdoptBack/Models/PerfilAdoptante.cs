namespace ProyectoAdoptBack.Models
{
    public class PerfilAdoptante
    {
        public int PerfilID { get; set; }
        public int AdoptanteID { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string Ocupacion { get; set; } = string.Empty;
        public decimal Ingresos { get; set; }
        public string TipoVivienda { get; set; } = string.Empty;
        public bool TienePatio { get; set; }
        public bool OtrosAnimales { get; set; }
        public string ExperienciaPrevia { get; set; } = string.Empty;
        public string MotivoAdopcion { get; set; } = string.Empty;
    }
}
