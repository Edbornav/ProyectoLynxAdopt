using Dapper;
using Npgsql;
using ProyectoAdoptBack.Models;
namespace ProyectoAdoptBack.Repositories
{
    public interface IRefugioAdministradoresRepository
    {
        //Procesos que se debe implementar  si o si
        Task<IEnumerable<RefugioAdministradores>> GetAllAsync();
        Task<RefugioAdministradores?> GetByIdAsync(int refugioId, int adminId);
        Task CreateAsync(RefugioAdministradores refugioAdministradores);
        
    }
    public class RefugioAdministradoresRepository : IRefugioAdministradoresRepository  //Contrato con la interfaz
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
            return await connection.QueryAsync<RefugioAdministradores>("SELECT * FROM sp_get_refugio_administradores();");
        }

        public async Task<RefugioAdministradores?> GetByIdAsync(int refugioId, int adminId)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<RefugioAdministradores>(
                "SELECT * FROM sp_get_refugio_administrador_by_ids(@p_RefugioID, @p_AdministradorID);",
                new { p_RefugioID = refugioId, p_AdministradorID = adminId });
        }

        public async Task CreateAsync(RefugioAdministradores refugioAdministradores)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_insert_refugio_administrador(@p_RefugioAdministradorID, @p_RefugioID, @p_AdministradorID);",
                new
                {
                    p_RefugioAdministradorID = refugioAdministradores.RefugioAdministradorID,
                    p_RefugioID = refugioAdministradores.RefugioID,
                    p_AdministradorID = refugioAdministradores.AdministradorID
                });
        }

       

    }
}
