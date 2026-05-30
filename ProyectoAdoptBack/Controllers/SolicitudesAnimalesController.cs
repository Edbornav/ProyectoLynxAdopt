using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudesAnimalesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<SolicitudAnimalesDTO>> GetAll()
        {
            return Ok(new List<SolicitudAnimalesDTO>());
        }

        [HttpGet("{solicitudId}/{animalId}")]
        public ActionResult<SolicitudAnimalesDTO> GetById(int solicitudId, int animalId)
        {
            return NotFound();
        }

        [HttpPost]
        public ActionResult<SolicitudAnimalesDTO> Create([FromBody] CreateSolicitudAnimalesDTO dto)
        {
            return Ok(new SolicitudAnimalesDTO());
        }

    }
}
