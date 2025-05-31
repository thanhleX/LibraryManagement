using AutoMapper;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.API.Middleware;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Domain.Entities;
using System.Security.Claims;

namespace LibraryManagement.Application.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBorrowerRepository _borrowerRepository;
        private readonly IBorrowRecordRepository _borrowRecordRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BorrowService(
            IBookRepository bookRepository,
            IBorrowerRepository borrowerRepository,
            IBorrowRecordRepository borrowRecordRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _bookRepository = bookRepository;
            _borrowerRepository = borrowerRepository;
            _borrowRecordRepository = borrowRecordRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BorrowRecordDto> BorrowBookAsync(BorrowBookRequest request)
        {
            var book = await _bookRepository.GetByIdAsync(request.BookId);
            if (book == null)
                throw new AppException(ErrorCodes.BOOK_ID_NOT_FOUND);

            if (!book.IsAvailable)
                throw new AppException(ErrorCodes.BOOK_NOT_AVAILABLE);

            // Get user from JWT token if authenticated
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;            
            User? user = null;
            if (!string.IsNullOrEmpty(userId))
            {
                user = await _userRepository.GetByIdAsync(int.Parse(userId));
                if (user == null)
                    throw new AppException(ErrorCodes.USER_NOT_FOUND);
            }

            // Xử lý người dùng chưa xác thực
            if (user == null)
            {
                if (string.IsNullOrEmpty(request.Email))
                    throw new AppException(ErrorCodes.INVALID_CREDENTIALS);

                var borrower = await _borrowerRepository.GetByEmailAsync(request.Email);
                if (borrower == null)
                {
                    if (borrower == null)                    
                        borrower = _mapper.Map<Borrower>(request);
                    
                    await _borrowerRepository.AddAsync(borrower);
                }

                // Tạo bản ghi mượn sách cho người dùng chưa xác thực
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

            // Xử lý người dùng đã xác thực
            var userBorrowRecord = new BorrowRecord
            {
                BookId = book.Id,
                UserId = user.Id,
                BorrowedAt = DateTime.UtcNow
            };

            book.IsAvailable = false;
            await _borrowRecordRepository.AddAsync(userBorrowRecord);
            await _bookRepository.UpdateAsync(book);

            return _mapper.Map<BorrowRecordDto>(userBorrowRecord);
        }

        public async Task<IEnumerable<BorrowRecordDto>> GetAllBorrowRecordsAsync()
        {
            var records = await _borrowRecordRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<BorrowRecordDto>>(records);
        }

        public async Task<BorrowRecordDto> ReturnBookAsync(ReturnBookRequest request)
        {
            // Lấy người dùng từ JWT token nếu đã xác thực
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            User? user = null;
            if (!string.IsNullOrEmpty(userId))
            {
                user = await _userRepository.GetByIdAsync(int.Parse(userId));
                if (user == null)
                    throw new AppException(ErrorCodes.USER_NOT_FOUND);
            }

            var book = await _bookRepository.GetByIdAsync(request.BookId)
                ?? throw new AppException(ErrorCodes.BOOK_ID_NOT_FOUND);

            // Tìm bản ghi mượn sách đang hoạt động
            BorrowRecord? borrowRecord;
            if (user != null)
            {
                // Nếu đã đăng nhập, tìm theo UserId
                borrowRecord = await _borrowRecordRepository.GetActiveBorrowRecordByUserIdAsync(book.Id, user.Id);
            }
            else
            {
                // Nếu chưa đăng nhập, yêu cầu email
                if (string.IsNullOrEmpty(request.BorrowerEmail))
                    throw new AppException(ErrorCodes.INVALID_CREDENTIALS);

                var borrower = await _borrowerRepository.GetByEmailAsync(request.BorrowerEmail)
                    ?? throw new AppException(ErrorCodes.BORROWER_NOT_FOUND);

                borrowRecord = await _borrowRecordRepository.GetActiveBorrowRecordAsync(book.Id, borrower.Id);
            }

            if (borrowRecord == null)
                throw new AppException(ErrorCodes.BORROW_RECORD_NOT_FOUND);

            borrowRecord.ReturnedAt = DateTime.UtcNow;
            await _borrowRecordRepository.UpdateAsync(borrowRecord);

            book.IsAvailable = true;
            await _bookRepository.UpdateAsync(book);

            return _mapper.Map<BorrowRecordDto>(borrowRecord);
        }
    }
}
