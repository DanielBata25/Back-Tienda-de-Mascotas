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
    public class PagoController : ControllerBase
    {
        private readonly PagoService _pagoService;
        private readonly ILogger<PagoController> _logger;

        public PagoController(PagoService pagoService, ILogger<PagoController> logger)
        {
            _pagoService = pagoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _pagoService.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _pagoService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PagoDto dto)
        {
            var p = await _pagoService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = p.Id }, p);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PagoDto dto) => Ok(await _pagoService.UpdateAsync(dto));

        [HttpDelete("permanent/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _pagoService.DeletePermanentAsync(id);
            return Ok(new { message = "Pago eliminado" });
        }

        [HttpPut("logico/{id:int}")]
        public async Task<IActionResult> DeleteLogical(int id)
        {
            await _pagoService.DeleteLogicalAsync(id);
            return Ok(new { message = "Pago eliminado lógicamente" });
        }
    }
}
