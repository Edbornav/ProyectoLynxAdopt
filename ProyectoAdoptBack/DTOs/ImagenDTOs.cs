namespace ProyectoAdoptBack.DTOs
{
    public class ImagenDTO
    {
        public int ImagenID { get; set; }
        public string EntidadTipo { get; set; } = string.Empty;
        public int EntidadID { get; set; }
        public string Url { get; set; } = string.Empty;
        public int Orden { get; set; }
        public string NombreArchivo { get; set; } = string.Empty;
        public DateTime FechaSubida { get; set; }
    }

    public class CreateImagenDTO
    {
        public string EntidadTipo { get; set; } = string.Empty;
        public int EntidadID { get; set; }
        public string Url { get; set; } = string.Empty;
        public int Orden { get; set; }
        public string NombreArchivo { get; set; } = string.Empty;
    }

    public class UpdateImagenDTO
{
    public string Url { get; set; } = string.Empty;
    public int Orden { get; set; }
    public string NombreArchivo { get; set; } = string.Empty;
}
}
