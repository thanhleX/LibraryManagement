using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.DTOs.Request;

namespace LibraryManagement.Application.Interfaces.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllAsync();
        Task<IEnumerable<BookDto>> SearchAsync(int? categoryId, string? searchTerm);
        Task<BookDto> GetByIdAsync(int id);
        Task<EbookInfoDto?> GetEbookInfoAsync(int id);
        Task<BookDto> CreateAsync(CreateBookRequest request);
        Task<BookDto> UpdateAsync(int id, UpdateBookRequest request);
        Task DeleteAsync(int id);
    }
}
