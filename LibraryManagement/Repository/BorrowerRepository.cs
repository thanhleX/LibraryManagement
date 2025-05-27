using LibraryManagement.AppDataContext;
using LibraryManagement.Models;
using LibraryManagement.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repository
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
