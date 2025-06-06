using LibraryManagement.API.Middleware;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Application.DTOs.Response;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookImageController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IBookImageRepository _bookImageRepository;

        public BookImageController(
            IBookService bookService,
            ICloudinaryService cloudinaryService,
            IBookImageRepository bookImageRepository)
        {
            _bookService = bookService;
            _cloudinaryService = cloudinaryService;
            _bookImageRepository = bookImageRepository;
        }

        // GET: api/bookimage/{bookId}
        [HttpGet("{bookId}")]
        [AllowAnonymous]
        public async Task<ApiResponse<IEnumerable<BookImageDto>>> GetBookImages(int bookId)
        {
            var images = await _bookImageRepository.GetByBookIdAsync(bookId);
            return ApiResponse<IEnumerable<BookImageDto>>.Success(
                images.Select(i => new BookImageDto
                {
                    Id = i.Id,
                    Url = i.Url,
                    CreatedAt = i.CreatedAt
                })
            );
        }

        // POST: api/bookimage/{bookId}
        [HttpPost("{bookId}")]
        [Authorize]
        public async Task<ApiResponse<BookImageDto>> UploadImage(int bookId, [FromForm] UploadBookImageRequest request)
        {
            // Kiểm tra sách tồn tại
            var book = await _bookService.GetByIdAsync(bookId);
            if (book == null)
                throw new AppException(ErrorCodes.BOOK_ID_NOT_FOUND);

            // Upload ảnh lên Cloudinary
            var (publicId, url) = await _cloudinaryService.UploadImageAsync(request.File, $"books/{bookId}");

            // Lưu thông tin ảnh vào database
            var image = new BookImage
            {
                BookId = bookId,
                PublicId = publicId,
                Url = url,
            };

            await _bookImageRepository.AddAsync(image);

            return ApiResponse<BookImageDto>.Success(new BookImageDto
            {
                Id = image.Id,
                Url = image.Url,
                CreatedAt = image.CreatedAt
            });
        }

        // DELETE: api/bookimage/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ApiResponse<object>> DeleteImage(int id)
        {
            var image = await _bookImageRepository.GetByIdAsync(id);
            if (image == null)
                throw new AppException(ErrorCodes.BOOK_IMAGE_NOT_FOUND);

            // Xóa ảnh khỏi Cloudinary
            await _cloudinaryService.DeleteImageAsync(image.PublicId);

            // Xóa thông tin ảnh khỏi database
            await _bookImageRepository.DeleteAsync(image);

            return new ApiResponse<object>();
        }
    }
} 