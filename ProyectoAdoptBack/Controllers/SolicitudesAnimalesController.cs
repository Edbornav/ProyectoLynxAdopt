using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAdoptBack.DTOs;
using ProyectoAdoptBack.Services;

namespace ProyectoAdoptBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudesAnimalesController : ControllerBase
    {
        private readonly ISolicitudAnimalesService _service;

        public SolicitudesAnimalesController(ISolicitudAnimalesService service)
        {
            _service = service;
        }
        [Authorize]
        [HttpGet("{solicitudId}")]
        public async Task<IActionResult> GetBySolicitud(int solicitudId)
        {
            var items = await _service.GetBySolicitudAsync(solicitudId);
            return Ok(items);
        }
        [Authorize(Roles ="Adoptante")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSolicitudAnimalesDTO dto)
        {
            await _service.CreateAsync(dto);
            return Ok(new { });
        }
    }
}