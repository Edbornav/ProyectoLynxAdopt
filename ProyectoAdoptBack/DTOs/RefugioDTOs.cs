namespace ProyectoAdoptBack.DTOs
{
    public class RefugioDTO
    {
        public int RefugioID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Estatus { get; set; } = string.Empty;
        public DateTime FechaDeRegistro { get; set; }
    }

    public class CreateRefugioDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Estatus { get; set; } = string.Empty;
    }

    public class UpdateRefugioDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Estatus { get; set; } = string.Empty;
    }
}
