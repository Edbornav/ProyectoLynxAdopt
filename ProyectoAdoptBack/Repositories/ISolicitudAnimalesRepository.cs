using Dapper;
using Npgsql;
using ProyectoAdoptBack.Models;

namespace ProyectoAdoptBack.Repositories
{
    public interface ISolicitudAnimalesRepository
    {
        Task<IEnumerable<SolicitudAnimales>> GetBySolicitudAsync(int solicitudId);
        Task CreateAsync(SolicitudAnimales solicitudAnimal);
    }

    public class SolicitudAnimalesRepository : ISolicitudAnimalesRepository
    {
        private readonly IConfiguration _configuration;

        public SolicitudAnimalesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private NpgsqlConnection CreateConnection()
            => new(_configuration.GetConnectionString("DefaultConnection"));

        public async Task<IEnumerable<SolicitudAnimales>> GetBySolicitudAsync(int solicitudId)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<SolicitudAnimales>(
                "SELECT * FROM sp_get_animales_by_solicitud(@p_solicitudid);",
                new { p_solicitudid = solicitudId });
        }

        public async Task CreateAsync(SolicitudAnimales solicitudAnimal)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_insert_solicitud_animal(@p_solicitudid, @p_animalid);",
                new
                {
                    p_solicitudid = solicitudAnimal.SolicitudID,
                    p_animalid = solicitudAnimal.AnimalID
                });
        }
    }
}