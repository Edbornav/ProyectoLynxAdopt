using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Services;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdoptantesController : ControllerBase
    {
        private readonly IAdoptanteService _service;

        public AdoptantesController(IAdoptanteService service)
        {
            _service = service;
        }
        [Authorize(Roles ="Administrador")]
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
        [AllowAnonymous]
        [HttpGet("por-usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId)
        {
            var item = await _service.GetByUsuarioAsync(usuarioId);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAdoptanteDTO dto)
        {
            var item = await _service.CreateAsync(dto);
            return Ok(item);
        }
        [AllowAnonymous]
        [HttpPost("con-foto")]
        public async Task<IActionResult> CreateConFoto([FromForm] CreateAdoptanteConFotoRequest request)
        {
            var id = await _service.CreateConFotoAsync(request);
            return Ok(new { id });
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAdoptanteDTO dto)
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