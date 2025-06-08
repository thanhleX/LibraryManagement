using AutoMapper;
using LibraryManagement.API.Middleware;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserRequest request)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(request.Username.Trim().ToLower());
            if (existingUser != null)
                throw new AppException(ErrorCodes.USERNAME_ALREADY_EXISTS);

            if (request.Password != request.RePassword)
                throw new AppException(ErrorCodes.PASSWORD_MISMATCH);

            // Mã hóa mật khẩu trước khi lưu vào cơ sở dữ liệu
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password.Trim());

            var user = _mapper.Map<User>(request);
            user.Password = passwordHash;
            user.Role = !string.IsNullOrEmpty(request.Role) ? request.Role : "User";

            await _userRepository.CreateAsync(user);
            return _mapper.Map<UserDto>(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new AppException(ErrorCodes.USER_ID_NOT_FOUND);

            user.IsActive = false;
            await _userRepository.UpdateAsync(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new AppException(ErrorCodes.USER_ID_NOT_FOUND);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateUserAsync(int userId, UpdateUserRequest request, bool isAdmin = false)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new AppException(ErrorCodes.USER_ID_NOT_FOUND);

            if (!string.IsNullOrEmpty(request.Password))
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password.Trim());

            user.FullName = request.FullName ?? user.FullName;
            user.Email = request.Email ?? user.Email;

            user.Role = isAdmin && !string.IsNullOrEmpty(request.Role) ? request.Role : user.Role;
            
            await _userRepository.UpdateAsync(user);
            return _mapper.Map<UserDto>(user);
        }
    }
}
