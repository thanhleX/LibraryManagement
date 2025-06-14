﻿namespace LibraryManagement.Application.DTOs.Response
{
    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public bool IsAuthenticated { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
