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
                "SELECT ImagenID, EntidadTipo, EntidadID, Url, Orden, NombreArchivo, FechaSubida FROM sp_get_imagenes_by_entidad(@p_entidadtipo, @p_entidadid);", 
                new
                {
                    p_entidadtipo = entidadTipo,
                    p_entidadid = entidadId
                });
        }

        public async Task<int> CreateAsync(Imagen imagen)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync( // cambio sp_insert_imagen retorna VOID, no debe usarse ExecuteScalar
                "SELECT sp_insert_imagen(@p_entidadtipo, @p_entidadid, @p_url, @p_orden, @p_nombrearchivo);", 
                new
                {
                    p_entidadtipo = imagen.EntidadTipo,
                    p_entidadid = imagen.EntidadID,
                    p_url = imagen.Url,
                    p_orden = imagen.Orden,
                    p_nombrearchivo = imagen.NombreArchivo
                });
            return imagen.ImagenID; // cambio se mantiene la firma sin esperar retorno de la funcion VOID
        }

        public async Task<Imagen?> DeleteAsync(int imagenId)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "SELECT sp_delete_imagen(@p_id);", // cambio (sp_delete_imagen retorna VOID, no debe consultarse con SELECT *)
                new { p_id = imagenId }); // cambio (parametro igual al script SQL)
            return null; // cambio (la funcion SQL no devuelve imagen eliminada)
        }

        public async Task<IEnumerable<Imagen>> DeleteByEntidadAsync(string entidadTipo, int entidadId)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Imagen>(
                "DELETE FROM Imagenes WHERE EntidadTipo = @p_entidadtipo AND EntidadID = @p_entidadid RETURNING ImagenID, EntidadTipo, EntidadID, Url, Orden, NombreArchivo, FechaSubida;", // cambio (se reemplazo funcion inexistente por SQL PostgreSQL valido)
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
                "UPDATE Imagenes SET Orden = @p_orden WHERE ImagenID = @p_imagenid;", // cambio (se reemplazo funcion inexistente por SQL PostgreSQL valido)
                new
                {
                    p_imagenid = imagenId,
                    p_orden = orden
                });
        }
    }
}
