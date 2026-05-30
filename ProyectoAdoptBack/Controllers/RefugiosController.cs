using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefugiosController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<RefugioDTO>> GetAll()
        {
            return Ok(new List<RefugioDTO>());
        }

        [HttpGet("{id}")]
        public ActionResult<RefugioDTO> GetById(int id)
        {
            return NotFound();
        }

        [HttpPost]
        public ActionResult<RefugioDTO> Create([FromBody] CreateRefugioDTO dto)
        {
            return Ok(new RefugioDTO());
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] UpdateRefugioDTO dto)
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
