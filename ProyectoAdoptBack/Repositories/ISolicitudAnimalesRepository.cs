using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoAdoptBack.Models;
namespace ProyectoAdoptBack.Repositories
{
    public interface ISolicitudAnimalesRepository
    {
        Task<IEnumerable<SolicitudAnimales>> GetAllAsync();
        Task<SolicitudAnimales?> GetByIdAsync(int solicitudId, int animalId);
        Task<SolicitudAnimales> CreateAsync(SolicitudAnimales solicitudAnimal);
    }

    public class SolicitudAnimalesRepository:ISolicitudAnimalesRepository
    {
        private readonly string _connectionString;

        public SolicitudAnimalesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<SolicitudAnimales>> GetAllAsync()
        {
            const string sql = "EXEC sp_get_solicitud_animales";
            using var conn = CreateConnection();
            return await conn.QueryAsync<SolicitudAnimales>(sql);
        }

        public async Task<SolicitudAnimales?> GetByIdAsync(int solicitudId, int animalId)
        {
            const string sql = "EXEC sp_get_solicitud_animal_by_id @p_solicitudId, @p_animalId";

            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<SolicitudAnimales>(sql, new
            {
                p_solicitudId = solicitudId,
                p_animalId = animalId
            });
        }

        public async Task<SolicitudAnimales> CreateAsync(SolicitudAnimales solicitudAnimal)
        {
            const string sql = "EXEC sp_insert_solicitud_animal @p_solicitudId, @p_animalId";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_solicitudId = solicitudAnimal.SolicitudID,
                p_animalId = solicitudAnimal.AnimalID
            });

            return solicitudAnimal;
        }

    }
}
