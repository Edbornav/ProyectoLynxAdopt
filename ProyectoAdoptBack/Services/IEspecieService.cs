using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.DTOs.Especie;
using ProyectoAdoptBack.Models;
using ProyectoAdoptBack.Repositories;
namespace ProyectoAdoptBack.Services
{
    public interface IEspecieService
    {
        Task<IEnumerable<EspecieDTO>> GetAllAsync();
        Task<EspecieDTO?> GetByIdAsync(int id);
        Task<EspecieDTO> CreateAsync(CreateEspecieDTO dto);
        Task UpdateAsync(int id, UpdateEspecieDTO dto);

    }

    public class EspecieService: IEspecieService
    {
        private readonly IEspecieRepository repository;

        public EspecieServie(IEspecieRepository repository)
        {
            _repository= repository;

        }

        public async Task<IEnumerable<EspecieDTO>> GetAllAsync()
        {
            var especies = await _repository.GetAllAsync();
            return especies.Select(e => ToDTO(e));
        }

        public async Task<EspecieDTO?> GetByIdAsync(int id)
        {
            var especie = await _repository.GetAllAsync();
            if (especie == null) return null;
            return ToDTO(especie);
        }

        public async Task<EspecieDTO> CreateAsync(CreateEspecieDTO dto)
        {
            var especie = new Especie
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };
            var creado = await _repository.CreateAsync(especie);
            return ToDTO(creado);
        }

        public async Task UpdateAsync(int id, UpdateEspecieDTO dto)
        {
            var especie = await _repo.GetByIdAsync(id);
            if (especie == null) throw new KeyNotFoundException($"Especie {id} no encontrada.");

            especie.Nombre = dto.Nombre;

            await _repo.UpdateAsync(especie);
        }

        private static EspecieDTO ToDTO(Especie e) => new EspecieDTO
        {
            EspecieID = e.EspecieID,
            Nombre = e.Nombre
        };

    }
}
