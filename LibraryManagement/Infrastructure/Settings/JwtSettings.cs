namespace LibraryManagement.Infrastructure.Settings
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpirationTime { get; set; }
        public int RefreshTokenExpirationTime { get; set; }
    }
}
