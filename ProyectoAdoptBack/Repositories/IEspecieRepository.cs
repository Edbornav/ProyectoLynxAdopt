using ProyectoAdoptBack.Models;
using Dapper;
using Npgsql;

namespace ProyectoAdoptBack.Repositories
{
    public interface IEspecieRepository
    {
        Task<IEnumerable<Especie>> GetAllAsync();
        Task<Especie?> GetByIdAsync(int id);
        Task CreateAsync(Especie especie);
        Task UpdateAsync(int id, Especie especie);
    }

    public class EspecieRepository : IEspecieRepository
    {
        private readonly IConfiguration _configuration;

        public EspecieRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private NpgsqlConnection CreateConnection()
            => new(_configuration.GetConnectionString("PostgreSQL"));

        public async Task<IEnumerable<Especie>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Especie>(
                "SELECT * FROM sp_get_especies();");
        }

        public async Task<Especie?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Especie>(
                "SELECT * FROM sp_get_especie_by_id(@p_id);",
                new { p_id = id });
        }

        public async Task CreateAsync(Especie especie)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_insert_especie(@p_nombre);",
                new { p_nombre = especie.Nombre });
        }

        public async Task UpdateAsync(int id, Especie especie)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_update_especie(@p_id, @p_nombre);",
                new
                {
                    p_id = id,
                    p_nombre = especie.Nombre
                });
        }
    }
}
