using LibraryManagement.Application.Interface.Repositories;
using LibraryManagement.Domain.Models;
using LibraryManagement.Infrastructure.AppDataContext;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BorrowRecordRepository : IBorrowRecordRepository
    {
        private readonly LibraryManagementDbContext _context;

        public BorrowRecordRepository(LibraryManagementDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(BorrowRecord record)
        {
            _context.BorrowRecords.Add(record);
            await _context.SaveChangesAsync();
        }

        public async Task<BorrowRecord?> GetActiveBorrowRecordAsync(int bookId, int borrowerId)
        {
            return await _context.BorrowRecords
                .FirstOrDefaultAsync(r =>
                    r.BookId == bookId &&
                    r.BorrowerId == borrowerId &&
                    r.ReturnedAt == null);
        }

        public async Task<IEnumerable<BorrowRecord>> GetAllWithDetailsAsync()
        {
            return await _context.BorrowRecords
                .Include(r => r.Book)
                .Include(r => r.Borrower)
                .ToListAsync();
        }

        public Task UpdateAsync(BorrowRecord record)
        {
            _context.BorrowRecords.Update(record);
            return _context.SaveChangesAsync();
        }
    }
}
