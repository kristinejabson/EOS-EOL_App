using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TimeBasedPreventiveMeasures.Services;

namespace TimeBasedPreventiveMeasures.BackgroundTask
{
    public class NotificationBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<NotificationBackgroundService> _logger;

        public NotificationBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<NotificationBackgroundService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("NotificationBackgroundService is starting.");

            stoppingToken.Register(() =>
                _logger.LogInformation("NotificationBackgroundService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Generating notifications at: {time}", DateTimeOffset.Now);

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                        await notificationService.GenerateNotificationsAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while generating notifications.");
                }

                // Wait for a period before checking again. Adjust the interval as needed.
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }

            _logger.LogInformation("NotificationBackgroundService has stopped.");
        }
    }
}
