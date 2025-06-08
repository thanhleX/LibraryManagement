using System.IdentityModel.Tokens.Jwt;
using LibraryManagement.API.Middleware;
using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Application.DTOs.Response;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Domain.Entities;
using System.Security.Claims;

namespace LibraryManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IInvalidTokenRepository _iInvalidTokenRepository;
        private readonly IJwtService _jwtTokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IUserRepository userRepository, IInvalidTokenRepository invalidTokenRepository,
            IJwtService jwtTokenService, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _iInvalidTokenRepository = invalidTokenRepository;
            _jwtTokenService = jwtTokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthResponse> RegisterAsync(CreateUserRequest request)
        {
            // Check if username already exists
            var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUser != null)
                throw new AppException(ErrorCodes.USERNAME_ALREADY_EXISTS);

            // Validate password match
            if (request.Password != request.RePassword)
                throw new AppException(ErrorCodes.PASSWORD_MISMATCH);

            // Create new user
            var user = new User
            {
                Username = request.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FullName = request.FullName,
                Email = request.Email,
                Role = "User" //Role default is User
            };

            await _userRepository.CreateAsync(user);

            // Generate tokens for the new user
            var tokenInfo = _jwtTokenService.GenerateTokens(user);
            return new AuthResponse
            {
                AccessToken = tokenInfo.AccessToken,
                RefreshToken = tokenInfo.RefreshToken,
                IsAuthenticated = true,
                ExpiresAt = tokenInfo.ExpiresAt
            };
        }

        public async Task<AuthResponse> LoginAsync(AuthRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            bool isAuthenticated = BCrypt.Net.BCrypt.Verify(request.Password, user?.Password ?? string.Empty);
            if (user == null || !isAuthenticated)
                throw new AppException(ErrorCodes.INVALID_CREDENTIALS);

            var tokenInfo = _jwtTokenService.GenerateTokens(user);
            return new AuthResponse
            {
                AccessToken = tokenInfo.AccessToken,
                RefreshToken = tokenInfo.RefreshToken,
                IsAuthenticated = true,
                ExpiresAt = tokenInfo.ExpiresAt
            };
        }

        public async Task LogoutAsync(LogoutRequest request)
        {
            // Validate and blacklist the access token
            var accessTokenPrincipal = await _jwtTokenService.ValidateToken(request.AccessToken);
            if (accessTokenPrincipal != null)
            {
                var accessTokenExpiry = accessTokenPrincipal.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;
                if (!string.IsNullOrEmpty(accessTokenExpiry))
                {
                    await _iInvalidTokenRepository.AddInvalidTokenAsync(new InvalidToken
                    {
                        Token = request.AccessToken,
                        ExpiratedAt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(accessTokenExpiry)).UtcDateTime
                    });
                }
            }

            // Validate and blacklist the refresh token
            var refreshTokenPrincipal = await _jwtTokenService.ValidateToken(request.RefreshToken);
            if (refreshTokenPrincipal != null)
            {
                var refreshTokenExpiry = refreshTokenPrincipal.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;
                if (!string.IsNullOrEmpty(refreshTokenExpiry))
                {
                    await _iInvalidTokenRepository.AddInvalidTokenAsync(new InvalidToken
                    {
                        Token = request.RefreshToken,
                        ExpiratedAt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(refreshTokenExpiry)).UtcDateTime
                    });
                }
            }
        }

        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var principal = await _jwtTokenService.ValidateToken(request.RefreshToken);
            if (principal == null)
                throw new AppException(ErrorCodes.INVALID_TOKEN);

            // Verify this is a refresh token
            var tokenType = principal.FindFirst("token_type")?.Value;
            if (tokenType != "refresh_token")
                throw new AppException(ErrorCodes.INVALID_TOKEN);

            // Check if token is in invalid tokens list
            if (await _iInvalidTokenRepository.IsTokenInvalidAsync(request.RefreshToken))
                throw new AppException(ErrorCodes.INVALID_TOKEN);

            // Get user ID from token
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new AppException(ErrorCodes.INVALID_TOKEN);

            // Get user from database
            var user = await _userRepository.GetByIdAsync(int.Parse(userId));
            if (user == null)
                throw new AppException(ErrorCodes.USER_NOT_FOUND);

            // Invalidate the old refresh token
            var expiryTime = principal.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;
            if (!string.IsNullOrEmpty(expiryTime))
            {
                await _iInvalidTokenRepository.AddInvalidTokenAsync(new InvalidToken
                {
                    Token = request.RefreshToken,
                    ExpiratedAt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiryTime)).UtcDateTime
                });
            }

            // Generate new tokens
            var tokenInfo = _jwtTokenService.GenerateTokens(user);
            return new AuthResponse
            {
                AccessToken = tokenInfo.AccessToken,
                RefreshToken = tokenInfo.RefreshToken,
                IsAuthenticated = true,
                ExpiresAt = tokenInfo.ExpiresAt
            };
        }

        public async Task<IntrospectResponse> IntrospectAsync(IntrospectRequest request)
        {
            var principal = await _jwtTokenService.ValidateToken(request.Token);
            if (principal == null)
                return new IntrospectResponse { IsValid = false };

            var tokenId = principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            if (string.IsNullOrEmpty(tokenId))
                return new IntrospectResponse { IsValid = false };

            var isInvalid = await _iInvalidTokenRepository.IsTokenInvalidAsync(request.Token);
            return new IntrospectResponse { IsValid = !isInvalid };
        }
    }
}
