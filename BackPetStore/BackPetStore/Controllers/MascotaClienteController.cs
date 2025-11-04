using Business.Services;
using Entity.Context;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MascotaClienteController : ControllerBase
    {
        private readonly MascotaClienteService _service;
        private readonly ILogger<MascotaClienteController> _logger;

        public MascotaClienteController(MascotaClienteService service, ILogger<MascotaClienteController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MascotaClienteDto dto)
        {
            var mc = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = mc.Id }, mc);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MascotaClienteDto dto) => Ok(await _service.UpdateAsync(dto));

        [HttpDelete("permanent/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeletePermanentAsync(id);
            return Ok(new { message = "Relación Mascota-Cliente eliminada" });
        }

        [HttpPut("logico/{id:int}")]
        public async Task<IActionResult> DeleteLogical(int id)
        {
            await _service.DeleteLogicalAsync(id);
            return Ok(new { message = "Relación Mascota-Cliente eliminada lógicamente" });
        }
    }
}
