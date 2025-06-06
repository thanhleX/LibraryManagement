using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Application.DTOs.Response;
using LibraryManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
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
        [AllowAnonymous]
        public async Task<ApiResponse<IEnumerable<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return ApiResponse<IEnumerable<CategoryDto>>.Success(categories);
        }

        // POST: api/category
        [HttpPost]
        [Authorize]
        public async Task<ApiResponse<CategoryDto>> CreatAsync([FromBody] CreateCategoryRequest request)
        {
            return ApiResponse<CategoryDto>
                .Success(await _categoryService.AddCategoryAsync(request));
        }
    }
}
