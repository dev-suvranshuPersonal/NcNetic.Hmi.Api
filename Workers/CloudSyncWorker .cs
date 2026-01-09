
using NcNetic.Hmi.Api.Interfaces;

namespace NcNetic.Hmi.Api.Workers
{
    public class CloudSyncWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CloudSyncWorker> _logger;

        public CloudSyncWorker(
            IServiceScopeFactory scopeFactory,
            ILogger<CloudSyncWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CloudSyncWorker started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var timeLogger = scope.ServiceProvider
                        .GetRequiredService<ITimeLoggerService>();

                    var cloudSync = scope.ServiceProvider
                        .GetRequiredService<ICloudSyncService>();

                    var data = await timeLogger.GetDailySummaryAsync();
                    await cloudSync.SyncDailySummaryAsync(data);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Cloud sync failed");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
