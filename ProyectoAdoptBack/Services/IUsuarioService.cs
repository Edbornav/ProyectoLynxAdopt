using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Models;
using ProyectoAdoptBack.Repositories;

namespace ProyectoAdoptBack.Services
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> GetAllAsync();
        Task<UsuarioDTO?> GetByIdAsync(int id);
        Task CreateAsync(CreateUsuarioDTO dto);
        Task UpdateAsync(int id, UpdateUsuarioDTO dto);
        Task DesactivarAsync(int id);
    }

    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<UsuarioDTO>> GetAllAsync()
        {
            var usuarios = await _repository.GetAllAsync();
            return usuarios.Select(MapToDto).ToList();
        }

        public async Task<UsuarioDTO?> GetByIdAsync(int id)
        {
            var usuario = await _repository.GetByIdAsync(id);
            return usuario is null ? null : MapToDto(usuario);
        }

        public async Task CreateAsync(CreateUsuarioDTO dto)
        {
            ValidateCreate(dto);

            var model = new Usuario
            {
                Correo = dto.Correo.Trim(),
                TipoUsuario = dto.TipoUsuario.Trim(),
                Estatus = dto.Estatus.Trim(),
                FechaRegistro = DateTime.UtcNow
            };

            await _repository.CreateAsync(model);
        }

        public async Task UpdateAsync(int id, UpdateUsuarioDTO dto)
        {
            ValidateUpdate(dto);

            var model = new Usuario
            {
                Correo = dto.Correo.Trim(),
                TipoUsuario = dto.TipoUsuario.Trim(),
                Estatus = dto.Estatus.Trim()
            };

            await _repository.UpdateAsync(id, model);
        }

        public async Task DesactivarAsync(int id)
        {
            await _repository.DesactivarAsync(id);
        }

        private static UsuarioDTO MapToDto(Usuario model)
        {
            return new UsuarioDTO
            {
                UsuarioID = model.UsuarioID,
                Correo = model.Correo,
                TipoUsuario = model.TipoUsuario,
                Estatus = model.Estatus,
                FechaRegistro = model.FechaRegistro
            };
        }

        private static void ValidateCreate(CreateUsuarioDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Correo) ||
                string.IsNullOrWhiteSpace(dto.TipoUsuario) ||
                string.IsNullOrWhiteSpace(dto.Estatus))
                throw new ArgumentException("Agrega todos los campos, son obligatorios.");

            if (!dto.Correo.Contains('@') || !dto.Correo.Contains('.'))
                throw new ArgumentException("El correo no tiene un formato válido.");

            var tiposValidos = new[] { "Administrador", "Adoptante", "Refugio" };
            if (!tiposValidos.Contains(dto.TipoUsuario.Trim()))
                throw new ArgumentException("TipoUsuario no válido.");

            var estatusValidos = new[] { "Activo", "Inactivo" };
            if (!estatusValidos.Contains(dto.Estatus.Trim()))
                throw new ArgumentException("Estatus no válido.");
        }

        private static void ValidateUpdate(UpdateUsuarioDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Correo) ||
                string.IsNullOrWhiteSpace(dto.TipoUsuario) ||
                string.IsNullOrWhiteSpace(dto.Estatus))
                throw new ArgumentException("Agrega todos los campos, son obligatorios.");

            if (!dto.Correo.Contains('@') || !dto.Correo.Contains('.'))
                throw new ArgumentException("El correo no tiene un formato válido.");

            var tiposValidos = new[] { "Administrador", "Adoptante", "Refugio" };
            if (!tiposValidos.Contains(dto.TipoUsuario.Trim()))
                throw new ArgumentException("TipoUsuario no válido.");

            var estatusValidos = new[] { "Activo", "Inactivo" };
            if (!estatusValidos.Contains(dto.Estatus.Trim()))
                throw new ArgumentException("Estatus no válido.");
        }
    }
}