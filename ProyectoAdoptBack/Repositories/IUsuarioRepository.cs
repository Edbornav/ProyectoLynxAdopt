using ProyectoAdoptBack.Models;
using Dapper;
using Npgsql;
namespace ProyectoAdoptBack.Repositories
{
   public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task CreateAsync(Usuario usuario);
        Task UpdateAsync(int id, Usuario usuario);
        Task DesactivarAsync(int id);
    }

    public class UsuarioRepository : IUsuarioRepository  //Contrato con la interfaz
    {
        private readonly IConfiguration _configuration;

        public UsuarioRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private NpgsqlConnection CreateConnection()
            => new(_configuration.GetConnectionString("DefaultConnection"));

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Usuario>("SELECT * FROM sp_get_usuarios();");
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Usuario>(
                "SELECT * FROM sp_get_usuario_by_id(@p_id);",
                new { p_id = id });
        }

        public async Task CreateAsync(Usuario usuario)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_insert_usuario(@p_usuarioID, @p_Correo, @p_TipoUsuario, @p_Estatus, @p_fechaRegistro);",
                new
                {
                p_usuarioID = usuario.UsuarioID,
                p_Correo = usuario.Correo,
                p_TipoUsuario = usuario.TipoUsuario,
                p_Estatus = usuario.Estatus,
                p_fechaRegistro = usuario.FechaRegistro
                });
        }

        public async Task UpdateAsync(int id, Usuario usuario)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_update_usuario(@p_id, @p_usuarioID, @p_Correo, @p_TipoUsuario, @p_Estatus, @p_fechaRegistro);",
                new
                {
                    p_id = id,
                    p_usuarioID = usuario.UsuarioID,
                    p_Correo = usuario.Correo,
                    p_TipoUsuario = usuario.TipoUsuario,
                    p_Estatus = usuario.Estatus,
                    p_fechaRegistro = usuario.FechaRegistro
                });
        }

        public async Task DesactivarAsync(int id)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_desactivar_usuario(@p_id);",
                new { p_id = id });
        }
    }
}
