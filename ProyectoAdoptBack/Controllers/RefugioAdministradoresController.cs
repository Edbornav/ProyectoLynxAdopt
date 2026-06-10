using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Services;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefugioAdministradoresController : ControllerBase
    {
        private readonly IRefugioAdministradoresService _service;

        public RefugioAdministradoresController(IRefugioAdministradoresService service)
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
        [HttpGet("{refugioId}")]
        public async Task<IActionResult> GetByRefugio(int refugioId)
        {
            var items = await _service.GetByRefugioAsync(refugioId);
            return Ok(items);
        }

        [AllowAnonymous]
        [HttpGet("por-administrador/{administradorId}")]
        public async Task<IActionResult> GetByAdministrador(int administradorId)
        {
            var items = await _service.GetByAdministradorAsync(administradorId);
            return Ok(items);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRefugioAdministradoresDTO dto)
        {
            await _service.CreateAsync(dto);
            return Ok();
        }
    }
}