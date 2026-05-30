using Microsoft.Data.SqlClient;
using Dapper;
using ProyectoAdoptBack.Models;

namespace ProyectoAdoptBack.Repositories
{
    public interface IRazaRepository
    {
        Task<IEnumerable<Raza>> GetAllAsync();
        Task<Raza?> GetbyIdAsync(int id);
        Task<Raza> CreateAsync(Raza raza);
        Task<Raza> UpdateAsync(Raza raza);

    }

    public class RazaRepository : IRazaRepository
    {
        private readonly String _connectionString;

        public RazaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        }

        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Raza>> GetAllAsync()
        {
            const string sql = "SELECT * FROM sp_get_razas()";
            using var conn = CreateConnection();
            return await conn.QueryAsync<Raza>(sql);
        }

        public async Task<Raza?> GetbyIdAsync(int id)
        {
            const string sql = "SELECT * FROM sp_get_raza_by_id(@Id) ";
            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Raza>(sql, new { Id = id });
        }

        public async Task<Raza> CreateAsync(Raza raza)
        {
            const string sql = @"SELECT SP_insert_raza(@p_nombre, @p_especieId) ";
            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_nombre = raza.Nombre,
                p_especieId = raza.EspecieID
            });
            return raza;
        }

        public async Task<Raza> UpdateAsync(Raza raza)
        {
            const string sql = @"SELECT SP_update_raza(@p_id, @p_nombre, @p_especieId) ";
            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_id = raza.RazaID,
                p_nombre = raza.Nombre,
                p_especieId = raza.EspecieID
            });
            return raza;

        }
    }
}
