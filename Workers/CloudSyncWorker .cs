using SilHmiApi.Interfaces;

namespace SilHmiApi.Workers
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

                    var machineInfo = scope.ServiceProvider.GetRequiredService<IMachineInfoService>();

                    var serialNo = await machineInfo.GetMachineSerialNoAsync();

                    var lastSnapshot = await cloudSync.GetLastSnapshotTimeFromCloudAsync(serialNo);

                    var newSnapshots = await timeLogger.GetMachineTimeSnapshotsAsync(lastSnapshot);

                    if(newSnapshots.Any())
                    {
                        await cloudSync.SyncTimeSnapshotsAsync(newSnapshots);

                        _logger.LogInformation("Synced {Count} new snapshots to cloud", newSnapshots.Count);
                    }
                    else
                    {
                        _logger.LogInformation("No new snapshots to sync");
                    }


                    var dailySummary = await timeLogger.GetDailySummaryAsync();
                    await cloudSync.SyncDailySummaryAsync(dailySummary);
                    
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
