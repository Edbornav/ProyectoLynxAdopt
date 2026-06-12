using Dapper;
using Npgsql;
using ProyectoAdoptBack.Models;

namespace ProyectoAdoptBack.Repositories
{
    public interface IAnimalesRepository
    {
        Task<IEnumerable<Animales>> GetAllAsync();
        Task<Animales?> GetByIdAsync(int id);
        Task<IEnumerable<Animales>> GetByRefugioAsync(int refugioId);
        Task<IEnumerable<Animales>> GetDisponiblesAsync();
        Task<int> CreateAsync(Animales animales);
        Task UpdateAsync(int id, Animales animales);
        Task DesactivarAsync(int id);
    }

    public class AnimalesRepository : IAnimalesRepository
    {
        private readonly IConfiguration _configuration;

        public AnimalesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private NpgsqlConnection CreateConnection()
            => new(_configuration.GetConnectionString("PostgreSQL"));

        private const string Columnas = "a.AnimalID, a.RefugioID, a.RazaID, a.Nombre, a.Sexo, a.FechaNacimiento, a.Descripcion, a.Estatus, a.FechaRegistro";
        private const string FotoUrlSubquery = "(SELECT i.Url FROM Imagenes i WHERE i.EntidadTipo='Animal' AND i.EntidadID=a.AnimalID ORDER BY i.Orden LIMIT 1) AS FotoUrl";

        public async Task<IEnumerable<Animales>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Animales>(
                $"SELECT {Columnas}, {FotoUrlSubquery} FROM Animales a;"); 
        }

        public async Task<Animales?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Animales>( 
                $"SELECT {Columnas}, {FotoUrlSubquery} FROM Animales a WHERE a.AnimalID = @p_id;", 
                new { p_id = id });
        }

        public async Task<IEnumerable<Animales>> GetByRefugioAsync(int refugioId)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Animales>(
                $"SELECT {Columnas}, {FotoUrlSubquery} FROM Animales a WHERE a.RefugioID = @p_refugioid;", 
                new { p_refugioid = refugioId });
        }

        public async Task<IEnumerable<Animales>> GetDisponiblesAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Animales>(
                $"SELECT {Columnas}, {FotoUrlSubquery} FROM Animales a WHERE a.Estatus = 'Disponible';"); 
        }

        public async Task<int> CreateAsync(Animales animales)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(
                "SELECT sp_insert_animal(@p_refugioid, @p_razaid, @p_nombre, @p_sexo, @p_fechanacimiento::date, @p_descripcion, @p_estatus);",
                new
                {
                    p_refugioid = animales.RefugioID,
                    p_razaid = animales.RazaID,
                    p_nombre = animales.Nombre,
                    p_sexo = animales.Sexo,
                    p_fechanacimiento = animales.FechaNacimiento,
                    p_descripcion = animales.Descripcion,
                    p_estatus = animales.Estatus
                });
        }

        public async Task UpdateAsync(int id, Animales animales)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_update_animal(@p_id, @p_razaid, @p_nombre, @p_sexo, @p_fechanacimiento::date, @p_descripcion, @p_estatus);",
                new
                {
                    p_id = id,
                    p_razaid = animales.RazaID,
                    p_nombre = animales.Nombre,
                    p_sexo = animales.Sexo,
                    p_fechanacimiento = animales.FechaNacimiento,
                    p_descripcion = animales.Descripcion,
                    p_estatus = animales.Estatus
                });
        }

        public async Task DesactivarAsync(int id)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_desactivar_animal(@p_id);",
                new { p_id = id });
        }
    }
}
