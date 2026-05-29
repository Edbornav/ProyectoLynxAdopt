namespace ProyectoAdoptBack.Models
{
    public class Citas
    {
        public int CitaID { get; set; }
        public int SolicitudID { get; set; }
        public DateTime FechaCita { get; set; }
        public string HoraCita { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Notas { get; set; } = string.Empty;
    }
}
