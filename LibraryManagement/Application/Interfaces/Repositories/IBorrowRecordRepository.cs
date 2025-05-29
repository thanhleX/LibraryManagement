using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Interfaces.Repositories
{
    public interface IBorrowRecordRepository
    {
        Task AddAsync(BorrowRecord record);
        Task<IEnumerable<BorrowRecord>> GetAllWithDetailsAsync(); // include Book & Borrower
        Task<BorrowRecord?> GetActiveBorrowRecordAsync(int bookId, int borrowerId);
        Task UpdateAsync(BorrowRecord record);
    }
}
