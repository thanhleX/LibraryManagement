using AutoMapper;
using LibraryManagement.DTOs;
using LibraryManagement.DTOs.Request;
using LibraryManagement.Middleware;
using LibraryManagement.Models;
using LibraryManagement.Repository.Interfaces;
using LibraryManagement.Services.Interfaces;

namespace LibraryManagement.Services
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

        public async Task<UserDto> CreateUserAsync(CreateUserRequest request, bool isAdminCreating = false)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(request.Username.Trim().ToLower());
            if (existingUser != null)
                throw new AppException(ErrorCodes.USERNAME_ALREADY_EXISTS);

            if (request.Password != request.RePassword)
                throw new AppException(ErrorCodes.PASSWORD_MISMATCH);

            var user = _mapper.Map<User>(request);

            user.Role = isAdminCreating && !string.IsNullOrEmpty(request.Role) ? request.Role : "User";

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
            if ( user == null)
                throw new AppException(ErrorCodes.USER_ID_NOT_FOUND);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateUserAsync(int userId, UpdateUserRequest request, bool isAdmin = false)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new AppException(ErrorCodes.USER_ID_NOT_FOUND);

            user.Password = request.Password ?? user.Password;
            user.FullName = request.FullName ?? user.FullName;
            user.Email = request.Email ?? user.Email;

            user.Role = isAdmin && !string.IsNullOrEmpty(request.Role) ? request.Role : user.Role;
            
            await _userRepository.UpdateAsync(user);
            return _mapper.Map<UserDto>(user);
        }
    }
}
