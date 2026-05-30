using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoAdoptBack.Models;
namespace ProyectoAdoptBack.Repositories
{
    public interface IRefugioAdministradoresRepository
    {
        Task<IEnumerable<RefugioAdministradores>> GetAllAsync();
        Task<RefugioAdministradores?> GetByIdAsync(int refugioId, int adminID);
        Task<RefugioAdministradores> CreateAsync(RefugioAdministradores refugioAdmin);
    }

    public class RefugioAdministradoresRepository: IRefugioAdministradoresRepository

    {
        private readonly string _connectionString;

        public RefugioAdministradoresRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<RefugioAdministradores>> GetAllAsync()
            {
            const string sql = "EXEC sp_get_refugio_administradores>";
            using var connection = CreateConnection();
            return await connection.QueryAsync<RefugioAdministradores>(sql);
        }

        public async Task<RefugioAdministradores?> GetByIdAsync(int refugioId, int adminId)
        {
            const string sql = "EXEC sp_get_refugio_administrador_by_id @p_refugioId, @p_adminId";

            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<RefugioAdministradores>(sql, new
            {
                p_refugioId = refugioId,
                p_adminId = adminId
            });
        }

        public async Task<RefugioAdministradores> CreateAsync(RefugioAdministradores refugioAdmin)
        {
            const string sql = "EXEC sp_insert_refugio_administrador @p_refugioId, @p_adminId";
            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_refugioId = refugioAdmin.RefugioID,
                p_adminId = refugioAdmin.AdministradorID
            });
            return refugioAdmin;
        }


    }
}
