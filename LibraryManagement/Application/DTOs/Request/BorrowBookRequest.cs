namespace LibraryManagement.Application.DTOs.Request
{
    public class BorrowBookRequest
    {
        public int BookId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
    }
}
