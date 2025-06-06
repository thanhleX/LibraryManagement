using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Interfaces.Repositories
{
    public interface IBookImageRepository
    {
        Task<BookImage> AddAsync(BookImage image);
        Task<BookImage?> GetByIdAsync(int id);
        Task<IEnumerable<BookImage>> GetByBookIdAsync(int bookId);
        Task UpdateAsync(BookImage image);
        Task DeleteAsync(BookImage image);
    }
} 