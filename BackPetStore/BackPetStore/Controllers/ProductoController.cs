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
    public class ProductoController : ControllerBase
    {
        private readonly ProductoService _productoService;
        private readonly ILogger<ProductoController> _logger;

        public ProductoController(ProductoService productoService, ILogger<ProductoController> logger)
        {
            _productoService = productoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _productoService.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _productoService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductoDto dto)
        {
            var p = await _productoService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = p.Id }, p);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProductoDto dto) => Ok(await _productoService.UpdateAsync(dto));

        [HttpDelete("permanent/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productoService.DeletePermanentAsync(id);
            return Ok(new { message = "Producto eliminado" });
        }

        [HttpPut("logico/{id:int}")]
        public async Task<IActionResult> DeleteLogical(int id)
        {
            await _productoService.DeleteLogicalAsync(id);
            return Ok(new { message = "Producto eliminado lógicamente" });
        }
    }
}
