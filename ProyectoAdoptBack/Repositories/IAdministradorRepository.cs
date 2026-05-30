using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoAdoptBack.Models;

namespace ProyectoAdoptBack.Repositories
{
    public interface IAdministradorRepository
    {
        Task<IEnumerable<Administrador>> GetAllAsync();
        Task<Administrador?> GetByIdAsync(int id);
        Task<Administrador> CreateAsync(Administrador administrador);
        Task UpdateAsync(Administrador administrador);
        Task DesactivarAsync(int id);
    }

    public class AdministradorRepository : IAdministradorRepository
    {
        private readonly string _connectionString;

        public AdministradorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Administrador>> GetAllAsync()
        {
            const string sql = "EXEC sp_get_administradores";

            using var conn = CreateConnection();
            return await conn.QueryAsync<Administrador>(sql);
        }

        public async Task<Administrador?> GetByIdAsync(int id)
        {
            const string sql = "EXEC sp_get_administrador_by_id @p_id";

            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Administrador>(sql, new { p_id = id });
        }

        public async Task<Administrador> CreateAsync(Administrador administrador)
        {
            const string sql = @"EXEC sp_insert_administrador
                                    @p_usuarioid, @p_nombre,
                                    @p_apellidopaterno, @p_apellidomaterno,
                                    @p_telefono";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_usuarioid = administrador.UsuarioID,
                p_nombre = administrador.Nombre,
                p_apellidopaterno = administrador.ApellidoPaterno,
                p_apellidomaterno = administrador.ApellidoMaterno,
                p_telefono = administrador.Telefono
            });

            return administrador;
        }

        public async Task UpdateAsync(Administrador administrador)
        {
            const string sql = @"EXEC sp_update_administrador
                                    @p_id, @p_usuarioid, @p_nombre,
                                    @p_apellidopaterno, @p_apellidomaterno,
                                    @p_telefono";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_id = administrador.AdministradorID,
                p_usuarioid = administrador.UsuarioID,
                p_nombre = administrador.Nombre,
                p_apellidopaterno = administrador.ApellidoPaterno,
                p_apellidomaterno = administrador.ApellidoMaterno,
                p_telefono = administrador.Telefono
            });
        }

        public async Task DesactivarAsync(int id)
        {
            const string sql = "EXEC sp_desactivar_administrador @p_id";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new { p_id = id });
        }
    }
}