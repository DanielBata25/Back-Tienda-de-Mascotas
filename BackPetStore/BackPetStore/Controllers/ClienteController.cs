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
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _clienteService;
        private readonly ILogger<ClienteController> _logger;
        private readonly ApplicationDbContext _context;

        public ClienteController(ClienteService clienteService, ApplicationDbContext context, ILogger<ClienteController> logger)
        {
            _clienteService = clienteService;
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ClienteDto>), 200)]
        public async Task<IActionResult> GetAll()
        {
            try { return Ok(await _clienteService.GetAllAsync()); }
            catch (Exception ex) { _logger.LogError(ex, "Error al obtener clientes"); return StatusCode(500, new { ex.Message }); }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try { return Ok(await _clienteService.GetByIdAsync(id)); }
            catch (Exception ex) { _logger.LogError(ex, "Error al obtener cliente"); return StatusCode(500, new { ex.Message }); }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClienteDto dto)
        {
            try { var c = await _clienteService.CreateAsync(dto); return CreatedAtAction(nameof(GetById), new { id = c.Id }, c); }
            catch (Exception ex) { _logger.LogError(ex, "Error al crear cliente"); return StatusCode(500, new { ex.Message }); }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ClienteDto dto)
        {
            try { return Ok(await _clienteService.UpdateAsync(dto)); }
            catch (Exception ex) { _logger.LogError(ex, "Error al actualizar cliente"); return StatusCode(500, new { ex.Message }); }
        }

        [HttpDelete("permanent/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try { await _clienteService.DeletePermanentAsync(id); return Ok(new { message = "Cliente eliminado" }); }
            catch (Exception ex) { _logger.LogError(ex, "Error al eliminar cliente"); return StatusCode(500, new { ex.Message }); }
        }

        [HttpPut("logico/{id:int}")]
        public async Task<IActionResult> DeleteLogical(int id)
        {
            try { await _clienteService.DeleteLogicalAsync(id); return Ok(new { message = "Cliente eliminado lógicamente" }); }
            catch (Exception ex) { _logger.LogError(ex, "Error al eliminar cliente lógico"); return StatusCode(500, new { ex.Message }); }
        }
    }
}
