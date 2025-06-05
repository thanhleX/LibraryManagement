using System.Security.Claims;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.DTOs.Response;

namespace LibraryManagement.Application.Interfaces.Services
{
    public interface IJwtService
    {
        TokenInfo GenerateTokens(User user);
        Task<ClaimsPrincipal> ValidateToken(string token);
    }
}
