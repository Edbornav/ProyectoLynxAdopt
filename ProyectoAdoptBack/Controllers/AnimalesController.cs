using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<AnimalesDTO>> GetAll()
        {
            return Ok(new List<AnimalesDTO>());
        }

        [HttpGet("{id}")]
        public ActionResult<AnimalesDTO> GetById(int id)
        {
            return NotFound();
        }

        [HttpPost]
        public ActionResult<AnimalesDTO> Create([FromBody] CreateAnimalesDTO dto)
        {
            return Ok(new AnimalesDTO());
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] UpdateAnimalesDTO dto)
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
