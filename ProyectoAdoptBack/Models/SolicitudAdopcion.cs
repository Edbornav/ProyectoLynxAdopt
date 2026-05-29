namespace ProyectoAdoptBack.Models
{
    public class SolicitudAdopcion
    {
        public int SolicitudID { get; set; }
        public int AdoptanteID { get; set; }
        public int AnimalID { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
