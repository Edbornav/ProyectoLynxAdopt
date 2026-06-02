using Dapper;
using Npgsql;   
using ProyectoAdoptBack.Models;

namespace ProyectoAdoptBack.Repositories
{
    public interface ISolicitudAdopcionRepository
    {
        Task<IEnumerable<SolicitudAdopcion>> GetAllAsync();
        Task<SolicitudAdopcion?> GetByIdAsync(int id);
        Task<SolicitudAdopcion> CreateAsync(SolicitudAdopcion solicitud);
        Task UpdateAsync(int id, SolicitudAdopcion solicitud);
        Task DesactivarAsync(int id);
    }

    public class SolicitudAdopcionRepository : ISolicitudAdopcionRepository  //Contrato con la interfaz
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
            return await connection.QueryAsync<SolicitudAdopcion>("SELECT * FROM sp_get_solicitudes_adopcion();");
        }

        public async Task<SolicitudAdopcion?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<SolicitudAdopcion>(
                "SELECT * FROM sp_get_solicitud_by_id(@p_id);",
                new { p_id = id });
        }

        public async Task<SolicitudAdopcion> CreateAsync(SolicitudAdopcion solicitud)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_insert_solicitud( @p_RefugioID, @p_AdoptanteID, @p_MensajeAdoptante, @p_Estatus);",
                new
                {
                    
                    p_RefugioID =  solicitud.RefugioID,
                    p_AdoptanteID = solicitud.AdoptanteID,
                    p_MensajeAdoptante = solicitud.MensajeAdoptante,
                    p_Estatus = solicitud.Estatus,
                    
                });

            return solicitud;
        }

        public async Task UpdateAsync(int id, SolicitudAdopcion solicitud)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_update_solicitud(@p_id, @p_RefugioID, @p_AdoptanteID, @p_MensajeAdoptante, @p_Estatus);",
                new
                {
                    p_id = id,
                    p_RefugioID =  solicitud.RefugioID,
                    p_AdoptanteID = solicitud.AdoptanteID,
                    p_MensajeAdoptante = solicitud.MensajeAdoptante,
                    p_Estatus = solicitud.Estatus
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
    //Nota: Aun hacen falta algunos procedimientos que tenemos realizados
}
