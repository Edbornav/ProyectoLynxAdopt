using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Models;
using ProyectoAdoptBack.Repositories;

namespace ProyectoAdoptBack.Services
{
    public interface IAnimalesService
    {
        Task<List<AnimalesDTO>> GetAllAsync();
        Task<AnimalesDTO?> GetByIdAsync(int id);
        Task<List<AnimalesDTO>> GetByRefugioAsync(int refugioId);
        Task<List<AnimalesDTO>> GetDisponiblesAsync();
        Task<int> CreateAsync(CreateAnimalesDTO dto);
        Task UpdateAsync(int id, UpdateAnimalesDTO dto);
        Task DesactivarAsync(int id);
    }

    public class AnimalesService : IAnimalesService
    {
        private readonly IAnimalesRepository _repository;

        public AnimalesService(IAnimalesRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AnimalesDTO>> GetAllAsync()
        {
            var animales = await _repository.GetAllAsync();
            return animales.Select(MapToDto).ToList();
        }

        public async Task<AnimalesDTO?> GetByIdAsync(int id)
        {
            var animal = await _repository.GetByIdAsync(id);
            return animal is null ? null : MapToDto(animal);
        }

        public async Task<List<AnimalesDTO>> GetByRefugioAsync(int refugioId)
        {
            if (refugioId <= 0)
                throw new ArgumentException("RefugioID no válido.");
            var animales = await _repository.GetByRefugioAsync(refugioId);
            return animales.Select(MapToDto).ToList();
        }

        public async Task<List<AnimalesDTO>> GetDisponiblesAsync()
        {
            var animales = await _repository.GetDisponiblesAsync();
            return animales.Select(MapToDto).ToList();
        }

        public async Task<int> CreateAsync(CreateAnimalesDTO dto)
        {
            ValidateCreate(dto);
            var model = new Animales
            {
                RefugioID = dto.RefugioID,
                RazaID = dto.RazaID,
                Nombre = dto.Nombre.Trim(),
                Sexo = dto.Sexo.Trim(),
                FechaNacimiento = dto.FechaNacimiento,
                Descripcion = dto.Descripcion.Trim(),
                Estatus = dto.Estatus.Trim()
            };
            return await _repository.CreateAsync(model);
        }

        public async Task UpdateAsync(int id, UpdateAnimalesDTO dto)
        {
            ValidateUpdate(dto);
            var model = new Animales
            {
                AnimalID = id,
                RazaID = dto.RazaID,
                Nombre = dto.Nombre.Trim(),
                Sexo = dto.Sexo.Trim(),
                FechaNacimiento = dto.FechaNacimiento,
                Descripcion = dto.Descripcion.Trim(),
                Estatus = dto.Estatus.Trim()
            };
            await _repository.UpdateAsync(id, model); 
        }

        public async Task DesactivarAsync(int id)
        {
            await _repository.DesactivarAsync(id);
        }

        private static AnimalesDTO MapToDto(Animales model) => new()
        {
            AnimalID = model.AnimalID,
            RefugioID = model.RefugioID,
            RazaID = model.RazaID,
            Nombre = model.Nombre,
            Sexo = model.Sexo,
            FechaNacimiento = model.FechaNacimiento,
            Descripcion = model.Descripcion,
            Estatus = model.Estatus,
            FechaRegistro = model.FechaRegistro,
            FotoUrl = model.FotoUrl
        };

        private static void ValidateCreate(CreateAnimalesDTO dto)
        {
            if (dto.RefugioID <= 0)
                throw new ArgumentException("RefugioID es obligatorio.");
            if (dto.RazaID <= 0)
                throw new ArgumentException("RazaID es obligatorio.");
            if (string.IsNullOrWhiteSpace(dto.Nombre) ||
                string.IsNullOrWhiteSpace(dto.Descripcion))
                throw new ArgumentException("Todos los campos son obligatorios.");
            var sexosValidos = new[] { "Macho", "Hembra" };
            if (!sexosValidos.Contains(dto.Sexo.Trim()))
                throw new ArgumentException("Sexo no válido. Use: Macho o Hembra.");
            var estatusValidos = new[] { "Disponible", "En Proceso", "Adoptado" };
            if (!estatusValidos.Contains(dto.Estatus.Trim()))
                throw new ArgumentException("Estatus no válido. Use: Disponible, En Proceso o Adoptado.");
        }

        private static void ValidateUpdate(UpdateAnimalesDTO dto)
        {
            if (dto.RazaID <= 0)
                throw new ArgumentException("RazaID es obligatorio.");
            if (string.IsNullOrWhiteSpace(dto.Nombre) ||
                string.IsNullOrWhiteSpace(dto.Descripcion))
                throw new ArgumentException("Todos los campos son obligatorios.");
            var sexosValidos = new[] { "Macho", "Hembra" };
            if (!sexosValidos.Contains(dto.Sexo.Trim()))
                throw new ArgumentException("Sexo no válido. Use: Macho o Hembra.");
            var estatusValidos = new[] { "Disponible", "En Proceso", "Adoptado" };
            if (!estatusValidos.Contains(dto.Estatus.Trim()))
                throw new ArgumentException("Estatus no válido. Use: Disponible, En Proceso o Adoptado.");
        }
    }
}
