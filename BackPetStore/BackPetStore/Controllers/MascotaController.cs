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
    public class MascotaController : ControllerBase
    {
        private readonly MascotaService _mascotaService;
        private readonly ILogger<MascotaController> _logger;
        private readonly ApplicationDbContext _context;

        public MascotaController(MascotaService mascotaService, ApplicationDbContext context, ILogger<MascotaController> logger)
        {
            _mascotaService = mascotaService;
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _mascotaService.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _mascotaService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MascotaDto dto)
        {
            var m = await _mascotaService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = m.Id }, m);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MascotaDto dto) => Ok(await _mascotaService.UpdateAsync(dto));

        [HttpDelete("permanent/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mascotaService.DeletePermanentAsync(id);
            return Ok(new { message = "Mascota eliminada" });
        }

        [HttpPut("logico/{id:int}")]
        public async Task<IActionResult> DeleteLogical(int id)
        {
            await _mascotaService.DeleteLogicalAsync(id);
            return Ok(new { message = "Mascota eliminada lógicamente" });
        }
    }
}
