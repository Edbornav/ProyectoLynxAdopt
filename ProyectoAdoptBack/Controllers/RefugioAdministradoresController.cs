using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Services;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Administrador")]
    public class RefugioAdministradoresController : ControllerBase
    {
        private readonly IRefugioAdministradoresService _service;

        public RefugioAdministradoresController(IRefugioAdministradoresService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }
        [HttpGet("{refugioId}")]
        public async Task<IActionResult> GetByRefugio(int refugioId)
        {
            var items = await _service.GetByRefugioAsync(refugioId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRefugioAdministradoresDTO dto)
        {
            await _service.CreateAsync(dto);
            return Ok();
        }
    }
}