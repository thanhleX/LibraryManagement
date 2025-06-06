using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.AppDataContext;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BookImageRepository : IBookImageRepository
    {
        private readonly LibraryManagementDbContext _context;

        public BookImageRepository(LibraryManagementDbContext context)
        {
            _context = context;
        }

        public async Task<BookImage> AddAsync(BookImage image)
        {
            _context.BookImages.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<BookImage?> GetByIdAsync(int id)
        {
            return await _context.BookImages.FindAsync(id);
        }

        public async Task<IEnumerable<BookImage>> GetByBookIdAsync(int bookId)
        {
            return await _context.BookImages
                .Where(i => i.BookId == bookId)
                .ToListAsync();
        }

        public async Task UpdateAsync(BookImage image)
        {
            _context.BookImages.Update(image);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(BookImage image)
        {
            _context.BookImages.Remove(image);
            await _context.SaveChangesAsync();
        }
    }
} 