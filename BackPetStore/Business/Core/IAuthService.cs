using Entity.DTOs;
using Entity.Model;

namespace Business.Interfaces
{
    public interface IAuthService
    {
        Task<string?> AuthenticateAsync(LoginDto loginDto);
        Task<Cliente?> RegisterAsync(RegisterDto registerDto);
    }
}
