using NcNetic.Hmi.Api.Models;

namespace NcNetic.Hmi.Api.Interfaces
{
    public interface ICloudSyncService
    {
        Task SyncDailySummaryAsync(IEnumerable<MachineSummaryDto> summaries);
    }
}
