using System.IdentityModel.Tokens.Jwt;
using LibraryManagement.API.Middleware;
using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Application.DTOs.Response;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IInvalidTokenRepository _iInvalidTokenRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IUserRepository userRepository, IInvalidTokenRepository invalidTokenRepository,
            IJwtTokenService jwtTokenService, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _iInvalidTokenRepository = invalidTokenRepository;
            _jwtTokenService = jwtTokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthResponse> LoginAsync(AuthRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            bool isAuthenticated = BCrypt.Net.BCrypt.Verify(request.Password, user?.Password ?? string.Empty);
            if (user == null || !isAuthenticated)
                throw new AppException(ErrorCodes.INVALID_CREDENTIALS);

            var token = _jwtTokenService.GenerateToken(user);
            return new AuthResponse
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                ExpirationTime = token.AccessTokenExpiration
            };
        }

        public async Task LogoutAsync(LogoutRequest request)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
                            .FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
                throw new AppException(ErrorCodes.INVALID_TOKEN);

            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken;

            try
            {
                jwtToken = handler.ReadJwtToken(token);
            }
            catch
            {
                throw new AppException(ErrorCodes.INVALID_TOKEN);
            }

            var expiredAt = jwtToken.ValidTo;

            await _iInvalidTokenRepository.AddInvalidTokenAsync(new InvalidToken
            {
                Token = token,
                ExpiratedAt = expiredAt,
            });
        }

        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            if (await _iInvalidTokenRepository.IsTokenInvalidAsync(request.RefreshToken))
                throw new AppException(ErrorCodes.INVALID_TOKEN);

            var principal = _jwtTokenService.GetPrincipalFromExpiredToken(request.RefreshToken);
            if (principal == null)
                throw new AppException(ErrorCodes.INVALID_TOKEN);

            var username = principal.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                throw new AppException(ErrorCodes.INVALID_CREDENTIALS);

            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
                throw new AppException(ErrorCodes.USER_NOT_FOUND);

            var token = _jwtTokenService.GenerateToken(user);

            return new AuthResponse
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                ExpirationTime = token.AccessTokenExpiration
            };
        }
    }
}
