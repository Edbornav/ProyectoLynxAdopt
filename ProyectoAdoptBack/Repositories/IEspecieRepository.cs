using ProyectoAdoptBack.Models;
using Dapper;
using Npgsql;
namespace ProyectoAdoptBack.Repositories
{
    public interface IEspecieRepository
    {
        Task<IEnumerable<Especie>> GetEspecies();
        Task<Especie?> GetByIdAsync(int id); 
        Task<Especie> CreateAsync(Especie especie);
        Task UpdateAsync(Especie especie);

    } 

    public class EspecieRepository : IEspecieRepository
    {
        private readonly String _connectionString;
        public EspecieRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        private NpgsqlConnection CreateConnection() => new NpgsqlConnection(_connectionString);
        public async Task<IEnumerable<Especie>> GetEspecies()
        {
            const string sql = "SELECT * FROM sp_get_especies()";
            using var conn = CreateConnection();
            return await conn.QueryAsync<Especie>(sql);
        }
        public async Task<Especie?> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM sp_get_especie_by_id(@Id) ";
            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Especie>(sql, new { Id = id });
        }
        public async Task<Especie> CreateAsync(Especie especie)
        {
            const string sql = @"SELECT SP_insert_especie(@p_nombre) ";
            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_nombre = especie.Nombre
            });
            return especie;
        }
        public async Task UpdateAsync(Especie especie)
        {
            const string sql = @"SELECT SP_update_especie(@p_id, @p_nombre) ";
            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_id = especie.EspecieID,
                p_nombre = especie.Nombre
            });
        }
    }
}