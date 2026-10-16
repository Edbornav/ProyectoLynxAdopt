using Dapper;
using Npgsql;
using ProyectoAdoptBack.Models;

namespace ProyectoAdoptBack.Repositories
{
    public interface IAdoptanteRepository
    {
        Task<IEnumerable<Adoptante>> GetAllAsync();
        Task<Adoptante?> GetByIdAsync(int id);
        Task<Adoptante?> GetByUsuarioAsync(int usuarioId);
        Task<int> CreateAsync(Adoptante adoptante);
        Task UpdateAsync(Adoptante adoptante);
        Task DesactivarAsync(int id);
    }

    public class AdoptanteRepository : IAdoptanteRepository
    {
        private readonly IConfiguration _configuration;

        public AdoptanteRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private NpgsqlConnection CreateConnection()
            => new(_configuration.GetConnectionString("PostgreSQL"));

        public async Task<IEnumerable<Adoptante>> GetAllAsync()
        {
            const string sql = "SELECT AdoptanteID, UsuarioID, Nombre, ApellidoPaterno, ApellidoMaterno, Telefono, FechaNacimiento FROM sp_get_adoptantes();"; 

            using var conn = CreateConnection();
            return await conn.QueryAsync<Adoptante>(sql);
        }
  
        public async Task<Adoptante?> GetByIdAsync(int id)
        {
            const string sql = "SELECT AdoptanteID, UsuarioID, Nombre, ApellidoPaterno, ApellidoMaterno, Telefono, FechaNacimiento FROM sp_get_adoptante_by_id(@p_id);"; 

            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Adoptante>(sql, new { p_id = id });
        }

        public async Task<Adoptante?> GetByUsuarioAsync(int usuarioId)
        {
            const string sql = "SELECT AdoptanteID, UsuarioID, Nombre, ApellidoPaterno, ApellidoMaterno, Telefono, FechaNacimiento FROM sp_get_adoptantes() WHERE usuarioid = @p_usuarioid;";

            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Adoptante>(sql, new { p_usuarioid = usuarioId });
        }

        public async Task<int> CreateAsync(Adoptante adoptante)
        {
            using var connection = CreateConnection();
        
            return await connection.ExecuteScalarAsync<int>(
                "SELECT sp_insert_adoptante(@p_usuarioid, @p_nombre, @p_apellidopaterno, @p_apellidomaterno, @p_telefono, CAST(@p_fechanacimiento AS DATE));",
                new
            {
                p_usuarioid = adoptante.UsuarioID,
                p_nombre = adoptante.Nombre,
                p_apellidopaterno = adoptante.ApellidoPaterno,
                p_apellidomaterno = adoptante.ApellidoMaterno,
                p_telefono = adoptante.Telefono,
                p_fechanacimiento = adoptante.FechaNacimiento
            });

    
        }

        public async Task UpdateAsync(Adoptante adoptante)
        {
            const string sql = @"SELECT sp_update_adoptante(@p_id, @p_nombre, @p_apellidopaterno, @p_apellidomaterno, @p_telefono, CAST(@p_fechanacimiento AS DATE));"; // cambio sqlQL

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_id = adoptante.AdoptanteID, // cambio parametro igual al script SQL
                p_nombre = adoptante.Nombre,
                p_apellidopaterno = adoptante.ApellidoPaterno,
                p_apellidomaterno = adoptante.ApellidoMaterno,
                p_telefono = adoptante.Telefono,
                p_fechanacimiento = adoptante.FechaNacimiento
            });
        }

        public async Task DesactivarAsync(int id)
        {
            const string sql = "SELECT sp_desactivar_adoptante(@p_id);"; // cambio 

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new { p_id = id });
        }
    }
}
