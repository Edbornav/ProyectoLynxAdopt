using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Models;
using ProyectoAdoptBack.Repositories;

namespace ProyectoAdoptBack.Services
{
    public interface ISolicitudAdopcionService // error: estaba como class vacia
    {
        Task<List<SolicitudAdopcionDTO>> GetAllAsync();
        Task<SolicitudAdopcionDTO?> GetByIdAsync(int id);
        Task<List<SolicitudAdopcionDTO>> GetByAdoptanteAsync(int adoptanteId);
        Task<List<SolicitudAdopcionDTO>> GetByRefugioAsync(int refugioId);
        Task CreateAsync(CreateSolicitudAdopcionDTO dto);
        Task UpdateEstatusAsync(int id, UpdateSolicitudAdopcionDTO dto); // error: el DTO real se llama UpdateSolicitudAdopcionDTO
        Task DesactivarAsync(int id);
    }

    public class SolicitudAdopcionService : ISolicitudAdopcionService
    {
        private readonly ISolicitudAdopcionRepository _repository;

        public SolicitudAdopcionService(ISolicitudAdopcionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SolicitudAdopcionDTO>> GetAllAsync()
        {
            var solicitudes = await _repository.GetAllAsync();
            return solicitudes.Select(MapToDto).ToList();
        }

        public async Task<SolicitudAdopcionDTO?> GetByIdAsync(int id)
        {
            var solicitud = await _repository.GetByIdAsync(id);
            return solicitud is null ? null : MapToDto(solicitud);
        }

        public async Task<List<SolicitudAdopcionDTO>> GetByAdoptanteAsync(int adoptanteId)
        {
            if (adoptanteId <= 0)
                throw new ArgumentException("AdoptanteID no valido."); // error: id invalido
            var solicitudes = await _repository.GetByAdoptanteAsync(adoptanteId);
            return solicitudes.Select(MapToDto).ToList();
        }

        public async Task<List<SolicitudAdopcionDTO>> GetByRefugioAsync(int refugioId)
        {
            if (refugioId <= 0)
                throw new ArgumentException("RefugioID no valido."); // error: id invalido
            var solicitudes = await _repository.GetByRefugioAsync(refugioId);
            return solicitudes.Select(MapToDto).ToList();
        }

        public async Task CreateAsync(CreateSolicitudAdopcionDTO dto)
        {
            ValidateCreate(dto);
            var model = new SolicitudAdopcion
            {
                RefugioID = dto.RefugioID,
                AdoptanteID = dto.AdoptanteID,
                MensajeAdoptante = dto.MensajeAdoptante.Trim() // error: faltaba limpiar espacios
            };
            await _repository.CreateAsync(model);
        }

        public async Task UpdateEstatusAsync(int id, UpdateSolicitudAdopcionDTO dto)
        {
            var estatus = dto.Estatus.Trim(); // error: evitar Trim repetido
            var estatusValidos = new[] { "Pendiente", "Aprobada", "Rechazada" };
            if (!estatusValidos.Contains(estatus))
                throw new ArgumentException("Estatus no valido. Use: Pendiente, Aprobada o Rechazada."); // error: estatus invalido
            await _repository.UpdateEstatusAsync(id, estatus);
        }

        public async Task DesactivarAsync(int id)
        {
            await _repository.DesactivarAsync(id);
        }

        private static SolicitudAdopcionDTO MapToDto(SolicitudAdopcion model) => new()
        {
            SolicitudID = model.SolicitudID,
            RefugioID = model.RefugioID,
            AdoptanteID = model.AdoptanteID,
            MensajeAdoptante = model.MensajeAdoptante,
            Estatus = model.Estatus,
            FechaRegistro = model.FechaRegistro
        };

        private static void ValidateCreate(CreateSolicitudAdopcionDTO dto)
        {
            if (dto.RefugioID <= 0)
                throw new ArgumentException("RefugioID es obligatorio."); // error: id obligatorio
            if (dto.AdoptanteID <= 0)
                throw new ArgumentException("AdoptanteID es obligatorio."); // error: id obligatorio
            if (string.IsNullOrWhiteSpace(dto.MensajeAdoptante))
                throw new ArgumentException("El mensaje es obligatorio."); // error: mensaje obligatorio
        }
    }
}
