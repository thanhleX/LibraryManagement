namespace LibraryManagement.Application.DTOs.Request
{
    public class CreateUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public string Role { get; set; }
    }
}
