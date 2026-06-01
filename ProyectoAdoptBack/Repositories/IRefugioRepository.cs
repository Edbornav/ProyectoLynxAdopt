using Dapper;
using Npgsql;
using ProyectoAdoptBack.Models;
namespace ProyectoAdoptBack.Repositories
{
    public interface IRefugioRepository
    {
        //Procesos que se debe implementar  si o si
        Task<IEnumerable<Refugio>> GetAllAsync();
        Task<Refugio?> GetByIdAsync(int id);
        Task CreateAsync(Refugio refugio);
        Task UpdateAsync(int id, Refugio refugio);
        Task DesactivarAsync(int id);
    }
    public class RefugioRepository : IRefugioRepository  //Contrato con la interfaz
    {
        private readonly IConfiguration _configuration;

        public RefugioRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private NpgsqlConnection CreateConnection()
            => new(_configuration.GetConnectionString("DefaultConnection"));

        public async Task<IEnumerable<Refugio>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Refugio>("SELECT * FROM sp_get_refugios();");
        }

        public async Task<Refugio?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Refugio>(
                "SELECT * FROM sp_get_refugio_by_id(@p_id);",
                new { p_id = id });
        }

        public async Task CreateAsync(Refugio refugio)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_insert_administrador(@p_usuarioid, @p_nombre, @p_apellidopaterno, @p_apellidomaterno, @p_telefono);",
                new
                {
                   p_RefugioID = refugio.RefugioID,
                   p_Nombre = refugio.Nombre,
                   p_Descripcion = refugio.Descripcion,
                   p_Direccion = refugio.Direccion,
                   p_Telefono = refugio.Telefono,
                   p_Correo = refugio.Correo,
                   p_Estatus = refugio.Estatus,
                   p_FechaDeRegistro = refugio.FechaDeRegistro
                });
        }

        public async Task UpdateAsync(int id, Refugio refugio)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_update_refugio(@p_id, @p_refugioid, @p_nombre, @p_descripcion, @p_direccion, @p_telefono, @p_correo, @p_estatus, @p_fechaderegistro);",
                new
                {
                    //Verrificar los valores que puede cambiar un refugio antes de probarlos y subirlos a main
                   p_id = id,
                   p_RefugioID = refugio.RefugioID,
                   p_Nombre = refugio.Nombre,
                   p_Descripcion = refugio.Descripcion,
                   p_Direccion = refugio.Direccion,
                   p_Telefono = refugio.Telefono,
                   p_Correo = refugio.Correo,
                   p_Estatus = refugio.Estatus,
                   p_FechaDeRegistro = refugio.FechaDeRegistro
                });
        }

        public async Task DesactivarAsync(int id)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_desactivar_refugio(@p_id);",
                new { p_id = id });
        }
    }

}

