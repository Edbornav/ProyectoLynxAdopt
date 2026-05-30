using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoAdoptBack.Models;
namespace ProyectoAdoptBack.Repositories
{
    public interface IRefugioRepository
    {
        Task<IEnumerable<Refugio>> GetAllAsync();
        Task<Refugio?> GetByIdAsync(int id);
        Task<Refugio> CreateAsync(Refugio refugio);
        Task<Refugio> UpdateAsync(Refugio refugio);
        Task DesactivarAsync(int id);
    }

    public class RefugioRepository : IRefugioRepository
    {
        private readonly string _connectionString;

        public RefugioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Refugio>> GetAllAsync()
        {
            const string sql = "SELECT * FROM sp_get_refugios()";
            using var conn = CreateConnection();
            return await conn.QueryAsync<Refugio>(sql);
        }

        public async Task<Refugio?> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM sp_get_refugio_by_id(@p_id)";
            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Refugio>(sql, new { p_id = id });
        }

        public async Task<Refugio> CreateAsync(Refugio refugio)
        {
            const string sql = @"EXEC sp_insert_refugio 
                                    @p_nombre, @p_descripcion, @p_direccion,
                                    @p_telefono, @p_correo, @p_estatus, @p_fechaderegistro";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_nombre = refugio.Nombre,
                p_descripcion = refugio.Descripcion,
                p_direccion = refugio.Direccion,
                p_telefono = refugio.Telefono,
                p_correo = refugio.Correo,
                p_estatus = refugio.Estatus,
                p_fechaderegistro = refugio.FechaDeRegistro
            });

            return refugio;
        }

        public async Task<Refugio> UpdateAsync(Refugio refugio)
        {
            const string sql = @"EXEC sp_insert_refugio 
                                    @p_nombre, @p_descripcion, @p_direccion,
                                    @p_telefono, @p_correo, @p_estatus, @p_fechaderegistro";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_nombre = refugio.Nombre,
                p_descripcion = refugio.Descripcion,
                p_direccion = refugio.Direccion,
                p_telefono = refugio.Telefono,
                p_correo = refugio.Correo,
                p_estatus = refugio.Estatus,
                p_fechaderegistro = refugio.FechaDeRegistro
            });

            return refugio;
        }

        public async Task DesactivarAsync(int id)
        {
            const string sql = "EXEC sp_desactivar_refugio @p_id";
            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new { p_id = id });

        }
    }
}

