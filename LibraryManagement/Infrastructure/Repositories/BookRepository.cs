using System.Linq;
using LibraryManagement.Application.Interface.Repositories;
using LibraryManagement.Domain.Models;
using LibraryManagement.Infrastructure.AppDataContext;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryManagementDbContext _context;

        public BookRepository(LibraryManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync(int? categoryId)
        {
            var query = _context.Books
                .Include(b => b.Category)
                .Where(b => b.IsActive);
            
            if (categoryId.HasValue)
                query = query.Where(b => b.CategoryId == categoryId.Value);
            
            return await query.ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            return book?.IsActive == true ? book : null;
        }

        public async Task AddAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }
    }
}
