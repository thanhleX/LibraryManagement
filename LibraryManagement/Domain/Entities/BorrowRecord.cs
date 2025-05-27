namespace LibraryManagement.Domain.Models
{
    public class BorrowRecord
    {
        public int Id { get; set; }
        public bool IsReturned => ReturnedAt.HasValue;
        public DateTime BorrowedAt { get; set; } = DateTime.Now;
        public DateTime? ReturnedAt { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public int? UserId { get; set; } // nếu người mượn có tài khoản
        public User User { get; set; }

        public int? BorrowerId { get; set; } // nếu người mượn không có tài khoản
        public Borrower Borrower { get; set; }
    }
}