using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Models;
using ProyectoAdoptBack.Repositories;

namespace ProyectoAdoptBack.Services
{
    public interface IImagenService
    {
        Task<List<ImagenDTO>> GetByEntidadAsync(string entidadTipo, int entidadId);
        Task<int> CreateAsync(CreateImagenDTO dto);
        Task<int> CreateConArchivoAsync(CreateImagenRequest request);
        Task<ImagenDTO?> DeleteAsync(int imagenId);
        Task<List<ImagenDTO>> DeleteByEntidadAsync(string entidadTipo, int entidadId);
        Task ReordenarAsync(int imagenId, int orden);
    }

    public class ImagenService : IImagenService
    {
        private readonly IImagenRepository _repository;
        private readonly ISupabaseStorageService _storage;

        public ImagenService(IImagenRepository repository, ISupabaseStorageService storage)
        {
            _repository = repository;
            _storage = storage;
        }

        public async Task<int> CreateConArchivoAsync(CreateImagenRequest request)
        {
            if (request.Archivo == null || request.Archivo.Length == 0)
                throw new ArgumentException("El archivo es obligatorio.");

            ValidateEntidad(request.EntidadTipo, request.EntidadID);

            using var stream = request.Archivo.OpenReadStream();
            var url = await _storage.UploadAsync(
                stream,
                request.Archivo.FileName,
                request.Archivo.ContentType,
                request.EntidadTipo
            );

            var model = new Imagen
            {
                EntidadTipo = request.EntidadTipo.Trim(),
                EntidadID = request.EntidadID,
                Url = url,
                Orden = request.Orden,
                NombreArchivo = request.Archivo.FileName.Trim()
            };

            return await _repository.CreateAsync(model);
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
            if (imagen is null) return null;

            try { await _storage.DeleteAsync(imagen.Url); }
            catch { }

            return MapToDto(imagen);
        }

        public async Task<List<ImagenDTO>> DeleteByEntidadAsync(string entidadTipo, int entidadId)
        {
            ValidateEntidad(entidadTipo, entidadId);
            var imagenes = await _repository.DeleteByEntidadAsync(entidadTipo, entidadId);
            foreach (var img in imagenes)
            {
                try { await _storage.DeleteAsync(img.Url); }
                catch { }
            }
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