namespace ProyectoAdoptBack.Models
{
    public class Citas
    {
        public int CitaID { get; set; }
        public int SolicitudID { get; set; }
        public DateTime FechaHoraCita { get; set; }
        public string EstadoCita { get; set; } = string.Empty;
    }
}
