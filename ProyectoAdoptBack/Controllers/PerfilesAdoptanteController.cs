using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Services;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class PerfilesAdoptanteController : ControllerBase
    {
        private readonly IPerfilAdoptanteService _service;

        public PerfilesAdoptanteController(IPerfilAdoptanteService service)
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
         [Authorize(Roles ="Administrador")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }
        [Authorize(Roles ="Adoptante")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePerfilAdoptanteDTO dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(new { id });
        }
        [Authorize(Roles ="Adoptante")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePerfilAdoptanteDTO dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }
    }
}