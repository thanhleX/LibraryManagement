namespace LibraryManagement.Application.DTOs
{
    public class BorrowRecordDto
    {
        public int Id { get; set; }
        public string BookTitle { get; set; }
        public string BorrowerName { get; set; }
        public DateTime BorrowedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
    }
}
