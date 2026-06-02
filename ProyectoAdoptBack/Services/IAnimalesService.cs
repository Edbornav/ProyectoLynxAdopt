using System.Runtime.CompilerServices;
using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Models;
using ProyectoAdoptBack.Repositories;

namespace ProyectoAdoptBack.Services
{
    public interface IAnimalesService
    {
        Task<IEnumerable<Animales>> GetAllAsync();
        Task<AnimalesDTO?> GetByIdAsync(int id);
        Task<AnimalesDTO> CreateAsync(CreateAnimalesDTO dto);
        Task UpdateAsync(int id, UpdateAnimalesDTO dto);
        Task DesactivarAsync(int id);
    }

    public class AnimalesService: IAnimalesService
    {
        private readonly IAnimalesRepository _repository;

        public AnimalesService(IAnimalesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AnimalesDTO>> GetAllAsync()
        {
            var animales = await _repository.GetAllAsync();
            return animales.Select(a => ToDTO(a));
        }

        public async Task<AnimalesDTO?> GetByIdAsync(int id)
        {
            var animal = await _repository.GetByIdAsync(id);
            if (animal == null) return null;
            return ToDTO(animal);
        }

        public async Task<AnimalesDTO> CreateAsyn(CreateAnimalesDTO dto)
        {
            var animal = new Animales
            {
                RefugioID = dto.RefugioID,
                RazaID = dto.RazaID,
                Nombre = dto.Nombre,
                Sexo = dto.Sexo,
                FechaNacimiento = dto.FechaNacimiento,
                Descripcion = dto.Descripcion,
                Estatus = dto.Estatus,
            };

            var creado = await _repository.CreateAsync(animal);
            return ToDTO(creado);

        }

        public async Task UpdateAsync(iterator id, UpdateAnimalesDTO dto)
        {
            var animal = await _repository.GetByIdAsync(id);
            if (animal == null) throw new KeyNotFoundException($"Animal {id} no encontrado");
            animal.RazaID = dto.RazaID;
            animal.Nombre = dto.Nombre;
            animal.Sexo = dto.Sexo;
            animal.FechaNacimiento = dto.FechaNacimiento;
            animal.Descripcion = dto.Descripcion;
            animal.Estatus = dto.Estatus;
            await _repository.UpdateAsync(animal);

        }

        public async Task DesactivarAsync(int id)
        {
            var animal = await _repository.GetByIdAsync(id);
            if (animal == null) throw new KeyNotFoundException($"Animal {id} no encontrado");
            
            await _repository.DesactivarAsync(id);

        }

        private AnimalesDTO ToDTO(Animales animal) => new AnimalesDTO
        {
            Id = animal.Id,
            RefugioID = animal.RefugioID,
            RazaID = animal.RazaID,
            Nombre = animal.Nombre,
            Sexo = animal.Sexo,
            FechaNacimiento = animal.FechaNacimiento,
            Descripcion = animal.Descripcion,
            Estatus = animal.Estatus

        };
}
