using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Models;
using ProyectoAdoptBack.Repositories;

namespace ProyectoAdoptBack.Services
{
    public interface ISolicitudAnimalesService // error: estaba como class vacia
    {
        Task<List<SolicitudAnimalesDTO>> GetBySolicitudAsync(int solicitudId);
        Task CreateAsync(CreateSolicitudAnimalesDTO dto);
    }

    public class SolicitudAnimalesService : ISolicitudAnimalesService
    {
        private readonly ISolicitudAnimalesRepository _repository;

        public SolicitudAnimalesService(ISolicitudAnimalesRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SolicitudAnimalesDTO>> GetBySolicitudAsync(int solicitudId)
        {
            if (solicitudId <= 0)
                throw new ArgumentException("SolicitudID no valido."); // error: id invalido

            var animales = await _repository.GetAllAsync(); // error: repository no tiene GetBySolicitudAsync
            return animales.Where(a => a.SolicitudID == solicitudId).Select(MapToDto).ToList();
        }

        public async Task CreateAsync(CreateSolicitudAnimalesDTO dto)
        {
            ValidateCreate(dto);
            var model = new SolicitudAnimales
            {
                SolicitudID = dto.SolicitudID,
                AnimalID = dto.AnimalID
            };
            await _repository.CreateAsync(model);
        }

        private static SolicitudAnimalesDTO MapToDto(SolicitudAnimales model) => new()
        {
            SolicitudID = model.SolicitudID,
            AnimalID = model.AnimalID
        };

        private static void ValidateCreate(CreateSolicitudAnimalesDTO dto)
        {
            if (dto.SolicitudID <= 0)
                throw new ArgumentException("SolicitudID es obligatorio."); // error: id obligatorio
            if (dto.AnimalID <= 0)
                throw new ArgumentException("AnimalID es obligatorio."); // error: id obligatorio
        }
    }
}
