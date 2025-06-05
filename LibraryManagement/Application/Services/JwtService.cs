using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.DTOs.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagement.Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly IInvalidTokenRepository _iInvalidTokenRepository;

        public JwtService(IConfiguration configuration, IInvalidTokenRepository iInvalidTokenRepository)
        {
            _configuration = configuration;
            _iInvalidTokenRepository = iInvalidTokenRepository;
        }

        public TokenInfo GenerateTokens(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var accessTokenExpirationInMinutes = int.Parse(jwtSettings["AccessTokenExpirationInMinutes"]);
            var refreshTokenExpirationInDays = int.Parse(jwtSettings["RefreshTokenExpirationInDays"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("fullname", user.FullName ?? string.Empty),
                new Claim("scope", BuildScope(user)),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            };

            // Generate Access Token
            var accessTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(accessTokenExpirationInMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var accessToken = tokenHandler.CreateToken(accessTokenDescriptor);

            // Generate Refresh Token
            var refreshTokenClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("token_type", "refresh_token")
            };

            var refreshTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(refreshTokenClaims),
                Expires = DateTime.UtcNow.AddDays(refreshTokenExpirationInDays),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };

            var refreshToken = tokenHandler.CreateToken(refreshTokenDescriptor);

            return new TokenInfo
            {
                AccessToken = tokenHandler.WriteToken(accessToken),
                RefreshToken = tokenHandler.WriteToken(refreshToken),
                ExpiresAt = DateTime.UtcNow.AddMinutes(accessTokenExpirationInMinutes)
            };
        }

        private string BuildScope(User user)
        {
            var scopes = new List<string>();
            if (!string.IsNullOrEmpty(user.Role))
            {
                scopes.Add($"ROLE_{user.Role}");
                // Add any additional permissions based on role here
            }
            return string.Join(" ", scopes);
        }

        public async Task<ClaimsPrincipal> ValidateToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            
            try
            {
                // First check if token is blacklisted
                if (await _iInvalidTokenRepository.IsTokenInvalidAsync(token))
                {
                    return null;
                }

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                
                // Additional validation to ensure token hasn't expired
                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    if (jwtToken.ValidTo < DateTime.UtcNow)
                    {
                        return null;
                    }
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
