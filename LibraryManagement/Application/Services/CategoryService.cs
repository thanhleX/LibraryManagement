using AutoMapper;
using LibraryManagement.API.Middleware;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Services
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
