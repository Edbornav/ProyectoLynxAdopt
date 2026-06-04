using Dapper;
using Npgsql;
using ProyectoAdoptBack.Models;

namespace ProyectoAdoptBack.Repositories
{
    public interface IRefugioAdministradoresRepository
    {
        Task<IEnumerable<RefugioAdministradores>> GetAllAsync();
        Task<IEnumerable<RefugioAdministradores>> GetByRefugioAsync(int refugioId);
        Task CreateAsync(RefugioAdministradores refugioAdministradores);
    }

    public class RefugioAdministradoresRepository : IRefugioAdministradoresRepository
    {
        private readonly IConfiguration _configuration;

        public RefugioAdministradoresRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private NpgsqlConnection CreateConnection()
            => new(_configuration.GetConnectionString("DefaultConnection"));

        public async Task<IEnumerable<RefugioAdministradores>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<RefugioAdministradores>(
                "SELECT * FROM sp_get_refugio_administradores();");
        }

        public async Task<IEnumerable<RefugioAdministradores>> GetByRefugioAsync(int refugioId)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<RefugioAdministradores>(
                "SELECT * FROM sp_get_admins_by_refugio(@p_refugioid);",
                new { p_refugioid = refugioId });
        }

        public async Task CreateAsync(RefugioAdministradores refugioAdministradores)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_insert_refugio_administrador(@p_refugioid, @p_usuarioadminid);",
                new
                {
                    p_refugioid = refugioAdministradores.RefugioID,
                    p_usuarioadminid = refugioAdministradores.AdministradorID
                });
        }
    }
}