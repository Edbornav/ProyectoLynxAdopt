namespace ProyectoAdoptBack.DTOs
{
    public class RazaDTO
    {
        public int RazaID { get; set; }
        public int EspecieID { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class CreateRazaDTO
    {
        public int EspecieID { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class UpdateRazaDTO
    {
        public int EspecieID { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
