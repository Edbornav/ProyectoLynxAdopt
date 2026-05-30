namespace ProyectoAdoptBack.DTOs
{
    public class EspecieDTO
    {
        public int EspecieID { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class CreateEspecieDTO
    {
        public string Nombre { get; set; } = string.Empty;
    }

    public class UpdateEspecieDTO
    {
        public string Nombre { get; set; } = string.Empty;
    }
}
