using Dapper;
using Npgsql;
using ProyectoAdoptBack.Models;

namespace ProyectoAdoptBack.Repositories
{
    public interface ISolicitudAdopcionRepository
    {
        Task<IEnumerable<SolicitudAdopcion>> GetAllAsync();
        Task<SolicitudAdopcion?> GetByIdAsync(int id);
        Task<IEnumerable<SolicitudAdopcion>> GetByAdoptanteAsync(int adoptanteId);
        Task<IEnumerable<SolicitudAdopcion>> GetByRefugioAsync(int refugioId);
        Task CreateAsync(SolicitudAdopcion solicitud);
        Task UpdateEstatusAsync(int id, string estatus);
        Task DesactivarAsync(int id);
    }

    public class SolicitudAdopcionRepository : ISolicitudAdopcionRepository
    {
        private readonly IConfiguration _configuration;

        public SolicitudAdopcionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private NpgsqlConnection CreateConnection()
            => new(_configuration.GetConnectionString("DefaultConnection"));

        public async Task<IEnumerable<SolicitudAdopcion>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<SolicitudAdopcion>(
                "SELECT SolicitudID, RefugioID, AdoptanteID, MensajeAdoptante, Estatus, FechaRegistro FROM sp_get_solicitudes();"); 
        }

        public async Task<SolicitudAdopcion?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<SolicitudAdopcion>(
                "SELECT SolicitudID, RefugioID, AdoptanteID, MensajeAdoptante, Estatus, FechaRegistro FROM sp_get_solicitud_by_id(@p_id);", 
                new { p_id = id });
        }

        public async Task<IEnumerable<SolicitudAdopcion>> GetByAdoptanteAsync(int adoptanteId)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<SolicitudAdopcion>(
                "SELECT SolicitudID, RefugioID, AdoptanteID, MensajeAdoptante, Estatus, FechaRegistro FROM sp_get_solicitudes_by_adoptante(@p_adoptanteid);", 
                new { p_adoptanteid = adoptanteId });
        }

        public async Task<IEnumerable<SolicitudAdopcion>> GetByRefugioAsync(int refugioId)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<SolicitudAdopcion>(
                "SELECT SolicitudID, RefugioID, AdoptanteID, MensajeAdoptante, Estatus, FechaRegistro FROM sp_get_solicitudes_by_refugio(@p_refugioid);", 
                new { p_refugioid = refugioId });
        }

        public async Task CreateAsync(SolicitudAdopcion solicitud)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_insert_solicitud(@p_refugioid, @p_adoptanteid, @p_mensajeadoptante);",
                new
                {
                    p_refugioid = solicitud.RefugioID,
                    p_adoptanteid = solicitud.AdoptanteID,
                    p_mensajeadoptante = solicitud.MensajeAdoptante
                });
        }

        public async Task UpdateEstatusAsync(int id, string estatus)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_update_estatus_solicitud(@p_id, @p_estatus);",
                new
                {
                    p_id = id,
                    p_estatus = estatus
                });
        }

        public async Task DesactivarAsync(int id)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_desactivar_solicitud(@p_id);",
                new { p_id = id });
        }
    }
}
