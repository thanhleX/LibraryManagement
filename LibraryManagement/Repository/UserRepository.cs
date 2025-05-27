using LibraryManagement.AppDataContext;
using LibraryManagement.Models;
using LibraryManagement.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly LibraryManagementDbContext _context;

        public UserRepository(LibraryManagementDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.BorrowRecords)
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.BorrowRecords)
                .FirstOrDefaultAsync(u => u.Id == id);

            return user?.IsActive == true ? user : null;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var user = await _context.Users
                .Include(u => u.BorrowRecords)
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

            return user?.IsActive == true ? user : null;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
