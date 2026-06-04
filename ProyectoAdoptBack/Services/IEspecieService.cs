using ProyectoAdoptBack.DTOs;
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
        private readonly IEspecieRepository _repository; // cambio (se corrigio nombre del campo usado en el service)

        public EspecieService(IEspecieRepository repository) // cambio (se corrigio typo del constructor)
        {
            _repository= repository;

        }

        public async Task<IEnumerable<EspecieDTO>> GetAllAsync()
        {
            var especies = await _repository.GetEspecies(); // cambio (metodo correcto del repository)
            return especies.Select(e => ToDTO(e));
        }

        public async Task<EspecieDTO?> GetByIdAsync(int id)
        {
            var especie = await _repository.GetByIdAsync(id); // cambio (se consulta por id en lugar de traer todos)
            if (especie == null) return null;
            return ToDTO(especie);
        }

        public async Task<EspecieDTO> CreateAsync(CreateEspecieDTO dto)
        {
            ValidateCreate(dto); // error: faltaba validar nombre vacio
            var especie = new Especie
            {
                Nombre = dto.Nombre.Trim() // error: faltaba limpiar espacios
            };
            var creado = await _repository.CreateAsync(especie);
            return ToDTO(creado);
        }

        public async Task UpdateAsync(int id, UpdateEspecieDTO dto)
        {
            ValidateUpdate(dto); // error: faltaba validar nombre vacio
            var especie = await _repository.GetByIdAsync(id); // cambio (se corrigio nombre del repository)
            if (especie == null) throw new KeyNotFoundException($"Especie {id} no encontrada.");

            especie.Nombre = dto.Nombre.Trim(); // error: faltaba limpiar espacios

            await _repository.UpdateAsync(especie); // cambio (se corrigio nombre del repository)
        }

        private static EspecieDTO ToDTO(Especie e) => new EspecieDTO
        {
            EspecieID = e.EspecieID,
            Nombre = e.Nombre
        };

        private static void ValidateCreate(CreateEspecieDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new ArgumentException("El nombre de la especie es obligatorio."); // error: nombre obligatorio
        }

        private static void ValidateUpdate(UpdateEspecieDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new ArgumentException("El nombre de la especie es obligatorio."); // error: nombre obligatorio
        }

    }
}
