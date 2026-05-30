using Dapper;
using Microsoft.Data.SqlClient;
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
        private readonly string _connectionString;

        public CitasRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Citas>> GetCitas()
        {
            const string sql = "EXEC sp_get_citas";

            using var conn = CreateConnection();
            return await conn.QueryAsync<Citas>(sql);
        }

        public async Task<Citas?> GetByIdAsync(int id)
        {
            const string sql = "EXEC sp_get_cita_by_id @Id";

            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Citas>(sql, new { Id = id });
        }

        public async Task<Citas> CreateAsync(Citas citas)
        {
            const string sql = "EXEC sp_insert_cita @p_adoptanteId, @p_animalId, @p_fechaHora, @p_estado";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_adoptanteId = citas.AdoptanteID,
                p_animalId = citas.AnimalID,
                p_fechaHora = citas.FechaHora,
                p_estado = citas.Estado
            });

            return citas;
        }

        
        public async Task UpdateAsync(Citas citas)
        {
            const string sql = "EXEC sp_update_cita @p_id, @p_adoptanteId, @p_animalId, @p_fechaHora, @p_estado";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_id = citas.CitaID,
                p_adoptanteId = citas.AdoptanteID,
                p_animalId = citas.AnimalID,
                p_fechaHora = citas.FechaHora,
                p_estado = citas.Estado
            });
        }


        public async Task DesactivarAsync(int id)
        {
            const string sql = "EXEC sp_desactivar_cita @p_id";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new { p_id = id });
        }
    }
}
