namespace ProyectoAdoptBack.DTOs
{
    public class SolicitudAdopcionDTO
    {
        public int SolicitudID { get; set; }
        public int RefugioID { get; set; }
        public int AdoptanteID { get; set; }
        public string MensajeAdoptante { get; set; } = string.Empty;
        public string Estatus { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
    }

    public class CreateSolicitudAdopcionDTO
    {
        public int RefugioID { get; set; }
        public int AdoptanteID { get; set; }
        public string MensajeAdoptante { get; set; } = string.Empty;
    }

    public class UpdateSolicitudAdopcionDTO
    {
        public string Estatus { get; set; } = string.Empty;
    }
}
