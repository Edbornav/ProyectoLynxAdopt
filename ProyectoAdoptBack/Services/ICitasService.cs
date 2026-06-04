using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Models;
using ProyectoAdoptBack.Repositories;

namespace ProyectoAdoptBack.Services
{
    public interface ICitasService
    {
        Task<List<CitasDTO>> GetAllAsync(); 
        Task<CitasDTO?> GetByIdAsync(int id); 
        Task<List<CitasDTO>> GetBySolicitudAsync(int solicitudId); 
        Task CreateAsync(CreateCitasDTO dto); 
        Task UpdateAsync(int id, UpdateCitasDTO dto); 
        Task UpdateEstadoAsync(int id, UpdateCitasDTO dto); 
        Task DesactivarAsync(int id);
    }

    public class CitasService : ICitasService
    {
        private readonly ICitasRepository _repository;

        public CitasService(ICitasRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CitasDTO>> GetAllAsync() 
        {
            var citas = await _repository.GetAllAsync();
            return citas.Select(MapToDto).ToList();
        }

        public async Task<CitasDTO?> GetByIdAsync(int id)
        {
            var cita = await _repository.GetByIdAsync(id);
            return cita is null ? null : MapToDto(cita);
        }

        public async Task<List<CitasDTO>> GetBySolicitudAsync(int solicitudId)
        {
            if (solicitudId <= 0)
                throw new ArgumentException("SolicitudID no válido.");
            var citas = await _repository.GetBySolicitudAsync(solicitudId);
            return citas.Select(MapToDto).ToList();
        }

        public async Task CreateAsync(CreateCitasDTO dto) 
        {
            ValidateCreate(dto);
            var model = new Citas 
            {
                SolicitudID = dto.SolicitudID,
                FechaHoraCita = dto.FechaHoraCita,
                EstadoCita = dto.EstadoCita.Trim()
            };
            await _repository.CreateAsync(model);
        }

        public async Task UpdateAsync(int id, UpdateCitasDTO dto) 
        {
            ValidateUpdate(dto);
            var model = new Citas 
            {
                FechaHoraCita = dto.FechaHoraCita,
                EstadoCita = dto.EstadoCita.Trim()
            };
            await _repository.UpdateAsync(id, model);
        }

        public async Task UpdateEstadoAsync(int id, UpdateCitasDTO dto) 
        {
            var estadosValidos = new[] { "Pendiente", "Confirmada", "Cancelada", "Realizada" };
            if (!estadosValidos.Contains(dto.EstadoCita.Trim()))
                throw new ArgumentException("EstadoCita no válido. Use: Pendiente, Confirmada, Cancelada o Realizada.");
            await _repository.UpdateEstadoAsync(id, dto.EstadoCita.Trim());
        }

        public async Task DesactivarAsync(int id)
        {
            await _repository.DesactivarAsync(id);
        }

        private static CitasDTO MapToDto(Citas model) => new() 
        {
            CitaID = model.CitaID,
            SolicitudID = model.SolicitudID,
            FechaHoraCita = model.FechaHoraCita,
            EstadoCita = model.EstadoCita
        };

        private static void ValidateCreate(CreateCitasDTO dto) 
        {
            if (dto.SolicitudID <= 0)
                throw new ArgumentException("SolicitudID es obligatorio.");
            if (dto.FechaHoraCita <= DateTime.Now)
                throw new ArgumentException("La fecha y hora de la cita debe ser futura.");
            var estadosValidos = new[] { "Pendiente", "Confirmada", "Cancelada", "Realizada" };
            if (!estadosValidos.Contains(dto.EstadoCita.Trim()))
                throw new ArgumentException("EstadoCita no válido. Use: Pendiente, Confirmada, Cancelada o Realizada.");
        }

        private static void ValidateUpdate(UpdateCitasDTO dto)
        {
            if (dto.FechaHoraCita <= DateTime.Now)
                throw new ArgumentException("La fecha y hora de la cita debe ser futura.");
            var estadosValidos = new[] { "Pendiente", "Confirmada", "Cancelada", "Realizada" };
            if (!estadosValidos.Contains(dto.EstadoCita.Trim()))
                throw new ArgumentException("EstadoCita no válido. Use: Pendiente, Confirmada, Cancelada o Realizada.");
        }
    }
}
