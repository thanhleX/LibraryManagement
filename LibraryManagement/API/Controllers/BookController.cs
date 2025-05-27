using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Application.Interface.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/book
        [HttpGet]
        public async Task<ApiResponse<IEnumerable<BookDto>>> GetAll([FromQuery] int? categoryId)
        {
            return ApiResponse<IEnumerable<BookDto>>
                .Success(await _bookService.GetAllAsync(categoryId));
        }

        // GET: api/book/{id}
        [HttpGet("{id}")]
        public async Task<ApiResponse<BookDto>> GetById(int id)
        {
            return ApiResponse<BookDto>
                .Success(await _bookService.GetByIdAsync(id));
        }

        // POST: api/book
        [HttpPost]
        public async Task<ApiResponse<BookDto>> Create([FromBody] CreateBookRequest request)
        {
            return ApiResponse<BookDto>
                .Success(await _bookService.CreateAsync(request));
        }

        // PUT: api/book/{id}
        [HttpPut("{id}")]
        public async Task<ApiResponse<BookDto>> Update(int id, [FromBody] UpdateBookRequest request)
        {
            return ApiResponse<BookDto>
                .Success(await _bookService.UpdateAsync(id, request));
        }

        // DELETE: api/book/{id}
        [HttpDelete("{id}")]
        public async Task<ApiResponse<object>> Delete(int id)
        {
            await _bookService.DeleteAsync(id);
            return new ApiResponse<object>();
        }
    }
}
