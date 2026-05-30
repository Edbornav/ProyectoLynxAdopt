using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoAdoptBack.Models;

namespace ProyectoAdoptBack.Repositories
{
    public interface ISolicitudAdopcionRepository
    {
        Task<IEnumerable<SolicitudAdopcion>> GetAllAsync();
        Task<SolicitudAdopcion?> GetByIdAsync(int id);
        Task<SolicitudAdopcion> CreateAsync(SolicitudAdopcion solicitud);
        Task UpdateAsync(SolicitudAdopcion solicitud);
        Task DesactivarAsync(int id);
    }

    public class SolicitudAdopcionRepository: ISolicitudAdopcionRepository
    {
        private readonly string _connectionString;

        public SolicitudAdopcionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<SolicitudAdopcion>> GetAllAsync()
        {
            const string sql = "EXEX sp_get_solicitudes_adopcion()";
            using var conn = CreateConnection();
            return await conn.QueryAsync<SolicitudAdopcion>(sql);
        }

        public async Task<SolicitudAdopcion?> GetByIdAsync(int id)
        {
            const string sql = "EXEC sp_get_solicitud_adopcion_by_id @p_id";
            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<SolicitudAdopcion>(sql, new { p_id = id });
        }


        public async Task<SolicitudAdopcion> CreateAsync(SolicitudAdopcion solicitud)
        {
            const string sql = @"EXEC sp_insert_solicitud_adopcion
                                    @p_refugioId, @p_adoptanteId,
                                    @p_mensajeadoptante, @p_estatus, @p_fecharegistro";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_refugioId = solicitud.RefugioID,
                p_adoptanteId = solicitud.AdoptanteID,
                p_mensajeadoptante = solicitud.MensajeAdoptante,
                p_estatus = solicitud.Estatus,
                p_fecharegistro = solicitud.FechaRegistro
            });

            return solicitud;
        }

        public async Task UpdateAsync(SolicitudAdopcion solicitud)
        {
            const string sql = @"EXEC sp_update_solicitud_adopcion
                                    @p_id, @p_refugioId, @p_adoptanteId,
                                    @p_mensajeadoptante, @p_estatus";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_id = solicitud.SolicitudID,
                p_refugioId = solicitud.RefugioID,
                p_adoptanteId = solicitud.AdoptanteID,
                p_mensajeadoptante = solicitud.MensajeAdoptante,
                p_estatus = solicitud.Estatus
            });
        }

        public async Task DesactivarAsync(int id)
        {
            const string sql = "EXEC sp_desactivar_solicitud_adopcion @p_id";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new { p_id = id });
        }

    }
}
