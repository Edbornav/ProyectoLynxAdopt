using ProyectoAdoptBack.Models;
using Dapper;
using Microsoft.Data.SqlClient;
namespace ProyectoAdoptBack.Repositories
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario> CreateAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task DesactivarAsync(int id);
    }

    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            const string sql = "EXEC sp_get_usuarios";
            using var conn = CreateConnection();
            return await conn.QueryAsync<Usuario>(sql);
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            const string sql = "EXEC sp_get_usuario_by_id @p_id";
            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Usuario>(sql, new { p_id = id });
        }

        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            const string sql = @"EXEC sp_insert_usuario
                                    @p_correo, @p_tipousuario,
                                    @p_estatus, @p_fecharegistro";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_correo = usuario.Correo,
                p_tipousuario = usuario.TipoUsuario,
                p_estatus = usuario.Estatus,
                p_fecharegistro = usuario.FechaRegistro
            });

            return usuario;
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            const string sql = @"EXEC sp_update_usuario
                                    @p_id, @p_correo,
                                    @p_tipousuario, @p_estatus";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_id = usuario.UsuarioID,
                p_correo = usuario.Correo,
                p_tipousuario = usuario.TipoUsuario,
                p_estatus = usuario.Estatus
            });
        }

        public async Task DesactivarAsync(int id)
        {
            const string sql = "EXEC sp_desactivar_usuario @p_id";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new { p_id = id });
        }
    }
}
