using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Models;
using ProyectoAdoptBack.Repositories;

namespace ProyectoAdoptBack.Services
{
    public interface IRazaService
    {
        Task<List<RazaDTO>> GetAllAsync();
        Task<RazaDTO?> GetByIdAsync(int id);
        Task<List<RazaDTO>> GetByEspecieAsync(int especieId);
        Task<int> CreateAsync(CreateRazaDTO dto);
        Task UpdateAsync(int id, UpdateRazaDTO dto);
    }

    public class RazaService : IRazaService
    {
        private readonly IRazaRepository _repository;

        public RazaService(IRazaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RazaDTO>> GetAllAsync()
        {
            var razas = await _repository.GetAllAsync();
            return razas.Select(MapToDto).ToList();
        }

        public async Task<RazaDTO?> GetByIdAsync(int id)
        {
            var raza = await _repository.GetbyIdAsync(id); 
            return raza is null ? null : MapToDto(raza);
        }

        public async Task<List<RazaDTO>> GetByEspecieAsync(int especieId)
        {
            if (especieId <= 0)
                throw new ArgumentException("EspecieID no válido.");
            var razas = await _repository.GetByEspecieAsync(especieId);
            return razas.Select(MapToDto).ToList();
        }

        public async Task<int> CreateAsync(CreateRazaDTO dto)
        {
            ValidateCreate(dto);
            var model = new Raza
            {
                EspecieID = dto.EspecieID,
                Nombre = dto.Nombre.Trim()
            };
            return await _repository.CreateAsync(model);
        }

        public async Task UpdateAsync(int id, UpdateRazaDTO dto)
        {
            ValidateUpdate(dto);
            var model = new Raza
            {
                RazaID = id, 
                EspecieID = dto.EspecieID,
                Nombre = dto.Nombre.Trim()
            };
            await _repository.UpdateAsync(model); 
        }

        private static RazaDTO MapToDto(Raza model) => new()
        {
            RazaID = model.RazaID,
            EspecieID = model.EspecieID,
            Nombre = model.Nombre
        };

        private static void ValidateCreate(CreateRazaDTO dto)
        {
            if (dto.EspecieID <= 0)
                throw new ArgumentException("EspecieID es obligatorio.");
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new ArgumentException("El nombre de la raza es obligatorio.");
        }

        private static void ValidateUpdate(UpdateRazaDTO dto)
        {
            if (dto.EspecieID <= 0)
                throw new ArgumentException("EspecieID es obligatorio.");
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new ArgumentException("El nombre de la raza es obligatorio.");
        }
    }
}
