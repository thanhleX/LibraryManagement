using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Interfaces.Repositories
{
    public interface IBorrowerRepository
    {
        Task<Borrower?> GetByEmailAsync(string email);
        Task AddAsync(Borrower borrower);
    }
}
