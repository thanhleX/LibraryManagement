using System.Security.Claims;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Settings;

namespace LibraryManagement.Application.Interfaces.Services
{
    public interface IJwtTokenService
    {
        JwtTokenResult GenerateToken(User user);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
