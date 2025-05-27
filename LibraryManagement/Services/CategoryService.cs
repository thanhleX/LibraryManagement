using AutoMapper;
using LibraryManagement.DTOs;
using LibraryManagement.DTOs.Request;
using LibraryManagement.Middleware;
using LibraryManagement.Models;
using LibraryManagement.Repository.Interfaces;
using LibraryManagement.Services.Interfaces;

namespace LibraryManagement.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto> AddCategoryAsync(CreateCategoryRequest request)
        {
            var existCategory = await _categoryRepository.GetByNameAsync(request.Name.Trim().ToLower());
            if (existCategory != null)
                throw new AppException(ErrorCodes.CATEGORY_NAME_ALREADY_EXISTS);

            var category = _mapper.Map<Category>(request);

            await _categoryRepository.AddAsync(category);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var category = await _categoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(category);
        }
    }
}
