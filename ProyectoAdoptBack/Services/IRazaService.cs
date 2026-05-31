using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Models;
using ProyectoAdoptBack.Repositories;
namespace ProyectoAdoptBack.Services
{
    public interface IRazaService
    {
        Task<IEnumerable<RazaDTO>> GetAllAsync();
        Task<RazaDTO?> GetByIdAsync(int id);
        Task<RazaDTO> CreateAsync(CreateRazaDTO dto);
        Task UpdateAsync(int id, UpdateRazaDTO dto);
    }

    public class RazaService : IRazaService
    {
        private readonly IRazaRepository _repo; 

        public RazaService(IRazaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<RazaDTO>> GetAllAsync()
        {
            var razas = await _repo.GetAllAsync();
            return razas.Select(r => ToDTO(r));
        }

        public async Task<RazaDTO?> GetByIdAsync(int id)
        {
            var raza = await _repo.GetByIdAsync(id);
            if (raza == null) return null;
            return ToDTO(raza);
        }

        public async Task<RazaDTO> CreateAsync(CreateRazaDTO dto)
        {
            var raza = new Raza
            {
                EspecieID = dto.EspecieID,
                Nombre = dto.Nombre
            };

            var creada = await _repo.CreateAsync(raza);
            return ToDTO(creada);
        }

        public async Task UpdateAsync(int id, UpdateRazaDTO dto)
        {
            var raza = await _repo.GetByIdAsync(id);
            if (raza == null) throw new KeyNotFoundException($"Raza {id} no encontrada.");

            raza.EspecieID = dto.EspecieID;
            raza.Nombre = dto.Nombre;

            await _repo.UpdateAsync(raza);
        }

        private static RazaDTO ToDTO(Raza r) => new RazaDTO
        {
            RazaID = r.RazaID,
            EspecieID = r.EspecieID,
            Nombre = r.Nombre
        };
    }
}