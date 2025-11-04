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
    public class VentaController : ControllerBase
    {
        private readonly VentaService _ventaService;
        private readonly ILogger<VentaController> _logger;

        public VentaController(VentaService ventaService, ILogger<VentaController> logger)
        {
            _ventaService = ventaService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _ventaService.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _ventaService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VentaDto dto)
        {
            var v = await _ventaService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = v.Id }, v);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] VentaDto dto) => Ok(await _ventaService.UpdateAsync(dto));

        [HttpDelete("permanent/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ventaService.DeletePermanentAsync(id);
            return Ok(new { message = "Venta eliminada" });
        }

        [HttpPut("logico/{id:int}")]
        public async Task<IActionResult> DeleteLogical(int id)
        {
            await _ventaService.DeleteLogicalAsync(id);
            return Ok(new { message = "Venta eliminada lógicamente" });
        }
    }
}
