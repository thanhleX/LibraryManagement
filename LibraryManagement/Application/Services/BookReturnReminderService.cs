using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Services
{
    public class BookReturnReminderService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BookReturnReminderService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(24); // Kiểm tra mỗi 24 giờ

        public BookReturnReminderService(
            IServiceProvider serviceProvider,
            ILogger<BookReturnReminderService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckAndSendRemindersAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while checking book return reminders");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }

        private async Task CheckAndSendRemindersAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var borrowRecordRepository = scope.ServiceProvider.GetRequiredService<IBorrowRecordRepository>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            // Lấy tất cả bản ghi mượn sách chưa trả
            var activeBorrowRecords = await borrowRecordRepository.GetAllActiveBorrowRecordsAsync();

            foreach (var record in activeBorrowRecords)
            {
                try
                {
                    // Gửi email nhắc nhở nếu sắp đến hạn
                    await emailService.SendReturnReminderEmailAsync(record);

                    // Gửi email thông báo quá hạn nếu đã quá hạn
                    await emailService.SendOverdueEmailAsync(record);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error sending reminder email for borrow record {record.Id}");
                }
            }
        }
    }
} 