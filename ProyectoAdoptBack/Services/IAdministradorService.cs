using Dapper;
using System.Data.SqlClient;
using ProyectoAdoptBack.Repositories;
using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Models;

namespace ProyectoAdoptBack.Services
{

    public interface IAdministradorService
    {
        Task<IEnumerable<AdministradorDTO>> GetAllsync();
        Task<AdministradorDTO?> GetByIdAsync(int id);
        Task<AdministradorDTO> CreateAsync(CreateAdministradorDTO dto);
        Task<AdministradorDTO> UpdateAsync(int id, UpdateAdministradorDTO dto);
        Task DesactivarAsync(int id);
    }

    public class AdministradorService : IAdministradorService
    {
        private readonly IAdministradorRepository _repo;

        public AdministradorService(IAdministradorRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<AdministradorDTO>> GetAllAsync()
        {
            var admins = await _repo.GetAllAsync();
            return admins.Select(a => ToDTO(a));
        }

        public async Task<AdministradorDTO?> GetByIdAsync(int id)
        {
            var admin = await _repo.GetByIdAsync(id);
            if (admin == null) return null;
            return ToDTO(admin);
        }

        public async Task<AdministradorDTO> CreateAsync(CreateAdministradorDTO dto)
        {
            var admin = new Administrador
            {
                UsuarioID = dto.UsuarioID,
                Nombre = dto.Nombre,
                ApellidoPaterno = dto.ApellidoPaterno,
                ApellidoMaterno = dto.ApellidoMaterno,
                Telefono = dto.Telefono
            };

            var creado = await _repo.CreateAsync(admin);
            return ToDTO(creado);
        }

        public async Task UpdateAsync(int id, UpdateAdministradorDTO dto)
        {
            var admin = await _repo.GetByIdAsync(id);
            if (admin == null) throw new KeyNotFoundException($"Administrador {id} no encontrado.");

            admin.Nombre = dto.Nombre;
            admin.ApellidoPaterno = dto.ApellidoPaterno;
            admin.ApellidoMaterno = dto.ApellidoMaterno;
            admin.Telefono = dto.Telefono;

            await _repo.UpdateAsync(admin);
        }

        public async Task DesactivarAsync(int id)
        {
            var admin = await _repo.GetByIdAsync(id);
            if (admin == null) throw new KeyNotFoundException($"Administrador {id} no encontrado.");

            await _repo.DesactivarAsync(id);
        }

        // Mapeo de Model -> DTO
        private static AdministradorDTO ToDTO(Administrador a) => new AdministradorDTO
        {
            AdministradorID = a.AdministradorID,
            UsuarioID = a.UsuarioID,
            Nombre = a.Nombre,
            ApellidoPaterno = a.ApellidoPaterno,
            ApellidoMaterno = a.ApellidoMaterno,
            Telefono = a.Telefono
        };
    }


}
