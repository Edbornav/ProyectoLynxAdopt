using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagenController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<ImagenDTO>> GetAll()
        {
            return Ok(new List<ImagenDTO>());
        }

        [HttpGet("{id}")]
        public ActionResult<ImagenDTO> GetById(int id)
        {
            return NotFound();
        }

        [HttpPost]
        public ActionResult<ImagenDTO> Create([FromBody] CreateImagenDTO dto)
        {
            return Ok(new ImagenDTO());
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] UpdateImagenDTO dto)
        {
            return NoContent();
        }
    }
}