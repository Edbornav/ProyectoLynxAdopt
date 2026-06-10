using ProyectoAdoptBack.Models;
using Dapper;
using Npgsql;
namespace ProyectoAdoptBack.Repositories
{
   public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario?> LoginAsync(string correo);
        Task <int>CreateAsync(Usuario usuario);
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
            => new(_configuration.GetConnectionString("PostgreSQL"));

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Usuario>("SELECT UsuarioID, Correo, TipoUsuario, Estatus, FechaRegistro FROM sp_get_usuarios();"); 
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Usuario>(
                "SELECT UsuarioID, Correo, TipoUsuario, Estatus, FechaRegistro FROM sp_get_usuario_by_id(@p_id);", 
                new { p_id = id });
        }

        public async Task<Usuario?> LoginAsync(string correo)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Usuario>(
                "SELECT UsuarioID, Correo, PasswordHash, TipoUsuario, Estatus FROM sp_login(@p_correo);",
                new { p_correo = correo });
        }

        public async Task<int> CreateAsync(Usuario usuario)
        {
            using var connection = CreateConnection();

            return await connection.ExecuteScalarAsync<int>(
                //Agregado @p_passwordhash para que coincida con la firma del SP sp_insert_usuario(p_correo, p_passwordhash, p_tipousuario, p_estatus)
                "SELECT sp_insert_usuario(@p_correo, @p_passwordhash, @p_tipousuario, @p_estatus);",
                new
                {
                    p_correo = usuario.Correo,
                    p_passwordhash = usuario.PasswordHash, //Agregado: envia el hash generado en el service
                    p_tipousuario = usuario.TipoUsuario,
                    p_estatus = usuario.Estatus

                });
        }

        public async Task UpdateAsync(int id, Usuario usuario)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_update_usuario(@p_id, @p_correo, @p_tipousuario, @p_estatus);", 
                new
                {
                    p_id = id,
                    p_correo = usuario.Correo, 
                    p_tipousuario = usuario.TipoUsuario, 
                    p_estatus = usuario.Estatus 
                    // cambios (parametro igual al script SQL)
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
