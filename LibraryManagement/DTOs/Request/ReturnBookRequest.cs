namespace LibraryManagement.DTOs.Request
{
    public class ReturnBookRequest
    {
        public int BookId { get; set; }
        public string BorrowerEmail { get; set; }
    }
}
