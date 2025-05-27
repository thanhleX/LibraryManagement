using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.AppDataContext
{
    public class LibraryManagementDbContext : DbContext
    {
        public LibraryManagementDbContext(DbContextOptions<LibraryManagementDbContext> options)
            : base(options) {}

        public DbSet<Book> Books { get; set; }
        public DbSet<Borrower> Borrowers { get; set; }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình quan hệ giữa các thực thể

            // 1 - N giữa Book và BorrowRecord
            modelBuilder.Entity<BorrowRecord>()
                .HasOne(br => br.User)
                .WithMany(u => u.BorrowRecords)
                .HasForeignKey(br => br.UserId)
                .OnDelete(DeleteBehavior.SetNull); // Cho phép UserId có thể null nếu người mượn không có tài khoản

            // 1 - N giữa Book và BorrowRecord
            modelBuilder.Entity<BorrowRecord>()
                .HasOne(br => br.Book)
                .WithMany(b => b.BorrowRecords)
                .HasForeignKey(br => br.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1 - N giữa Borrower và BorrowRecord
            modelBuilder.Entity<BorrowRecord>()
                .HasOne(br => br.Borrower)
                .WithMany(b => b.BorrowRecords)
                .HasForeignKey(br => br.BorrowerId)
                .OnDelete(DeleteBehavior.Restrict);

            // 1 - N giữa Category và Book
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.Username, u.Email })
                .IsUnique(true);
        }
    }
}
