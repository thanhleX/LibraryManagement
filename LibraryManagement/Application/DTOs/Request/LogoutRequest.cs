namespace LibraryManagement.Application.DTOs.Request
{
    public class LogoutRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
