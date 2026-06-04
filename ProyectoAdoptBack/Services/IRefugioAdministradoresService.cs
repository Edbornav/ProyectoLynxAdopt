using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Models;
using ProyectoAdoptBack.Repositories;

namespace ProyectoAdoptBack.Services
{
    public interface IRefugioAdministradoresService // error: estaba como class vacia
    {
        Task<List<RefugioAdministradoresDTO>> GetAllAsync();
        Task<List<RefugioAdministradoresDTO>> GetByRefugioAsync(int refugioId);
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
                throw new ArgumentException("RefugioID no valido."); // error: id invalido

            var registros = await _repository.GetAllAsync(); // error: repository no tiene GetByRefugioAsync
            return registros.Where(r => r.RefugioID == refugioId).Select(MapToDto).ToList();
        }

        public async Task CreateAsync(CreateRefugioAdministradoresDTO dto)
        {
            ValidateCreate(dto);
            var model = new RefugioAdministradores
            {
                RefugioID = dto.RefugioID,
                AdministradorID = dto.AdministradorID // error: el DTO usa AdministradorID, no UsuarioAdminID
            };
            await _repository.CreateAsync(model);
        }

        private static RefugioAdministradoresDTO MapToDto(RefugioAdministradores model) => new()
        {
            RefugioID = model.RefugioID,
            AdministradorID = model.AdministradorID // error: el modelo usa AdministradorID, no UsuarioAdminID
        };

        private static void ValidateCreate(CreateRefugioAdministradoresDTO dto)
        {
            if (dto.RefugioID <= 0)
                throw new ArgumentException("RefugioID es obligatorio."); // error: id obligatorio
            if (dto.AdministradorID <= 0)
                throw new ArgumentException("AdministradorID es obligatorio."); // error: propiedad correcta
        }
    }
}
