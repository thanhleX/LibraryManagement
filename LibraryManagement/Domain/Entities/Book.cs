namespace LibraryManagement.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public bool IsAvailable { get; set; } = true;
        public bool IsActive { get; set; } = true;

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
    }
}
