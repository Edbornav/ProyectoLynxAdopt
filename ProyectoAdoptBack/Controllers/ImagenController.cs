using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Services;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ImagenController : ControllerBase
    {
        private readonly IImagenService _service;

        public ImagenController(IImagenService service)
        {
            _service = service;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetByEntidad([FromQuery] string entidadTipo, [FromQuery] int entidadId)
        {
            if (string.IsNullOrWhiteSpace(entidadTipo) || entidadId <= 0)
                return BadRequest("entidadTipo y entidadId son requeridos.");
            var items = await _service.GetByEntidadAsync(entidadTipo, entidadId);
            return Ok(items);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateImagenDTO dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(new { id });
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _service.DeleteAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteByEntidad([FromQuery] string entidadTipo, [FromQuery] int entidadId)
        {
            if (string.IsNullOrWhiteSpace(entidadTipo) || entidadId <= 0)
                return BadRequest("entidadTipo y entidadId son requeridos.");
            var items = await _service.DeleteByEntidadAsync(entidadTipo, entidadId);
            return Ok(items);
        }
        [Authorize]
        [HttpPatch("{id}/reordenar")]
        public async Task<IActionResult> Reordenar(int id, [FromBody] int orden)
        {
            await _service.ReordenarAsync(id, orden);
            return NoContent();
        }
    }
}