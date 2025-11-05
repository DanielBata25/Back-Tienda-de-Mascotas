using Business.Interfaces;
using Entity.Context;
using Entity.DTOs;
using Entity.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ApplicationDbContext context, ILogger<AuthController> logger)
        {
            _authService = authService;
            _context = context;
            _logger = logger;
        }

 
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var token = await _authService.AuthenticateAsync(loginDto);

                if (token == null)
                    return Unauthorized(new { message = "Credenciales incorrectas o cliente inactivo/eliminado." });

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el proceso de login");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Verificar correo duplicado
                var existe = await _context.Clientes.AnyAsync(c => c.Email == dto.Email && !c.IsDeleted);
                if (existe)
                    return BadRequest(new { message = "Ya existe un cliente registrado con este correo electrónico." });

                // Crear el cliente
                var cliente = new Cliente
                {
                    Nombre = dto.Nombre,
                    Apellido = dto.Apellido,
                    Email = dto.Email,
                    Telefono = dto.Telefono,
                    Direccion = dto.Direccion,
                    Password = HashPassword(dto.Password),
                    IsDeleted = false
                };

                await _context.Clientes.AddAsync(cliente);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation($"✅ Nuevo cliente registrado: {cliente.Nombre} {cliente.Apellido} ({cliente.Email})");

                return Ok(new
                {
                    message = "Registro exitoso.",
                    cliente = new
                    {
                        cliente.Id,
                        cliente.Nombre,
                        cliente.Apellido,
                        cliente.Email,
                        cliente.Telefono,
                        cliente.Direccion
                    }
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error durante el registro del cliente.");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        
        [HttpGet("clientes")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetClientes()
        {
            try
            {
                var clientes = await _context.Clientes
                    .Where(c => !c.IsDeleted)
                    .Select(c => new
                    {
                        c.Id,
                        c.Nombre,
                        c.Apellido,
                        c.Email,
                        c.Telefono,
                        c.Direccion
                    })
                    .ToListAsync();

                return Ok(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los clientes.");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        
        [HttpGet("test")]
        [Authorize]
        public IActionResult Test()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var nombre = User.FindFirstValue("nombre");
            var rol = User.FindFirstValue("rol") ?? "Cliente";

            return Ok(new
            {
                message = $"Hola {nombre ?? email}, accediste con éxito al área protegida.",
                rol
            });
        }

        [HttpGet("admin-only")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnly()
        {
            return Ok(new { message = "Accediste como administrador." });
        }

        [HttpGet("cliente-only")]
        [Authorize(Roles = "Cliente")]
        public IActionResult ClienteOnly()
        {
            return Ok(new { message = "Accediste como cliente autenticado." });
        }

        
        private static string HashPassword(string password)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
