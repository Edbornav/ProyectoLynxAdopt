using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<UsuarioDTO>> GetAll()
        {
            return Ok(new List<UsuarioDTO>());
        }

        [HttpGet("{id}")]
        public ActionResult<UsuarioDTO> GetById(int id)
        {
            return NotFound();
        }

        [HttpPost]
        public ActionResult<UsuarioDTO> Create([FromBody] CreateUsuarioDTO dto)
        {
            return Ok(new UsuarioDTO());
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] UpdateUsuarioDTO dto)
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
