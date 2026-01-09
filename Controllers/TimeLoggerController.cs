using Microsoft.AspNetCore.Mvc;
using NcNetic.Hmi.Api.Interfaces;

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
    }
}
