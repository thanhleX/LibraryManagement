namespace LibraryManagement.Application.DTOs.Request
{
    public class UpdateBookRequest
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public bool IsAvailable { get; set; }
        public string? EbookUrl { get; set; }
        public string? EbookFormat { get; set; }
        public long? EbookSize { get; set; }
        public int CategoryId { get; set; }
    }
}
