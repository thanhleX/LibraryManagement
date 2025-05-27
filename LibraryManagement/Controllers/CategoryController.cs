using LibraryManagement.DTOs;
using LibraryManagement.DTOs.Request;
using LibraryManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/category
        [HttpGet]
        public async Task<ApiResponse<IEnumerable<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return ApiResponse<IEnumerable<CategoryDto>>.Success(categories);
        }

        // POST: api/category
        [HttpPost]
        public async Task<ApiResponse<CategoryDto>> CreatAsync([FromBody] CreateCategoryRequest request)
        {
            return ApiResponse<CategoryDto>
                .Success(await _categoryService.AddCategoryAsync(request));
        }
    }
}
