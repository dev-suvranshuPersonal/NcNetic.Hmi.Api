using SilHmiApi.Models;

namespace SilHmiApi.Interfaces
{
    public interface ICloudSyncService
    {
        Task SyncDailySummaryAsync(IEnumerable<MachineSummaryDto> summaries);
        Task SyncTimeSnapshotsAsync(IEnumerable<MachineTimeSnapshots> snapshots);
        Task<DateTime> GetLastSnapshotTimeFromCloudAsync(string serialNo);
    }
}
