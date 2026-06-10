using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Services;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudesAdopcionController : ControllerBase
    {
        private readonly ISolicitudAdopcionService _service;

        public SolicitudesAdopcionController(ISolicitudAdopcionService service)
        {
            _service = service;
        }
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }
        [Authorize]
        [HttpGet("por-adoptante/{adoptanteId}")]
        public async Task<IActionResult> GetByAdoptante(int adoptanteId)
        {
            var items = await _service.GetByAdoptanteAsync(adoptanteId);
            return Ok(items);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("por-refugio/{refugioId}")]
        public async Task<IActionResult> GetByRefugio(int refugioId)
        {
            var items = await _service.GetByRefugioAsync(refugioId);
            return Ok(items);
        }

        [Authorize(Roles ="Adoptante")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSolicitudAdopcionDTO dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(new { id });
        }
        [Authorize(Roles ="Administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEstatus(int id, [FromBody] UpdateSolicitudAdopcionDTO dto)
        {
            await _service.UpdateEstatusAsync(id, dto);
            return NoContent();
        }
        [Authorize(Roles ="Administrador")]
        [HttpPatch("{id}/desactivar")]
        public async Task<IActionResult> Desactivar(int id)
        {
            await _service.DesactivarAsync(id);
            return NoContent();
        }
    }
}