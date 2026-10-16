using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Services;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalesController : ControllerBase
    {
        private readonly IAnimalesService _service;

        public AnimalesController(IAnimalesService service)
        {
            _service = service;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }
        [AllowAnonymous]
        [HttpGet("por-refugio/{refugioId}")]
        public async Task<IActionResult> GetByRefugio(int refugioId)
        {
            var items = await _service.GetByRefugioAsync(refugioId);
            return Ok(items);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAnimalesDTO dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(new { id });
        }
        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAnimalesDTO dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok(new { });
        }
        [Authorize(Roles = "Administrador")]
        [HttpPatch("{id}/desactivar")]
        public async Task<IActionResult> Desactivar(int id)
        {
            await _service.DesactivarAsync(id);
            return NoContent();
        }
    }
}