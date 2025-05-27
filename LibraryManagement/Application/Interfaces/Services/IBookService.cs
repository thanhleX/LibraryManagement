using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.DTOs.Request;

namespace LibraryManagement.Application.Interface.Services
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
