namespace ProyectoAdoptBack.Models
{
    public class Animales
    {
        public int AnimalID { get; set; }
        public int RefugioID { get; set; }
        public int EspecieID { get; set; }
        public int RazaID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Edad { get; set; }
        public string Sexo { get; set; } = string.Empty;
        public string Tamaño { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaIngreso { get; set; }
    }
}
