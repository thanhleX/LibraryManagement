﻿namespace LibraryManagement.Infrastructure.Settings
{
    public class JwtTokenResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
