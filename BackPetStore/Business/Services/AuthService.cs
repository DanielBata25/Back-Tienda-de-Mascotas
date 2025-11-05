using Business.Interfaces;
using Data.Interfaces;
using Entity.Context;
using Entity.DTOs;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(IClienteRepository clienteRepository, ApplicationDbContext context, IConfiguration configuration)
        {
            _clienteRepository = clienteRepository;
            _context = context;
            _configuration = configuration;
        }

        public async Task<string?> AuthenticateAsync(LoginDto loginDto)
        {
            // Buscar cliente por correo
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Email == loginDto.Email && !c.IsDeleted);

            if (cliente == null)
                return null;

            // Validar contraseña
            if (!VerifyPassword(loginDto.Password, cliente.Password))
                return null;

            // Claims personalizados del cliente
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, cliente.Id.ToString()),
                new Claim(ClaimTypes.Email, cliente.Email),
                new Claim("nombre", $"{cliente.Nombre} {cliente.Apellido}"),
                new Claim(ClaimTypes.Role, "Cliente")
            };

            // Clave secreta y tiempo de expiración
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
            int expirationMinutes = _configuration.GetValue<int>("Jwt:ExpirationMinutes", 60);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public async Task<Cliente?> RegisterAsync(RegisterDto dto)
        {
            // Verificar si el correo ya existe
            var existe = await _context.Clientes.AnyAsync(c => c.Email == dto.Email && !c.IsDeleted);
            if (existe)
                throw new InvalidOperationException("Ya existe un cliente registrado con este correo electrónico.");

            // Crear nuevo cliente
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

            return cliente;
        }


        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var hashedBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private static bool VerifyPassword(string inputPassword, string storedPassword)
        {
            var inputHash = HashPassword(inputPassword);
            // Permite login con texto plano (por compatibilidad) o cifrado
            return storedPassword == inputPassword || storedPassword == inputHash;
        }
    }
}
