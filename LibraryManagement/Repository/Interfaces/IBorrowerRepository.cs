using LibraryManagement.Models;

namespace LibraryManagement.Repository.Interfaces
{
    public interface IBorrowerRepository
    {
        Task<Borrower?> GetByEmailAsync(string email);
        Task AddAsync(Borrower borrower);
    }
}
