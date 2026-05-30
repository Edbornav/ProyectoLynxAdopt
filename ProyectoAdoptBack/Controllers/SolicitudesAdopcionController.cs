using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudesAdopcionController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<SolicitudAdopcionDTO>> GetAll()
        {
            return Ok(new List<SolicitudAdopcionDTO>());
        }

        [HttpGet("{id}")]
        public ActionResult<SolicitudAdopcionDTO> GetById(int id)
        {
            return NotFound();
        }

        [HttpPost]
        public ActionResult<SolicitudAdopcionDTO> Create([FromBody] CreateSolicitudAdopcionDTO dto)
        {
            return Ok(new SolicitudAdopcionDTO());
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] UpdateSolicitudAdopcionDTO dto)
        {
            return NoContent();
        }

        [HttpPatch("{id}/desactivar")]
        public ActionResult Desactivar(int id)
        {
            return NoContent();
        }
    }
}
