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
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _borrowService;

        public BorrowController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }

        // POST: api/borrow
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponse<BorrowRecordDto>> BorrowBook([FromBody] BorrowBookRequest request)
        {
            return ApiResponse<BorrowRecordDto>
                .Success(await _borrowService.BorrowBookAsync(request));
        }

        // GET: api/borrow
        [HttpGet]
        [Authorize]
        public async Task<ApiResponse<IEnumerable<BorrowRecordDto>>> GetAllBorrowRecords()
        {
            return ApiResponse<IEnumerable<BorrowRecordDto>>
                .Success(await _borrowService.GetAllBorrowRecordsAsync());
        }

        // POST: api/borrow/return
        [HttpPost("return")]
        [AllowAnonymous]
        public async Task<ApiResponse<BorrowRecordDto>> ReturnBook([FromBody] ReturnBookRequest request)
        {
            return ApiResponse<BorrowRecordDto>
                .Success(await _borrowService.ReturnBookAsync(request));
        }
    }
}
