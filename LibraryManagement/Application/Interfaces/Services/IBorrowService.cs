using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.DTOs.Request;

namespace LibraryManagement.Application.Interfaces.Services
{
    public interface IBorrowService
    {
        Task<BorrowRecordDto> BorrowBookAsync(BorrowBookRequest request);
        Task<IEnumerable<BorrowRecordDto>> GetAllBorrowRecordsAsync();
        Task<BorrowRecordDto> ReturnBookAsync(ReturnBookRequest request);
    }
}
