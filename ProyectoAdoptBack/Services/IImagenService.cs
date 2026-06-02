using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Models;
using ProyectoAdoptBack.Repositories;

namespace ProyectoAdoptBack.Services
{
    public interface IImagenService
    {
        Task<List<ImagenDTO>> GetByEntidadAsync(string entidadTipo, int entidadId);
        Task<int> CreateAsync(CreateImagenDTO dto);
        Task<ImagenDTO?> DeleteAsync(int imagenId);
        Task<List<ImagenDTO>> DeleteByEntidadAsync(string entidadTipo, int entidadId);
        Task ReordenarAsync(int imagenId, int orden);
    }

    public class ImagenService : IImagenService
    {
        private readonly IImagenRepository _repository;

        public ImagenService(IImagenRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ImagenDTO>> GetByEntidadAsync(string entidadTipo, int entidadId)
        {
            ValidateEntidad(entidadTipo, entidadId);
            var imagenes = await _repository.GetByEntidadAsync(entidadTipo, entidadId);
            return imagenes.Select(MapToDto).ToList();
        }

        public async Task<int> CreateAsync(CreateImagenDTO dto)
        {
            ValidateCreate(dto);

            var model = new Imagen
            {
                EntidadTipo = dto.EntidadTipo.Trim(),
                EntidadID = dto.EntidadID,
                Url = dto.Url.Trim(),
                Orden = dto.Orden,
                NombreArchivo = dto.NombreArchivo.Trim()
            };

            return await _repository.CreateAsync(model);
        }

        public async Task<ImagenDTO?> DeleteAsync(int imagenId)
        {
            if (imagenId <= 0)
                throw new ArgumentException("ImagenID no válido.");

            var imagen = await _repository.DeleteAsync(imagenId);
            return imagen is null ? null : MapToDto(imagen);
        }

        public async Task<List<ImagenDTO>> DeleteByEntidadAsync(string entidadTipo, int entidadId)
        {
            ValidateEntidad(entidadTipo, entidadId);
            var imagenes = await _repository.DeleteByEntidadAsync(entidadTipo, entidadId);
            return imagenes.Select(MapToDto).ToList();
        }

        public async Task ReordenarAsync(int imagenId, int orden)
        {
            if (imagenId <= 0)
                throw new ArgumentException("ImagenID no válido.");
            if (orden < 0)
                throw new ArgumentException("El orden no puede ser negativo.");

            await _repository.ReordenarAsync(imagenId, orden);
        }

        private static ImagenDTO MapToDto(Imagen model)
        {
            return new ImagenDTO
            {
                ImagenID = model.ImagenID,
                EntidadTipo = model.EntidadTipo,
                EntidadID = model.EntidadID,
                Url = model.Url,
                Orden = model.Orden,
                NombreArchivo = model.NombreArchivo,
                FechaSubida = model.FechaSubida
            };
        }

        private static void ValidateEntidad(string entidadTipo, int entidadId)
        {
            var tiposValidos = new[] { "Animal", "Adoptante", "Refugio" };
            if (!tiposValidos.Contains(entidadTipo.Trim()))
                throw new ArgumentException("EntidadTipo no válido. Use: Animal, Adoptante o Refugio.");

            if (entidadId <= 0)
                throw new ArgumentException("EntidadID no válido.");
        }

        private static void ValidateCreate(CreateImagenDTO dto)
        {
            ValidateEntidad(dto.EntidadTipo, dto.EntidadID);

            if (string.IsNullOrWhiteSpace(dto.Url))
                throw new ArgumentException("La URL es obligatoria.");

            if (string.IsNullOrWhiteSpace(dto.NombreArchivo))
                throw new ArgumentException("El nombre del archivo es obligatorio.");

            if (dto.Orden < 0)
                throw new ArgumentException("El orden no puede ser negativo.");
        }
    }
}