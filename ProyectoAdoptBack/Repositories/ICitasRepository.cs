using Dapper;
using Npgsql;
using ProyectoAdoptBack.Models;

namespace ProyectoAdoptBack.Repositories
{
    public interface ICitasRepository
    {
        Task<IEnumerable<Citas>> GetCitas();
        Task<Citas?> GetByIdAsync(int id);
        Task<Citas> CreateAsync(Citas citas);
        Task UpdateAsync(Citas citas);
        Task DesactivarAsync(int id);
    }

    public class CitasRepository : ICitasRepository
    {
        private readonly IConfiguration _configuration;

        public CitasRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

       private NpgsqlConnection CreateConnection()
            => new(_configuration.GetConnectionString("DefaultConnection"));

        public async Task<IEnumerable<Citas>> GetCitas()
        {
            const string sql = "Select sp_get_citas";

            using var conn = CreateConnection();
            return await conn.QueryAsync<Citas>(sql);
        }

        public async Task<Citas?> GetByIdAsync(int id)
        {
            const string sql = "Select * from sp_get_cita_by_id(@p_id)";

            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Citas>(sql, new { p_id = id });
        }

       public async Task<Citas> CreateAsync(Citas citas)
        {
            const string sql = @"Select sp_insert_cita
                                    @p_solicitudid, @p_fechahoracita, @p_estadocita";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_solicitudid   = citas.SolicitudID,
                p_fechahoracita = citas.FechaHoraCita,
                p_estadocita    = citas.EstadoCita
            });

            return citas;
        }
        
        public async Task UpdateAsync(Citas citas)
        {
            const string sql = @"Select sp_update_cita
                                    @p_id, @p_fechahoracita, @p_estadocita";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_id            = citas.CitaID,
                p_fechahoracita = citas.FechaHoraCita,
                p_estadocita    = citas.EstadoCita
            });
        }


        public async Task DesactivarAsync(int id)
        {
            const string sql = "Select * from sp_desactivar_cita(@p_id)";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new { p_id = id });
        }
    }
}