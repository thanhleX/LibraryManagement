using LibraryManagement.DTOs;
using LibraryManagement.DTOs.Request;
using LibraryManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
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
        public async Task<ApiResponse<BorrowRecordDto>> BorrowBook([FromBody] BorrowBookRequest request)
        {
            return ApiResponse<BorrowRecordDto>
                .Success(await _borrowService.BorrowBookAsync(request));
        }

        // GET: api/borrow
        [HttpGet]
        public async Task<ApiResponse<IEnumerable<BorrowRecordDto>>> GetAllBorrowRecords()
        {
            return ApiResponse<IEnumerable<BorrowRecordDto>>
                .Success(await _borrowService.GetAllBorrowRecordsAsync());
        }

        // POST: api/borrow/return
        [HttpPost("return")]
        public async Task<ApiResponse<BorrowRecordDto>> ReturnBook([FromBody] ReturnBookRequest request)
        {
            return ApiResponse<BorrowRecordDto>
                .Success(await _borrowService.ReturnBookAsync(request));
        }
    }
}
