using LibraryManagement.DTOs;
using LibraryManagement.DTOs.Request;

namespace LibraryManagement.Services.Interfaces
{
    public interface IBorrowService
    {
        Task<BorrowRecordDto> BorrowBookAsync(BorrowBookRequest request);
        Task<IEnumerable<BorrowRecordDto>> GetAllBorrowRecordsAsync();
        Task<BorrowRecordDto> ReturnBookAsync(ReturnBookRequest request);
    }
}
