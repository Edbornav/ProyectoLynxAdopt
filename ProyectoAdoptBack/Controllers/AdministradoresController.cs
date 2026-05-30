using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministradoresController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<AdministradorDTO>> GetAll()
        {
            return Ok(new List<AdministradorDTO>());
        }

        [HttpGet("{id}")]
        public ActionResult<AdministradorDTO> GetById(int id)
        {
            return NotFound();
        }

        [HttpPost]
        public ActionResult<AdministradorDTO> Create([FromBody] CreateAdministradorDTO dto)
        {
            return Ok(new AdministradorDTO());
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] UpdateAdministradorDTO dto)
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
