using AutoMapper;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.API.Middleware;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Application.Interface.Services;
using LibraryManagement.Application.Interface.Repositories;
using LibraryManagement.Domain.Models;

namespace LibraryManagement.Application.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBorrowerRepository _borrowerRepository;
        private readonly IBorrowRecordRepository _borrowRecordRepository;
        private readonly IMapper _mapper;

        public BorrowService(
            IBookRepository bookRepository,
            IBorrowerRepository borrowerRepository,
            IBorrowRecordRepository borrowRecordRepository,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _borrowerRepository = borrowerRepository;
            _borrowRecordRepository = borrowRecordRepository;
            _mapper = mapper;
        }

        public async Task<BorrowRecordDto> BorrowBookAsync(BorrowBookRequest request)
        {
            var book = await _bookRepository.GetByIdAsync(request.BookId);
            if (book == null)
                throw new AppException(ErrorCodes.BOOK_ID_NOT_FOUND);

            if (!book.IsAvailable)
                throw new AppException(ErrorCodes.BOOK_NOT_AVAILABLE);

            // Kiểm tra xem người mượn đã tồn tại chưa
            var borrower = await _borrowerRepository.GetByEmailAsync(request.Email);
            if (borrower == null)
            {
                borrower = _mapper.Map<Borrower>(request);
                await _borrowerRepository.AddAsync(borrower);
            }

            // Tạo record mượn sách
            var borrowRecord = new BorrowRecord
            {
                BookId = book.Id,
                BorrowerId = borrower.Id,
                BorrowedAt = DateTime.UtcNow
            };

            book.IsAvailable = false;

            await _borrowRecordRepository.AddAsync(borrowRecord);
            await _bookRepository.UpdateAsync(book);

            return _mapper.Map<BorrowRecordDto>(borrowRecord);
        }

        public async Task<IEnumerable<BorrowRecordDto>> GetAllBorrowRecordsAsync()
        {
            var records = await _borrowRecordRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<BorrowRecordDto>>(records);
        }

        public async Task<BorrowRecordDto> ReturnBookAsync(ReturnBookRequest request)
        {
            var borrower = await _borrowerRepository.GetByEmailAsync(request.BorrowerEmail)
                ?? throw new AppException(ErrorCodes.BORROWER_NOT_FOUND);

            var book = await _bookRepository.GetByIdAsync(request.BookId)
                ?? throw new AppException(ErrorCodes.BOOK_ID_NOT_FOUND);

            // Tìm bản ghi mượn chưa trả
            var borrowRecord = await _borrowRecordRepository
                .GetActiveBorrowRecordAsync(book.Id, borrower.Id);

            if (borrowRecord == null)
                throw new AppException(ErrorCodes.BORROW_RECORD_NOT_FOUND);

            borrowRecord.ReturnedAt = DateTime.Now;
            await _borrowRecordRepository.UpdateAsync(borrowRecord);

            // Optional: Cho sách trở về trạng thái sẵn sàng
            book.IsAvailable = true;
            await _bookRepository.UpdateAsync(book);

            return _mapper.Map<BorrowRecordDto>(borrowRecord);
        }
    }
}
