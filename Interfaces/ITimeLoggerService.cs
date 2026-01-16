using SilHmiApi.Models;

namespace SilHmiApi.Interfaces
{
    public interface ITimeLoggerService
    {
        Task<IReadOnlyList<MachineSummaryDto>> GetDailySummaryAsync();
        Task<IReadOnlyList<MachineTimeSnapshots>> GetMachineTimeSnapshotsAsync(DateTime lastSnapshotTime);
    }
}
