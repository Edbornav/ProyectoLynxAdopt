using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdoptantesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<AdoptanteDTO>> GetAll()
        {
            return Ok(new List<AdoptanteDTO>());
        }

        [HttpGet("{id}")]
        public ActionResult<AdoptanteDTO> GetById(int id)
        {
            return NotFound();
        }

        [HttpPost]
        public ActionResult<AdoptanteDTO> Create([FromBody] CreateAdoptanteDTO dto)
        {
            return Ok(new AdoptanteDTO());
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] UpdateAdoptanteDTO dto)
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
