using Dapper;
using Npgsql;
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
        private readonly IConfiguration _configuration;

        public AnimalesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

         private NpgsqlConnection CreateConnection()=> new(_configuration.GetConnectionString("DefaultConnection"));


        public async Task<IEnumerable<Animales>> GetAllAsync()
        {
            const string sql = "Select sp_get_animales";

            using var conn = CreateConnection();
            return await conn.QueryAsync<Animales>(sql);
        }

        public async Task<Animales?> GetByIdAsync(int id)
        {
            const string sql = "Select * from sp_get_animal_by_id(@p_id)";

            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Animales>(sql, new { p_id = id });
        }

        public async Task<Animales> CreateAsync(Animales animales)
        {
            const string sql = @"Select sp_insert_animal
                                    @p_nombre, @p_especie, @p_raza,
                                    @p_edad, @p_sexo, @p_descripcion, @p_fotourl";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_nombre = animales.Nombre,
                P_RazaID = animales.RazaID,
                p_Sexi = animales.Sexo,
                p_FechaNacimiento = animales.FechaNacimiento,
                p_descripcion = animales.Descripcion,
                p_Estatus = animales.Estatus,
            });

            return animales;
        }

        public async Task UpdateAsync(Animales animales)
        {
            const string sql = @"Select * from sp_update_animal
                                    @p_id, @p_nombre, @p_especie, @p_raza,
                                    @p_edad, @p_sexo, @p_descripcion, @p_fotourl";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
              p_nombre = animales.Nombre,
                P_RazaID = animales.RazaID,
                p_Sexi = animales.Sexo,
                p_FechaNacimiento = animales.FechaNacimiento,
                p_descripcion = animales.Descripcion,
                p_Estatus = animales.Estatus,

            });
        }

        public async Task DesactivarAsync(int id)
        {
            const string sql = "Select * from sp_desactivar_animal(@p_id)";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new { p_id = id });
        }
    }
}