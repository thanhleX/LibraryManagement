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
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/book
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiResponse<IEnumerable<BookDto>>> GetAll()
        {
            return ApiResponse<IEnumerable<BookDto>>
                .Success(await _bookService.GetAllAsync());
        }

        // GET: api/book/search
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ApiResponse<IEnumerable<BookDto>>> Search([FromQuery] int? categoryId, [FromQuery] string? searchTerm)
        {
            return ApiResponse<IEnumerable<BookDto>>
                .Success(await _bookService.SearchAsync(categoryId, searchTerm));
        }

        // GET: api/book/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ApiResponse<BookDto>> GetById(int id)
        {
            return ApiResponse<BookDto>
                .Success(await _bookService.GetByIdAsync(id));
        }

        // GET: api/book/{id}/ebook
        [HttpGet("{id}/ebook")]
        [Authorize]
        public async Task<IActionResult> DownloadEbook(int id)
        {
            var ebookInfo = await _bookService.GetEbookInfoAsync(id);
            if (ebookInfo == null)
                return NotFound(new ApiResponse<object> { Message = "E-book not found" });

            return Ok(new ApiResponse<EbookInfoDto> { Data = ebookInfo });
        }

        // POST: api/book
        [HttpPost]
        [Authorize]
        public async Task<ApiResponse<BookDto>> Create([FromBody] CreateBookRequest request)
        {
            return ApiResponse<BookDto>
                .Success(await _bookService.CreateAsync(request));
        }

        // PUT: api/book/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ApiResponse<BookDto>> Update(int id, [FromBody] UpdateBookRequest request)
        {
            return ApiResponse<BookDto>
                .Success(await _bookService.UpdateAsync(id, request));
        }

        // DELETE: api/book/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ApiResponse<object>> Delete(int id)
        {
            await _bookService.DeleteAsync(id);
            return new ApiResponse<object>();
        }
    }
}
