using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Application.DTOs.Response;
using LibraryManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/user
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<IEnumerable<UserDto>>> GetAll()
        {
            return ApiResponse<IEnumerable<UserDto>>
                .Success(await _userService.GetAllUsersAsync());
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<UserDto>> GetById(int id)
        {
            return ApiResponse<UserDto>
                .Success(await _userService.GetUserByIdAsync(id));
        }

        // POST: api/user
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<UserDto>> Create([FromBody] CreateUserRequest request)
        {
            return ApiResponse<UserDto>
                .Success(await _userService.CreateUserAsync(request));
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<UserDto>> Update(int id, [FromBody] UpdateUserRequest request)
        {
            return ApiResponse<UserDto>
                .Success(await _userService.UpdateUserAsync(id, request, true));
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResponse<object>> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            return new ApiResponse<object>();
        }

        // PUT: api/user/self
        [HttpPut("self")]
        public async Task<ApiResponse<UserDto>> UpdateSelf([FromBody] UpdateUserRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return ApiResponse<UserDto>
                .Success(await _userService.UpdateUserAsync(userId, request, false));
        }

        // DELETE: api/user/self
        [HttpDelete("self")]
        public async Task<ApiResponse<object>> DeleteSelf()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _userService.DeleteUserAsync(userId);
            return new ApiResponse<object>();
        }
    }
}
