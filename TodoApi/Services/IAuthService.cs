using TodoApi.DTOs;

namespace TodoApi.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        string GenerateJwtToken(int userId, string email, string username);
    }
}
