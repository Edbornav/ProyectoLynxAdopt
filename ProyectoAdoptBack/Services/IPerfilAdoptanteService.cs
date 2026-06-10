using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Models;
using ProyectoAdoptBack.Repositories;
namespace ProyectoAdoptBack.Services
{
    public interface IPerfilAdoptanteService
    {
        Task<IEnumerable<PerfilAdoptanteDTO>> GetAllAsync();
        Task<PerfilAdoptanteDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreatePerfilAdoptanteDTO dto);
        Task UpdateAsync(int id, UpdatePerfilAdoptanteDTO dto);
    }

    public class PerfilAdoptanteService:IPerfilAdoptanteService
    {
        private readonly IPerfilAdoptanteRepository _repository; // cambio (se corrigio nombre del campo usado en el service)

        public PerfilAdoptanteService(IPerfilAdoptanteRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PerfilAdoptanteDTO>> GetAllAsync()
        {
            var perfiles = await _repository.GetAllAsync(); // cambio (se corrigio nombre del repository)
            return perfiles.Select(p => ToDTO(p));
        }

        public async Task<PerfilAdoptanteDTO?> GetByIdAsync(int id)
        {
            var perfil = await _repository.GetByIdAsync(id); // cambio (se corrigio nombre del repository)
            if (perfil == null) return null;
            return ToDTO(perfil);
        }

        public async Task<int> CreateAsync(CreatePerfilAdoptanteDTO dto)
        {
            ValidateCreate(dto);
            var perfil = new PerfilAdoptante
            {
                AdoptanteUsuarioID = dto.AdoptanteUsuarioID,
                DescripcionCasa = dto.DescripcionCasa.Trim(),
                DescripcionMascotas = dto.DescripcionMascotas.Trim(),
                DescripcionExperienciaConMascotas = dto.DescripcionExperienciaConMascotas.Trim()
            };

            return await _repository.CreateAsync(perfil);
        }

        public async Task UpdateAsync(int id, UpdatePerfilAdoptanteDTO dto)
        {
            ValidateUpdate(dto); // error: faltaba validar campos obligatorios
            var perfil = await _repository.GetByIdAsync(id); // cambio (se corrigio nombre del repository)
            if (perfil == null) throw new KeyNotFoundException($"PerfilAdoptante {id} no encontrado.");

            perfil.DescripcionCasa = dto.DescripcionCasa.Trim(); // error: faltaba limpiar espacios
            perfil.DescripcionMascotas = dto.DescripcionMascotas.Trim(); // error: faltaba limpiar espacios
            perfil.DescripcionExperienciaConMascotas = dto.DescripcionExperienciaConMascotas.Trim(); // error: faltaba limpiar espacios

            await _repository.UpdateAsync(id, perfil);
        }

        private static PerfilAdoptanteDTO ToDTO(PerfilAdoptante p) => new PerfilAdoptanteDTO
        {
            PerfilAdoptanteID = p.PerfilAdoptanteID,
            AdoptanteUsuarioID = p.AdoptanteUsuarioID,
            DescripcionCasa = p.DescripcionCasa,
            DescripcionMascotas = p.DescripcionMascotas,
            DescripcionExperienciaConMascotas = p.DescripcionExperienciaConMascotas
        };

        private static void ValidateCreate(CreatePerfilAdoptanteDTO dto)
        {
            if (dto.AdoptanteUsuarioID <= 0)
                throw new ArgumentException("AdoptanteUsuarioID es obligatorio."); // error: id obligatorio
            if (string.IsNullOrWhiteSpace(dto.DescripcionCasa) ||
                string.IsNullOrWhiteSpace(dto.DescripcionMascotas) ||
                string.IsNullOrWhiteSpace(dto.DescripcionExperienciaConMascotas))
                throw new ArgumentException("Todos los campos son obligatorios."); // error: campos obligatorios
        }

        private static void ValidateUpdate(UpdatePerfilAdoptanteDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DescripcionCasa) ||
                string.IsNullOrWhiteSpace(dto.DescripcionMascotas) ||
                string.IsNullOrWhiteSpace(dto.DescripcionExperienciaConMascotas))
                throw new ArgumentException("Todos los campos son obligatorios."); // error: campos obligatorios
        }
          


    }
}
