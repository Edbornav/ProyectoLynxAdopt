using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Services;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefugiosController : ControllerBase
    {
        private readonly IRefugioService _service;

        public RefugiosController(IRefugioService service)
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }
        [Authorize(Roles ="Administrador")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRefugioDTO dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(new { id });
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost("con-logo")]
        public async Task<IActionResult> CreateConLogo([FromForm] CreateRefugioConLogoRequest request)
        {
            var id = await _service.CreateConLogoAsync(request);
            return Ok(new { id });
        }
        [Authorize(Roles ="Administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRefugioDTO dto)
        {
            await _service.UpdateAsync(id, dto);
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