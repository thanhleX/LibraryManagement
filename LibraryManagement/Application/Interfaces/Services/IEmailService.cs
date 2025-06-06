using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendReturnReminderEmailAsync(BorrowRecord borrowRecord);
        Task SendOverdueEmailAsync(BorrowRecord borrowRecord);
    }
} 