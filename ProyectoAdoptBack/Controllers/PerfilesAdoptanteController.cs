using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilesAdoptanteController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<PerfilAdoptanteDTO>> GetAll()
        {
            return Ok(new List<PerfilAdoptanteDTO>());
        }

        [HttpGet("{id}")]
        public ActionResult<PerfilAdoptanteDTO> GetById(int id)
        {
            return NotFound();
        }

        [HttpPost]
        public ActionResult<PerfilAdoptanteDTO> Create([FromBody] CreatePerfilAdoptanteDTO dto)
        {
            return Ok(new PerfilAdoptanteDTO());
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] UpdatePerfilAdoptanteDTO dto)
        {
            return NoContent();
        }

    }
}
