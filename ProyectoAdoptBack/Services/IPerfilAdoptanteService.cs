using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Models;
using ProyectoAdoptBack.Repositories;
namespace ProyectoAdoptBack.Services
{
    public interface IPerfilAdoptanteService
    {
        Task<IEnumerable<PerfilAdoptanteDTO>> GetAllAsync();
        Task<PerfilAdoptanteDTO?> GetByIdAsync(int id);
        Task<PerfilAdoptanteDTO> CreateAsync(CreatePerfilAdoptanteDTO dto);
        Task UpdateAsync(int id, UpdatePerfilAdoptanteDTO dto);
    }

    public class PerfilAdoptanteService:IPerfilAdoptanteService
    {
        private readonly IPerfilAdoptanteRepository repository;

        public PerfilAdoptanteService(IPerfilAdoptanteRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PerfilAdoptanteDTO>> GetAllAsync()
        {
            var perfiles = await _repo.GetAllAsync();
            return perfiles.Select(p => ToDTO(p));
        }

        public async Task<PerfilAdoptanteDTO?> GetByIdAsync(int id)
        {
            var perfil = await _repo.GetByIdAsync(id);
            if (perfil == null) return null;
            return ToDTO(perfil);
        }

        public async Task<PerfilAdoptanteDTO> CreateAsync(CreatePerfilAdoptanteDTO dto)
        {
            var perfil = new PerfilAdoptante
            {
                AdoptanteUsuarioID = dto.AdoptanteUsuarioID,
                DescripcionCasa = dto.DescripcionCasa,
                DescripcionMascotas = dto.DescripcionMascotas,
                DescripcionExperienciaConMascotas = dto.DescripcionExperienciaConMascotas
            };

            var creado = await _repo.CreateAsync(perfil);
            return ToDTO(creado);
        }

        public async Task UpdateAsync(int id, UpdatePerfilAdoptanteDTO dto)
        {
            var perfil = await _repo.GetByIdAsync(id);
            if (perfil == null) throw new KeyNotFoundException($"PerfilAdoptante {id} no encontrado.");

            perfil.DescripcionCasa = dto.DescripcionCasa;
            perfil.DescripcionMascotas = dto.DescripcionMascotas;
            perfil.DescripcionExperienciaConMascotas = dto.DescripcionExperienciaConMascotas;

            await _repo.UpdateAsync(perfil);
        }

        private static PerfilAdoptanteDTO ToDTO(PerfilAdoptante p) => new PerfilAdoptanteDTO
        {
            PerfilAdoptanteID = p.PerfilAdoptanteID,
            AdoptanteUsuarioID = p.AdoptanteUsuarioID,
            DescripcionCasa = p.DescripcionCasa,
            DescripcionMascotas = p.DescripcionMascotas,
            DescripcionExperienciaConMascotas = p.DescripcionExperienciaConMascotas
        };
         


    }
}
