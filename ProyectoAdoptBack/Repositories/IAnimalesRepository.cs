using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoAdoptBack.Models;

namespace ProyectoAdoptBack.Repositories
{
    public interface IAnimalesRepository
    {
        Task<IEnumerable<Animales>> GetAllAsync();
        Task<Animales?> GetByIdAsync(int id);
        Task<Animales> CreateAsync(Animales animales);
        Task UpdateAsync(Animales animales);
        Task DesactivarAsync(int id);
    }

    public class AnimalesRepository : IAnimalesRepository
    {
        private readonly string _connectionString;

        public AnimalesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Animales>> GetAllAsync()
        {
            const string sql = "EXEC sp_get_animales";

            using var conn = CreateConnection();
            return await conn.QueryAsync<Animales>(sql);
        }

        public async Task<Animales?> GetByIdAsync(int id)
        {
            const string sql = "EXEC sp_get_animal_by_id @p_id";

            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Animales>(sql, new { p_id = id });
        }

        public async Task<Animales> CreateAsync(Animales animales)
        {
            const string sql = @"EXEC sp_insert_animal
                                    @p_nombre, @p_especie, @p_raza,
                                    @p_edad, @p_sexo, @p_descripcion, @p_fotourl";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_nombre = animales.Nombre,
                p_especie = animales.Especie,
                p_raza = animales.Raza,
                p_edad = animales.Edad,
                p_sexo = animales.Sexo,
                p_descripcion = animales.Descripcion,
                p_fotourl = animales.FotoUrl
            });

            return animales;
        }

        public async Task UpdateAsync(Animales animales)
        {
            const string sql = @"EXEC sp_update_animal
                                    @p_id, @p_nombre, @p_especie, @p_raza,
                                    @p_edad, @p_sexo, @p_descripcion, @p_fotourl";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_id = animales.AnimalID,
                p_nombre = animales.Nombre,
                p_especie = animales.Especie,
                p_raza = animales.Raza,
                p_edad = animales.Edad,
                p_sexo = animales.Sexo,
                p_descripcion = animales.Descripcion,
                p_fotourl = animales.FotoUrl
            });
        }

        public async Task DesactivarAsync(int id)
        {
            const string sql = "EXEC sp_desactivar_animal @p_id";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new { p_id = id });
        }
    }
}