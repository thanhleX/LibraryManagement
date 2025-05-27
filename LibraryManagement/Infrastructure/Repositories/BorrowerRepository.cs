using LibraryManagement.Application.Interface.Repositories;
using LibraryManagement.Domain.Models;
using LibraryManagement.Infrastructure.AppDataContext;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BorrowerRepository : IBorrowerRepository
    {
        private readonly LibraryManagementDbContext _context;

        public BorrowerRepository(LibraryManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Borrower?> GetByEmailAsync(string email)
        {
            return await _context.Borrowers.FirstOrDefaultAsync(b => b.Email == email);
        }

        public async Task AddAsync(Borrower borrower)
        {
            _context.Borrowers.Add(borrower);
            await _context.SaveChangesAsync();
        }
    }
}
