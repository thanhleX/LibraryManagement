using LibraryManagement.Domain.Models;

namespace LibraryManagement.Application.Interface.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync(int? categoryId);
        Task<Book?> GetByIdAsync(int id);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
    }
}
