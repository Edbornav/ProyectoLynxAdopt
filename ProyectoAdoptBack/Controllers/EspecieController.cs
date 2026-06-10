using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Services;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecieController : ControllerBase
    {
        private readonly IEspecieService _service;

        public EspecieController(IEspecieService service)
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
        public async Task<IActionResult> Create([FromBody] CreateEspecieDTO dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(new { id });
        }
        [Authorize(Roles ="Administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEspecieDTO dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }
    }
}