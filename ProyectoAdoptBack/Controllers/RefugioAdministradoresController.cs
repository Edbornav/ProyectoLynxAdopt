using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefugioAdministradoresController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<RefugioAdministradoresDTO>> GetAll()
        {
            return Ok(new List<RefugioAdministradoresDTO>());
        }

        [HttpGet("{refugioId}/{adminId}")]
        public ActionResult<RefugioAdministradoresDTO> GetById(int refugioId, int adminId)
        {
            return NotFound();
        }

        [HttpPost]
        public ActionResult<RefugioAdministradoresDTO> Create([FromBody] CreateRefugioAdministradoresDTO dto)
        {
            return Ok(new RefugioAdministradoresDTO());
        }

    }
}
