using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Models;
using ProyectoAdoptBack.Repositories;
namespace ProyectoAdoptBack.Services
{
    public interface IcitasService{
        Task<IEnumerable<CitasDTO>> GetAllAsync();
        Task<CitasDTO?> GetByIdAsync(int id);
        Task<CitasDTO> CreateAsync(CreateCitasDTO dto);
        Task UpdateAsync(int id, UpdateCitasDTO dto);
        Task DesactivarAsync(int id);
    }

    public class CitasService: ICitasService
    {
        private readonly IcitasRepository _repository;

        public CitasService(IcitasService repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CitasDTO>> GetAllAsync()
        {
            var citas = await _repository.GetAllAsync();
            return citas.Select(citas => TODTO(c));
        }

        public async Task<CitasDTO?> GetByIdAsync(int id)
        {
            var cita = await _repository.GetByIdAsync(id);
            if (cita == null) return null;
            return TODTO(cita);

        }

        public async Task<CitasDTO> CreateAsync(CreateCitasDTO dto)
        {
            var cita = new Citas
            {
                SolicitudID = dto.SolicitudID,
                FechaHoraCita = dto.FechaHoraCita,
                EstadoCita = dto.EstadoCita
            };
            var creado = await _repository.CreateAsync(cita);
            return TODTO(creado);
        }

        public async Task UpdateAsync(int id, UpdateCitasDTO dto)
        {
            var cita = await _repository.GetbyIDAsync(id);
            if (cita == null) throw new Exception($"Cita {id} no encontrada");) 

            cita.FechaHoraCita = dto.FechaHoraCita;
            cita.EstadoCita = dto.EstadoCita;

            await _repository.UpdateAsync(cita);

        }

        public async Task DesactivarAsync(int id)
        {
            var cita = await _repo.GetByIdAsync(id);
            if (cita == null) throw new KeyNotFoundException($"Cita {id} no encontrada.");

            await _repo.DesactivarAsync(id);
        }

        private static CitasDTO ToDTO(Citas c) => new CitasDTO
        {
            CitaID = c.CitaID,
            SolicitudID = c.SolicitudID,
            FechaHoraCita = c.FechaHoraCita,
            EstadoCita = c.EstadoCita 
        };

    }
}
