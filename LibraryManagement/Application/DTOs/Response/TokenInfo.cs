namespace LibraryManagement.Application.DTOs.Response
{
    public class TokenInfo
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
} 