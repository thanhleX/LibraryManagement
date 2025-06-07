using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.AppDataContext;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class InvalidTokenRepository : IInvalidTokenRepository
    {
        private readonly LibraryManagementDbContext _context;

        public InvalidTokenRepository(LibraryManagementDbContext context)
        {
            _context = context;
        }

        public async Task AddInvalidTokenAsync(InvalidToken token)
        {
            await _context.InvalidTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsTokenInvalidAsync(string token)
        {
            return await _context.InvalidTokens.AnyAsync(t => t.Token == token);
        }

        public async Task RemoveExpireTokenAsync()
        {
            var expiredTokens = await _context.InvalidTokens
                .Where(t => t.ExpiratedAt < DateTime.UtcNow)
                .ToListAsync();

            if (expiredTokens.Any())
                _context.InvalidTokens.RemoveRange(expiredTokens);
            
            await _context.SaveChangesAsync();
        }

        public async Task RemoveInvalidTokenAsync(string token)
        {
            var expiredToken = _context.InvalidTokens.Where(t => t.ExpiratedAt < DateTime.UtcNow);
            _context.InvalidTokens.RemoveRange(expiredToken);
            await _context.SaveChangesAsync();
        }
    }
}
