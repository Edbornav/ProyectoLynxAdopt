using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Models;
using ProyectoAdoptBack.Repositories;

namespace ProyectoAdoptBack.Services
{
    public interface IAdministradorService
    {
        Task<List<AdministradorDTO>> GetAllAsync();
        Task<AdministradorDTO?> GetByIdAsync(int id);
        Task CreateAsync(CreateAdministradorDTO dto);
        Task UpdateAsync(int id, UpdateAdministradorDTO dto);
        Task DesactivarAsync(int id);
    }

    public class AdministradorService : IAdministradorService
    {
        private readonly IAdministradorRepository _repository;

        public AdministradorService(IAdministradorRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AdministradorDTO>> GetAllAsync()
        {
            var administradores = await _repository.GetAllAsync();
            return administradores.Select(MapToDto).ToList();
        }

        public async Task<AdministradorDTO?> GetByIdAsync(int id)
        {
            var administrador = await _repository.GetByIdAsync(id);
            return administrador is null ? null : MapToDto(administrador);
        }

        public async Task CreateAsync(CreateAdministradorDTO dto)
        {
            ValidateCreate(dto);

            var model = new Administrador
            {
                UsuarioID = dto.UsuarioID,
                Nombre = dto.Nombre.Trim(),
                ApellidoPaterno = dto.ApellidoPaterno.Trim(),
                ApellidoMaterno = dto.ApellidoMaterno.Trim(),
                Telefono = dto.Telefono.Trim()
            };

            await _repository.CreateAsync(model);
        }

        public async Task UpdateAsync(int id, UpdateAdministradorDTO dto)
        {
            ValidateUpdate(dto);

            var model = new Administrador
            {
                Nombre = dto.Nombre.Trim(),
                ApellidoPaterno = dto.ApellidoPaterno.Trim(),
                ApellidoMaterno = dto.ApellidoMaterno.Trim(),
                Telefono = dto.Telefono.Trim()
            };

            await _repository.UpdateAsync(id, model);
        }

        public async Task DesactivarAsync(int id)
        {
            await _repository.DesactivarAsync(id);
        }

        private static AdministradorDTO MapToDto(Administrador model)
        {
            return new AdministradorDTO
            {
                AdministradorID = model.AdministradorID,
                UsuarioID = model.UsuarioID,
                Nombre = model.Nombre,
                ApellidoPaterno = model.ApellidoPaterno,
                ApellidoMaterno = model.ApellidoMaterno,
                Telefono = model.Telefono
            };
        }

        private static void ValidateCreate(CreateAdministradorDTO dto)
        {
            if (dto.UsuarioID <= 0)
                throw new ArgumentException("UsuarioID es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Nombre) ||
                string.IsNullOrWhiteSpace(dto.ApellidoPaterno) ||
                string.IsNullOrWhiteSpace(dto.ApellidoMaterno) ||
                string.IsNullOrWhiteSpace(dto.Telefono))
                throw new ArgumentException("Todos los campos son obligatorios.");

            if (dto.Telefono.Trim().Length != 10)
                throw new ArgumentException("El teléfono debe tener 10 caracteres.");
        }

        private static void ValidateUpdate(UpdateAdministradorDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre) ||
                string.IsNullOrWhiteSpace(dto.ApellidoPaterno) ||
                string.IsNullOrWhiteSpace(dto.ApellidoMaterno) ||
                string.IsNullOrWhiteSpace(dto.Telefono))
                throw new ArgumentException("Todos los campos son obligatorios.");

            if (dto.Telefono.Trim().Length != 10)
                throw new ArgumentException("El teléfono debe tener 10 caracteres.");
        }
    }
}