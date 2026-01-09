using NcNetic.Hmi.Api.Models;

namespace NcNetic.Hmi.Api.Interfaces
{
    public interface ITimeLoggerService
    {
        Task<IReadOnlyList<MachineSummaryDto>> GetDailySummaryAsync();
    }
}
