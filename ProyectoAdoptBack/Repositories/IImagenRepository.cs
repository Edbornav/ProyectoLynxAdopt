using Dapper;
using Npgsql;
using ProyectoAdoptBack.Models;

namespace ProyectoAdoptBack.Repositories
{
    public interface IImagenRepository
    {
        Task<IEnumerable<Imagen>> GetByEntidadAsync(string entidadTipo, int entidadId);
        Task<int> CreateAsync(Imagen imagen);
        Task<Imagen?> DeleteAsync(int imagenId);
        Task<IEnumerable<Imagen>> DeleteByEntidadAsync(string entidadTipo, int entidadId);
        Task ReordenarAsync(int imagenId, int orden);
    }

    public class ImagenRepository : IImagenRepository
    {
        private readonly IConfiguration _configuration;

        public ImagenRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private NpgsqlConnection CreateConnection()
            => new(_configuration.GetConnectionString("DefaultConnection"));

        public async Task<IEnumerable<Imagen>> GetByEntidadAsync(string entidadTipo, int entidadId)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Imagen>(
                "SELECT * FROM sp_get_imagenes_by_entidad(@p_entidadtipo, @p_entidadid);",
                new
                {
                    p_entidadtipo = entidadTipo,
                    p_entidadid = entidadId
                });
        }

        public async Task<int> CreateAsync(Imagen imagen)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(
                "SELECT sp_insert_imagen(@p_entidadtipo, @p_entidadid, @p_url, @p_orden, @p_nombrearchivo);",
                new
                {
                    p_entidadtipo = imagen.EntidadTipo,
                    p_entidadid = imagen.EntidadID,
                    p_url = imagen.Url,
                    p_orden = imagen.Orden,
                    p_nombrearchivo = imagen.NombreArchivo
                });
        }

        public async Task<Imagen?> DeleteAsync(int imagenId)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Imagen>(
                "SELECT * FROM sp_delete_imagen(@p_imagenid);",
                new { p_imagenid = imagenId });
        }

        public async Task<IEnumerable<Imagen>> DeleteByEntidadAsync(string entidadTipo, int entidadId)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Imagen>(
                "SELECT * FROM sp_delete_imagenes_by_entidad(@p_entidadtipo, @p_entidadid);",
                new
                {
                    p_entidadtipo = entidadTipo,
                    p_entidadid = entidadId
                });
        }

        public async Task ReordenarAsync(int imagenId, int orden)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_reordenar_imagenes(@p_imagenid, @p_orden);",
                new
                {
                    p_imagenid = imagenId,
                    p_orden = orden
                });
        }
    }
}