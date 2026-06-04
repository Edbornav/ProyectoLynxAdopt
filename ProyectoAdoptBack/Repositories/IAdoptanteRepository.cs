using Dapper;
using Npgsql;
using ProyectoAdoptBack.Models;

namespace ProyectoAdoptBack.Repositories
{
    public interface IAdoptanteRepository
    {
        Task<IEnumerable<Adoptante>> GetAllAsync();
        Task<Adoptante?> GetByIdAsync(int id);
        Task<Adoptante> CreateAsync(Adoptante adoptante);
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
            => new(_configuration.GetConnectionString("DefaultConnection"));

        public async Task<IEnumerable<Adoptante>> GetAllAsync()
        {
            const string sql = "Select * from sp_get_adoptantes()";

            using var conn = CreateConnection();
            return await conn.QueryAsync<Adoptante>(sql);
        }
  
        public async Task<Adoptante?> GetByIdAsync(int id)
        {
            const string sql = "Select * from sp_get_adoptante_by_id(@p_id)";

            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Adoptante>(sql, new { p_id = id });
        }

        public async Task<Adoptante> CreateAsync(Adoptante adoptante)
        {
            const string sql = @"Select * from sp_insert_adoptante
                                    @p_usuarioid, @p_nombre,
                                    @p_apellidopaterno, @p_apellidomaterno,
                                    @p_telefono, @p_fechanacimiento";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_usuarioid = adoptante.UsuarioID,
                p_nombre = adoptante.Nombre,
                p_apellidopaterno = adoptante.ApellidoPaterno,
                p_apellidomaterno = adoptante.ApellidoMaterno,
                p_telefono = adoptante.Telefono,
                p_fechanacimiento = adoptante.FechaNacimiento
            });

            return adoptante;
        }

        public async Task UpdateAsync(Adoptante adoptante)
        {
            const string sql = @"Select * from sp_update_adoptante
                                    @p_adoptanteid, @p_usuarioid, @p_nombre,
                                    @p_apellidopaterno, @p_apellidomaterno,
                                    @p_telefono, @p_fechanacimiento";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_adoptanteid = adoptante.AdoptanteID,
                p_usuarioid = adoptante.UsuarioID,
                p_nombre = adoptante.Nombre,
                p_apellidopaterno = adoptante.ApellidoPaterno,
                p_apellidomaterno = adoptante.ApellidoMaterno,
                p_telefono = adoptante.Telefono,
                p_fechanacimiento = adoptante.FechaNacimiento
            });
        }

        public async Task DesactivarAsync(int id)
        {
            const string sql = "Select * from sp_desactivar_adoptante(@p_id)";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new { p_id = id });
        }
    }
}
