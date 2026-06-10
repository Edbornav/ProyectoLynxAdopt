using Dapper;
using Npgsql;
using ProyectoAdoptBack.Models;

namespace ProyectoAdoptBack.Repositories
{
    public interface IPerfilAdoptanteRepository
    {
        Task<IEnumerable<PerfilAdoptante>> GetAllAsync();
        Task<PerfilAdoptante?> GetByIdAsync(int id);
        Task<int> CreateAsync(PerfilAdoptante perfil);
        Task UpdateAsync(int id, PerfilAdoptante perfil);
    }

    public class PerfilAdoptanteRepository : IPerfilAdoptanteRepository
    {
        private readonly IConfiguration _configuration;

        public PerfilAdoptanteRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private NpgsqlConnection CreateConnection()
            => new(_configuration.GetConnectionString("PostgreSQL"));

        public async Task<IEnumerable<PerfilAdoptante>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<PerfilAdoptante>(
                "SELECT * FROM sp_get_perfiles_adoptante();");
        }

        public async Task<PerfilAdoptante?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<PerfilAdoptante>(
                "SELECT * FROM sp_get_perfil_adoptante_by_id(@p_id);",
                new { p_id = id });
        }

        public async Task<int> CreateAsync(PerfilAdoptante perfil)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(
                "SELECT sp_insert_perfil_adoptante(@p_adoptanteid, @p_descripcioncasa, @p_descripcionmascotas, @p_descripcionexperienciaconmascotas);",
                new
                {
                    p_adoptanteid = perfil.AdoptanteUsuarioID,
                    p_descripcioncasa = perfil.DescripcionCasa,
                    p_descripcionmascotas = perfil.DescripcionMascotas,
                    p_descripcionexperienciaconmascotas = perfil.DescripcionExperienciaConMascotas
                });
        }

        public async Task UpdateAsync(int id, PerfilAdoptante perfil)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_update_perfil_adoptante(@p_id, @p_descripcioncasa, @p_descripcionmascotas, @p_descripcionexperienciaconmascotas);",
                new
                {
                    p_id = id,
                    p_descripcioncasa = perfil.DescripcionCasa,
                    p_descripcionmascotas = perfil.DescripcionMascotas,
                    p_descripcionexperienciaconmascotas = perfil.DescripcionExperienciaConMascotas
                });
        }
    }
}
