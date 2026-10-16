using Dapper;
using Npgsql;
using ProyectoAdoptBack.Models;
//El uso del select y no call, es porque estamos haciendo uso de funciones  y no procedimientos almacenados, por lo que se hace uso de select para llamar a las funciones
namespace ProyectoAdoptBack.Repositories
{
    public interface IAdministradorRepository
    {
        //Procesos que se debe implementar  si o si
        Task<IEnumerable<Administrador>> GetAllAsync();
        Task<Administrador?> GetByIdAsync(int id);
        Task<Administrador?> GetByUsuarioAsync(int usuarioId);
        Task<int> CreateAsync(Administrador administrador);
        Task UpdateAsync(int id, Administrador administrador);
        Task DesactivarAsync(int id);
    }

    public class AdministradorRepository : IAdministradorRepository  //Contrato con la interfaz
    {
        private readonly IConfiguration _configuration;

        public AdministradorRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private NpgsqlConnection CreateConnection()
            => new(_configuration.GetConnectionString("PostgreSQL"));

        public async Task<IEnumerable<Administrador>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Administrador>("SELECT AdministradorID, UsuarioID, Nombre, ApellidoPaterno, ApellidoMaterno, Telefono FROM sp_get_administradores();"); 
        }

        public async Task<Administrador?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Administrador>(
                "SELECT AdministradorID, UsuarioID, Nombre, ApellidoPaterno, ApellidoMaterno, Telefono FROM sp_get_administrador_by_id(@p_id);", 
                new { p_id = id });
        }

        public async Task<Administrador?> GetByUsuarioAsync(int usuarioId)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Administrador>(
                "SELECT AdministradorID, UsuarioID, Nombre, ApellidoPaterno, ApellidoMaterno, Telefono FROM sp_get_administradores() WHERE usuarioid = @p_usuarioid;",
                new { p_usuarioid = usuarioId });
        }

        public async Task<int> CreateAsync(Administrador administrador)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(
                "SELECT sp_insert_administrador(@p_usuarioid, @p_nombre, @p_apellidopaterno, @p_apellidomaterno, @p_telefono);", 
                new
                {
                    p_usuarioid = administrador.UsuarioID,
                    p_nombre = administrador.Nombre,
                    p_apellidopaterno = administrador.ApellidoPaterno,
                    p_apellidomaterno = administrador.ApellidoMaterno,
                    p_telefono = administrador.Telefono
                });
        }

        public async Task UpdateAsync(int id, Administrador administrador)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_update_administrador(@p_id, @p_nombre, @p_apellidopaterno, @p_apellidomaterno, @p_telefono);",
                new
                {
                    p_id = id,
                    p_nombre = administrador.Nombre,
                    p_apellidopaterno = administrador.ApellidoPaterno,
                    p_apellidomaterno = administrador.ApellidoMaterno,
                    p_telefono = administrador.Telefono
                });
        }

        public async Task DesactivarAsync(int id)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_desactivar_administrador(@p_id);",
                new { p_id = id });
        }
    }
}
