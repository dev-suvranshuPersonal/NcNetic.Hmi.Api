using Microsoft.AspNetCore.Mvc;
using SilHmiApi.Interfaces;

namespace NcNetic.Hmi.Api.Controllers
{
    [ApiController]
    [Route("api/hmi/timelogger")]
    public class TimeLoggerController : ControllerBase
    {
        private readonly ITimeLoggerService _timeLoggerService;

        public TimeLoggerController(ITimeLoggerService timeLoggerService)
        {
            _timeLoggerService = timeLoggerService;
        }

        [HttpGet("daily-summary")]
        public async Task<IActionResult> GetDailySummary()
        {
            var data = await _timeLoggerService.GetDailySummaryAsync();
            return Ok(data);
        }
        [HttpGet("machine-time-snapshots")]
        public async Task<IActionResult> GetMachineTimeSnapshots([FromQuery] DateTime fromDate)
        {
            var data = await _timeLoggerService.GetMachineTimeSnapshotsAsync(fromDate);
            return Ok(data);
        }
    }
}
