using Microsoft.AspNetCore.Mvc;
using NcNetic.Hmi.Api.Services;

namespace NcNetic.Hmi.Api.Controllers
{
    [ApiController]
    [Route("api/hmi/timelogger")]
    public class TimeLoggerController : ControllerBase
    {
        private readonly TimeLoggerService _service;

        public TimeLoggerController(TimeLoggerService service)
        {
            _service = service;
        }

        [HttpGet("daily-summary")]
        public IActionResult GetDailySummary()
        {
            var data = _service.GetDailySummary();
            return Ok(data);
        }
    }
}
