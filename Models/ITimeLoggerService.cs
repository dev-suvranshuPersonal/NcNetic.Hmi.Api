using NcNetic.Hmi.Api.Models;

namespace NcNetic.Hmi.Api.Services
{
    public interface ITimeLoggerService
    {
        Task<IReadOnlyList<MachineSummaryDto>> GetDailySummaryAsync();
    }
}
