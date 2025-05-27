namespace LibraryManagement.Application.DTOs.Request
{
    public class CreateBookRequest
    {
        public string Title { get; set; }
        public string Author { get; set; }

        public int CategoryId { get; set; }
    }
}
