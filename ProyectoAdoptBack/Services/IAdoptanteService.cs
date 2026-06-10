using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Models;
using ProyectoAdoptBack.Repositories;
namespace ProyectoAdoptBack.Services
{
    public interface IAdoptanteService
    {
        Task<IEnumerable<AdoptanteDTO>> GetAllAsync();
        Task<AdoptanteDTO?> GetByIdAsync(int id);
        Task<AdoptanteDTO?> GetByUsuarioAsync(int usuarioId);
        Task<int> CreateAsync(CreateAdoptanteDTO dto);// Necesitamos retornar el id de adoptante para crear solicitud de adopcion :DD
        Task<int> CreateConFotoAsync(CreateAdoptanteConFotoRequest request);
        Task UpdateAsync(int id, UpdateAdoptanteDTO dto);
        Task DesactivarAsync(int id);
    }

    public class AdoptanteService: IAdoptanteService
    {
        private readonly IAdoptanteRepository _repository;
        private readonly ISupabaseStorageService _storage;
        private readonly IImagenRepository _imagenRepository;

        public AdoptanteService(IAdoptanteRepository repository, ISupabaseStorageService storage, IImagenRepository imagenRepository)
        {
            _repository = repository;
            _storage = storage;
            _imagenRepository = imagenRepository;
        }

        public async Task<int> CreateConFotoAsync(CreateAdoptanteConFotoRequest request)
        {
            var adoptanteId = await CreateAsync(new CreateAdoptanteDTO
            {
                UsuarioID = request.UsuarioID,
                Nombre = request.Nombre,
                ApellidoPaterno = request.ApellidoPaterno,
                ApellidoMaterno = request.ApellidoMaterno,
                Telefono = request.Telefono,
                FechaNacimiento = request.FechaNacimiento
            });

            if (request.Foto != null && request.Foto.Length > 0)
            {
                using var stream = request.Foto.OpenReadStream();
                var url = await _storage.UploadAsync(
                    stream,
                    request.Foto.FileName,
                    request.Foto.ContentType,
                    "Adoptante"
                );

                await _imagenRepository.CreateAsync(new Imagen
                {
                    EntidadTipo = "Adoptante",
                    EntidadID = adoptanteId,
                    Url = url,
                    Orden = 1,
                    NombreArchivo = request.Foto.FileName.Trim()
                });
            }

            return adoptanteId;
        }

      public async Task<IEnumerable<AdoptanteDTO>> GetAllAsync()
        {
            var adoptantes= await _repository.GetAllAsync();
            return adoptantes.Select(a => ToDTO(a));
        }

        public async Task<AdoptanteDTO?> GetByIdAsync(int id)
        {
            var adoptante = await _repository.GetByIdAsync(id);
            if (adoptante == null) return null;
            return ToDTO(adoptante);
        }

        public async Task<AdoptanteDTO?> GetByUsuarioAsync(int usuarioId)
        {
            var adoptante = await _repository.GetByUsuarioAsync(usuarioId);
            if (adoptante == null) return null;
            return ToDTO(adoptante);
        }

        public async Task<int> CreateAsync(CreateAdoptanteDTO dto)
        {
            var model = new Adoptante
            {
                UsuarioID = dto.UsuarioID,
                Nombre = dto.Nombre.Trim(),
                ApellidoPaterno = dto.ApellidoPaterno.Trim(),
                ApellidoMaterno = dto.ApellidoMaterno.Trim(),
                Telefono = dto.Telefono.Trim(), 
                FechaNacimiento = dto.FechaNacimiento
            };
            return await _repository.CreateAsync(model);


        }

        public async Task UpdateAsync(int id, UpdateAdoptanteDTO dto)
        {
            var adoptante = await _repository.GetByIdAsync(id);
            if (adoptante == null) throw new KeyNotFoundException($"Adoptante {id} no encontrado");
            
            adoptante.Nombre = dto.Nombre;
            adoptante.ApellidoPaterno = dto.ApellidoPaterno;
            adoptante.ApellidoMaterno = dto.ApellidoMaterno;
            adoptante.Telefono = dto.Telefono;
            adoptante.FechaNacimiento = dto.FechaNacimiento;

            await _repository.UpdateAsync(adoptante);

        }

        public async Task DesactivarAsync(int id)
        {
            var adoptante = await _repository.GetByIdAsync(id);
            if (adoptante == null) throw new KeyNotFoundException($"Adoptante {id} no encontrado");
            await _repository.DesactivarAsync(id);
        }

        private static AdoptanteDTO ToDTO(Adoptante a) => new AdoptanteDTO
        {
            AdoptanteID = a.AdoptanteID,
            UsuarioID = a.UsuarioID,
            Nombre = a.Nombre,
            ApellidoPaterno = a.ApellidoPaterno,
            ApellidoMaterno = a.ApellidoMaterno,
            Telefono = a.Telefono,
            FechaNacimiento = a.FechaNacimiento
        };

    }
}
