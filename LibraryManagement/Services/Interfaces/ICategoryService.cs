using LibraryManagement.DTOs;
using LibraryManagement.DTOs.Request;

namespace LibraryManagement.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> AddCategoryAsync(CreateCategoryRequest request);
    }
}
