using Dapper;
using Npgsql;
using ProyectoAdoptBack.Models;

namespace ProyectoAdoptBack.Repositories
{
    public interface ICitasRepository
    {
        Task<IEnumerable<Citas>> GetAllAsync();
        Task<Citas?> GetByIdAsync(int id);
        Task<IEnumerable<Citas>> GetBySolicitudAsync(int solicitudId);
        Task CreateAsync(Citas cita);
        Task UpdateAsync(int id, Citas cita);
        Task UpdateEstadoAsync(int id, string estadoCita);
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
            => new(_configuration.GetConnectionString("PostgreSQL"));

        public async Task<IEnumerable<Citas>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Citas>(
                "SELECT CitaID, SolicitudID, FechaHoraCita, EstadoCita FROM sp_get_citas();");
        }

        public async Task<Citas?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Citas>(
                "SELECT CitaID, SolicitudID, FechaHoraCita, EstadoCita FROM sp_get_cita_by_id(@p_id);", 
                new { p_id = id });
        }

        public async Task<IEnumerable<Citas>> GetBySolicitudAsync(int solicitudId)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Citas>(
                "SELECT CitaID, SolicitudID, FechaHoraCita, EstadoCita FROM sp_get_citas_by_solicitud(@p_solicitudid);", 
                new { p_solicitudid = solicitudId });
        }

        public async Task CreateAsync(Citas cita)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_insert_cita(@p_solicitudid, @p_fechahoracita, @p_estadocita);",
                new
                {
                    p_solicitudid = cita.SolicitudID,
                    p_fechahoracita = cita.FechaHoraCita,
                    p_estadocita = cita.EstadoCita
                });
        }

        public async Task UpdateAsync(int id, Citas cita)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_update_cita(@p_id, @p_fechahoracita, @p_estadocita);",
                new
                {
                    p_id = id,
                    p_fechahoracita = cita.FechaHoraCita,
                    p_estadocita = cita.EstadoCita
                });
        }

        public async Task UpdateEstadoAsync(int id, string estadoCita)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_update_estado_cita(@p_id, @p_estadocita);",
                new
                {
                    p_id = id,
                    p_estadocita = estadoCita
                });
        }

        public async Task DesactivarAsync(int id)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_desactivar_cita(@p_id);",
                new { p_id = id });
        }
    }
}
