using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Models;
using ProyectoAdoptBack.Repositories;
//Refugio
namespace ProyectoAdoptBack.Services
{
    public interface IRefugioService
    {
        Task<List<RefugioDTO>> GetAllAsync();
        Task<RefugioDTO?> GetByIdAsync(int id);
        Task <int> CreateAsync(CreateRefugioDTO dto);
        Task<int> CreateConLogoAsync(CreateRefugioConLogoRequest request);
        Task UpdateAsync(int id, UpdateRefugioDTO dto);
        Task DesactivarAsync(int id);
    }

    public class RefugioService : IRefugioService
    {
        private readonly IRefugioRepository _repository;
        private readonly ISupabaseStorageService _storage;
        private readonly IImagenRepository _imagenRepository;

        public RefugioService(IRefugioRepository repository, ISupabaseStorageService storage, IImagenRepository imagenRepository)
        {
            _repository = repository;
            _storage = storage;
            _imagenRepository = imagenRepository;
        }

        public async Task<int> CreateConLogoAsync(CreateRefugioConLogoRequest request)
        {
            var refugioId = await CreateAsync(new CreateRefugioDTO
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Direccion = request.Direccion,
                Telefono = request.Telefono,
                Correo = request.Correo,
                Estatus = request.Estatus
            });

            if (request.Logo != null && request.Logo.Length > 0)
            {
                using var stream = request.Logo.OpenReadStream();
                var url = await _storage.UploadAsync(
                    stream,
                    request.Logo.FileName,
                    request.Logo.ContentType,
                    "Refugio"
                );

                await _imagenRepository.CreateAsync(new Imagen
                {
                    EntidadTipo = "Refugio",
                    EntidadID = refugioId,
                    Url = url,
                    Orden = 1,
                    NombreArchivo = request.Logo.FileName.Trim()
                });
            }

            return refugioId;
        }

        public async Task<List<RefugioDTO>> GetAllAsync()
        {
            var refugios = await _repository.GetAllAsync();
            return refugios.Select(MapToDto).ToList();
        }

        public async Task<RefugioDTO?> GetByIdAsync(int id)
        {
            var refugio = await _repository.GetByIdAsync(id);
            return refugio is null ? null : MapToDto(refugio);
        }

        public async Task<int> CreateAsync(CreateRefugioDTO dto)
        {
            ValidateCreate(dto);

            var model = new Refugio
            {
                Nombre = dto.Nombre.Trim(),
                Descripcion = dto.Descripcion.Trim(),
                Direccion = dto.Direccion.Trim(),
                Telefono = dto.Telefono.Trim(),
                Correo = dto.Correo.Trim(),
                Estatus = dto.Estatus.Trim()
            };

            return await _repository.CreateAsync(model);
        }

        public async Task UpdateAsync(int id, UpdateRefugioDTO dto)
        {
            ValidateUpdate(dto);

            var model = new Refugio
            {
                Nombre = dto.Nombre.Trim(),
                Descripcion = dto.Descripcion.Trim(),
                Direccion = dto.Direccion.Trim(),
                Telefono = dto.Telefono.Trim(),
                Correo = dto.Correo.Trim(),
                Estatus = dto.Estatus.Trim()
            };

            await _repository.UpdateAsync(id, model);
        }

        public async Task DesactivarAsync(int id)
        {
            await _repository.DesactivarAsync(id);
        }

        private static RefugioDTO MapToDto(Refugio model) => new()
        {
            RefugioID = model.RefugioID,
            Nombre = model.Nombre,
            Descripcion = model.Descripcion,
            Direccion = model.Direccion,
            Telefono = model.Telefono,
            Correo = model.Correo,
            Estatus = model.Estatus,
            FechaDeRegistro = model.FechaDeRegistro
        };

        private static void ValidateCreate(CreateRefugioDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre) ||
                string.IsNullOrWhiteSpace(dto.Descripcion) ||
                string.IsNullOrWhiteSpace(dto.Direccion) ||
                string.IsNullOrWhiteSpace(dto.Telefono) ||
                string.IsNullOrWhiteSpace(dto.Correo))
                throw new ArgumentException("Todos los campos son obligatorios.");

            if (dto.Telefono.Trim().Length != 10)
                throw new ArgumentException("El teléfono debe tener 10 dígitos.");

            if (!dto.Correo.Contains('@') || !dto.Correo.Contains('.'))
                throw new ArgumentException("El correo no tiene un formato válido.");

            var estatusValidos = new[] { "Activo", "Inactivo" };
            if (!estatusValidos.Contains(dto.Estatus.Trim()))
                throw new ArgumentException("Estatus no válido. Use: Activo o Inactivo.");
        }

        private static void ValidateUpdate(UpdateRefugioDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre) ||
                string.IsNullOrWhiteSpace(dto.Descripcion) ||
                string.IsNullOrWhiteSpace(dto.Direccion) ||
                string.IsNullOrWhiteSpace(dto.Telefono) ||
                string.IsNullOrWhiteSpace(dto.Correo))
                throw new ArgumentException("Todos los campos son obligatorios.");

            if (dto.Telefono.Trim().Length != 10)
                throw new ArgumentException("El teléfono debe tener 10 dígitos.");

            if (!dto.Correo.Contains('@') || !dto.Correo.Contains('.'))
                throw new ArgumentException("El correo no tiene un formato válido.");

            var estatusValidos = new[] { "Activo", "Inactivo" };
            if (!estatusValidos.Contains(dto.Estatus.Trim()))
                throw new ArgumentException("Estatus no válido. Use: Activo o Inactivo.");
        }
    }
}
