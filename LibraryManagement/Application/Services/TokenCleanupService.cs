
using LibraryManagement.Application.Interfaces.Repositories;
using Microsoft.OpenApi.Writers;
using static System.Formats.Asn1.AsnWriter;

namespace LibraryManagement.Application.Services
{
    public class TokenCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly TimeSpan _interval = TimeSpan.FromHours(1); // quét token mỗi giờ

        public TokenCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {

                    var invalidTokenRepo = scope.ServiceProvider.GetRequiredService<IInvalidTokenRepository>(); ;

                    try
                    {
                        await invalidTokenRepo.RemoveExpireTokenAsync();
                    }
                    catch (Exception ex)
                    {
                        // Log the exception (consider using a logging framework)
                        Console.WriteLine($"Error during token cleanup: {ex.Message}");
                    }
                }
                
                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
