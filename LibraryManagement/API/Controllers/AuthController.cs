using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Application.DTOs.Response;
using LibraryManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ApiResponse<AuthResponse>> Login([FromBody] AuthRequest request)
        {
            return ApiResponse<AuthResponse>.Success(await _authService.LoginAsync(request));
        }

        // POST: api/auth/logout
        [HttpPost("logout")]
        public async Task<ApiResponse<object>> Logout([FromBody] LogoutRequest request)
        {
            await _authService.LogoutAsync(request);
            return ApiResponse<object>.Success(null);
        }

        // POST: api/auth/introspect
        [HttpPost("introspect")]
        public async Task<ApiResponse<IntrospectResponse>> Introspect([FromBody] IntrospectRequest request)
        {
            return ApiResponse<IntrospectResponse>.Success(await _authService.IntrospectAsync(request));
        }

        // POST: api/auth/refresh-token
        [HttpPost("refresh-token")]
        public async Task<ApiResponse<AuthResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            return ApiResponse<AuthResponse>.Success(await _authService.RefreshTokenAsync(request));
        }
    }
}
