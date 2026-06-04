using Dapper;
using Npgsql;
using ProyectoAdoptBack.Models;

namespace ProyectoAdoptBack.Repositories
{
    public interface IPerfilAdoptanteRepository
    {
        Task<IEnumerable<PerfilAdoptante>> GetAllAsync();
        Task<PerfilAdoptante?> GetByIdAsync(int id);
        Task<PerfilAdoptante> CreateAsync(PerfilAdoptante perfil);
        Task UpdateAsync(PerfilAdoptante perfil);
    }


    public class PerfilAdoptanteRepository: IPerfilAdoptanteRepository
    {
        private readonly string _connectionString;

        public PerfilAdoptanteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private NpgsqlConnection CreateConnection() => new NpgsqlConnection(_connectionString);

        public async Task<IEnumerable<PerfilAdoptante>> GetAllAsync()
        {
            const string sql = "SELECT * FROM sp_get_perfiles_adoptante()";

            using var conn = CreateConnection();
            return await conn.QueryAsync<PerfilAdoptante>(sql);
        }

        public async Task<PerfilAdoptante?> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM sp_get_perfil_adoptante_by_id(@p_id)";

            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<PerfilAdoptante>(sql, new { p_id = id });
        }

        public async Task<PerfilAdoptante> CreateAsync(PerfilAdoptante perfil)
        {
            const string sql = @"SELECT sp_insert_perfil_adoptante(
                                    @p_adoptanteusuarioid,
                                    @p_descripcioncasa,
                                    @p_descripcionmascotas,
                                    @p_descripcionexperienciaconmascotas)";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_adoptanteusuarioid = perfil.AdoptanteUsuarioID,
                p_descripcioncasa = perfil.DescripcionCasa,
                p_descripcionmascotas = perfil.DescripcionMascotas,
                p_descripcionexperienciaconmascotas = perfil.DescripcionExperienciaConMascotas
            });

            return perfil;
        }

        public async Task UpdateAsync(PerfilAdoptante perfil)
        {
            const string sql = @"SELECT sp_update_perfil_adoptante(
                                    @p_id,
                                    @p_descripcioncasa,
                                    @p_descripcionmascotas,
                                    @p_descripcionexperienciaconmascotas)";

            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                p_id = perfil.PerfilAdoptanteID,
                p_descripcioncasa = perfil.DescripcionCasa,
                p_descripcionmascotas = perfil.DescripcionMascotas,
                p_descripcionexperienciaconmascotas = perfil.DescripcionExperienciaConMascotas
            });

        
        }
    }
}