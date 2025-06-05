using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Application.DTOs.Response;

namespace LibraryManagement.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(AuthRequest request);
        Task LogoutAsync(LogoutRequest request);
        Task<IntrospectResponse> IntrospectAsync(IntrospectRequest request);
        Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
