namespace LibraryManagement.Application.DTOs.Request
{
    public class ReturnBookRequest
    {
        public int BookId { get; set; }
        public string? BorrowerEmail { get; set; }  // Optional for authenticated users
    }
}
