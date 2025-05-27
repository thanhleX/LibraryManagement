namespace LibraryManagement.Application.DTOs.Request
{
    public class UpdateUserRequest
    {
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public string? Role { get; set; } // chỉ admin chỉnh role
    }
}
