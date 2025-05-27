using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.DTOs.Request;

namespace LibraryManagement.Application.Interface.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<UserDto> CreateUserAsync(CreateUserRequest request, bool isAdminCreating = false);
        Task<UserDto> UpdateUserAsync(int userId, UpdateUserRequest request, bool isAdmin = false);
        Task DeleteUserAsync(int userId);
    }
}
