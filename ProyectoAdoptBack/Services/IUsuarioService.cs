using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Models;
using ProyectoAdoptBack.Repositories;
using BCrypt.Net; //Agregado para usar BCrypt.HashPassword()

namespace ProyectoAdoptBack.Services
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> GetAllAsync();
        Task<UsuarioDTO?> GetByIdAsync(int id);
        Task<UsuarioDTO?> LoginAsync(LoginDTO dto);
        Task<int> CreateAsync(CreateUsuarioDTO dto); //uso de int para retornar el id del usuario
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

        public async Task<UsuarioDTO?> LoginAsync(LoginDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Correo) || string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Correo y contraseña son obligatorios.");

            var usuario = await _repository.LoginAsync(dto.Correo.Trim());
            if (usuario == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
                return null;

            return MapToDto(usuario);
        }

        public async Task<int> CreateAsync(CreateUsuarioDTO dto) //Este metodo debe de retornar el id del usuario para que pueda ser utilizado en la creacion de adoptante o administrador
        {
            ValidateCreate(dto);

            //Se hashea la contraseña con BCrypt antes de guardarla
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var model = new Usuario
            {
                Correo = dto.Correo.Trim(),
                PasswordHash = passwordHash, //Se asigna el hash generado
                TipoUsuario = dto.TipoUsuario.Trim(),
                Estatus = dto.Estatus.Trim(),
                FechaRegistro = DateTime.UtcNow
            };

            return await _repository.CreateAsync(model);
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