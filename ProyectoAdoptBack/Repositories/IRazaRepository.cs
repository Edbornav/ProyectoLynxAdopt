using Npgsql;
using Dapper;
using ProyectoAdoptBack.Models;

namespace ProyectoAdoptBack.Repositories
{
    public interface IRazaRepository
    {
        Task<IEnumerable<Raza>> GetAllAsync();
        Task<Raza?> GetbyIdAsync(int id);
        Task<IEnumerable<Raza>> GetByEspecieAsync(int especieId); 
        Task<int> CreateAsync(Raza raza);
        Task<Raza> UpdateAsync(Raza raza);

    }

    public class RazaRepository : IRazaRepository
    {    
        private readonly String _connectionString;

        public RazaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgreSQL")!;

        }

        private NpgsqlConnection CreateConnection() => new NpgsqlConnection(_connectionString);

        public async Task<IEnumerable<Raza>> GetAllAsync()
        {
            const string sql = "SELECT RazaID, EspecieID, Nombre FROM sp_get_razas();"; 
            using var conn = CreateConnection();
            return await conn.QueryAsync<Raza>(sql);
        }

        public async Task<Raza?> GetbyIdAsync(int id)
        {
            const string sql = "SELECT RazaID, EspecieID, Nombre FROM sp_get_raza_by_id(@p_id);"; 
            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Raza>(sql, new { p_id = id }); // parametro igual al script sql
        }

        public async Task<IEnumerable<Raza>> GetByEspecieAsync(int especieId) 
        {
            const string sql = "SELECT RazaID, EspecieID, Nombre FROM sp_get_razas_by_especie(@p_especieid);"; 
            using var conn = CreateConnection();
            return await conn.QueryAsync<Raza>(sql, new { p_especieid = especieId }); 
        }

        public async Task<int> CreateAsync(Raza raza)
        {
            const string sql = @"SELECT sp_insert_raza(@p_especieid, @p_nombre);"; 
            using var conn = CreateConnection();
            return await conn.ExecuteScalarAsync<int>(sql, new
            {
                p_especieid = raza.EspecieID, 
                p_nombre = raza.Nombre 
            });
        }

        public async Task<Raza> UpdateAsync(Raza raza)
        {
            const string sql = @"SELECT sp_update_raza(@p_id, @p_especieid, @p_nombre);"; 
            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_id = raza.RazaID,
                p_especieid = raza.EspecieID, 
                p_nombre = raza.Nombre 
            });
            return raza;

        }
    }
}
