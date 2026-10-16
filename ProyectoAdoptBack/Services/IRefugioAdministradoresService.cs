using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Models;
using ProyectoAdoptBack.Repositories;

namespace ProyectoAdoptBack.Services
{
    public interface IRefugioAdministradoresService // error: estaba como class vacia
    {
        Task<List<RefugioAdministradoresDTO>> GetAllAsync();
        Task<List<RefugioAdministradoresDTO>> GetByRefugioAsync(int refugioId);
        Task<List<RefugioAdministradoresDTO>> GetByAdministradorAsync(int administradorId);
        Task CreateAsync(CreateRefugioAdministradoresDTO dto);
    }

    public class RefugioAdministradoresService : IRefugioAdministradoresService
    {
        private readonly IRefugioAdministradoresRepository _repository;

        public RefugioAdministradoresService(IRefugioAdministradoresRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RefugioAdministradoresDTO>> GetAllAsync()
        {
            var registros = await _repository.GetAllAsync();
            return registros.Select(MapToDto).ToList();
        }

        public async Task<List<RefugioAdministradoresDTO>> GetByRefugioAsync(int refugioId)
        {
            if (refugioId <= 0)
                throw new ArgumentException("RefugioID no valido.");

            var registros = await _repository.GetByRefugioAsync(refugioId);
            return registros.Select(MapToDto).ToList();
        }

        public async Task<List<RefugioAdministradoresDTO>> GetByAdministradorAsync(int administradorId)
        {
            if (administradorId <= 0)
                throw new ArgumentException("AdministradorID no valido.");

            var registros = await _repository.GetByAdministradorAsync(administradorId);
            return registros.Select(MapToDto).ToList();
        }

        public async Task CreateAsync(CreateRefugioAdministradoresDTO dto)
        {
            ValidateCreate(dto);
            var model = new RefugioAdministradores
            {
                RefugioID = dto.RefugioID,
                UsuarioAdminID = dto.UsuarioAdminID
            };
            await _repository.CreateAsync(model);
        }

        private static RefugioAdministradoresDTO MapToDto(RefugioAdministradores model) => new()
        {
            RefugioID = model.RefugioID,
            UsuarioAdminID = model.UsuarioAdminID
        };

        private static void ValidateCreate(CreateRefugioAdministradoresDTO dto)
        {
            if (dto.RefugioID <= 0)
                throw new ArgumentException("RefugioID es obligatorio.");
            if (dto.UsuarioAdminID <= 0)
                throw new ArgumentException("UsuarioAdminID es obligatorio.");
        }
    }
}
