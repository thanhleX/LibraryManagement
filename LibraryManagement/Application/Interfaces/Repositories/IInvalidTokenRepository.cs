using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Interfaces.Repositories
{
    public interface IInvalidTokenRepository
    {
        Task AddInvalidTokenAsync(InvalidToken token);
        Task<bool> IsTokenInvalidAsync(string token);
        Task RemoveInvalidTokenAsync(string token);
    }
}
