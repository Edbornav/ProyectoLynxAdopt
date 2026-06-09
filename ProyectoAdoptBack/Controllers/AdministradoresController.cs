using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Services;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AdministradoresController : ControllerBase
    {
        private readonly IAdministradorService _service;

        public AdministradoresController(IAdministradorService service)
        {
            _service = service;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }
        [Authorize(Roles ="Administrador")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAdministradorDTO dto)
        {
            await _service.CreateAsync(dto);
            return Ok();
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAdministradorDTO dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }
        [Authorize]
        [HttpPatch("{id}/desactivar")]
        public async Task<IActionResult> Desactivar(int id)
        {
            await _service.DesactivarAsync(id);
            return NoContent();
        }
    }
}