using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RazasController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<RazaDTO>> GetAll()
        {
            return Ok(new List<RazaDTO>());
        }

        [HttpGet("{id}")]
        public ActionResult<RazaDTO> GetById(int id)
        {
            return NotFound();
        }

        [HttpPost]
        public ActionResult<RazaDTO> Create([FromBody] CreateRazaDTO dto)
        {
            return Ok(new RazaDTO());
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] UpdateRazaDTO dto)
        {
            return NoContent();
        }

    }
}
