using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.DTOs.Request;

namespace LibraryManagement.Application.Interface.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> AddCategoryAsync(CreateCategoryRequest request);
    }
}
