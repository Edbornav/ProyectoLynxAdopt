namespace ProyectoAdoptBack.DTOs
{
    public class CitasDTO
    {
        public int CitaID { get; set; }
        public int SolicitudID { get; set; }
        public DateTime FechaHoraCita { get; set; }
        public string EstadoCita { get; set; } = string.Empty;
    }

    public class CreateCitasDTO
    {
        public int SolicitudID { get; set; }
        public DateTime FechaHoraCita { get; set; }
        public string EstadoCita { get; set; } = string.Empty;
    }

    public class UpdateCitasDTO
    {
        public DateTime FechaHoraCita { get; set; }
        public string EstadoCita { get; set; } = string.Empty;
    }
}
