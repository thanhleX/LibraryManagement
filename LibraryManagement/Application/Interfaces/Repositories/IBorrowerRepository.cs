using LibraryManagement.Domain.Models;

namespace LibraryManagement.Application.Interface.Repositories
{
    public interface IBorrowerRepository
    {
        Task<Borrower?> GetByEmailAsync(string email);
        Task AddAsync(Borrower borrower);
    }
}
