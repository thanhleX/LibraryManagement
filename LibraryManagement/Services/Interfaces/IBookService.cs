using LibraryManagement.DTOs;
using LibraryManagement.DTOs.Request;

namespace LibraryManagement.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllAsync(int? categoryId);
        Task<BookDto> GetByIdAsync(int id);
        Task<BookDto> CreateAsync(CreateBookRequest request);
        Task<BookDto> UpdateAsync(int id, UpdateBookRequest request);
        Task DeleteAsync(int id);
    }
}
