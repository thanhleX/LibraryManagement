using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Interfaces.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync(int? categoryId);
        Task<Book?> GetByIdAsync(int id);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
    }
}
