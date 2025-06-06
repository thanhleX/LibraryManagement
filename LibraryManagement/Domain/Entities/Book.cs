namespace LibraryManagement.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public bool IsAvailable { get; set; } = true;
        public bool IsActive { get; set; } = true;
        public string? EbookUrl { get; set; }  // URL để tải e-book
        public string? EbookFormat { get; set; }  // Định dạng e-book (PDF, EPUB, etc.)
        public long? EbookSize { get; set; }  // Kích thước file e-book (bytes)

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
        public ICollection<BookImage> Images { get; set; } = new List<BookImage>();
    }
}
