using Microsoft.AspNetCore.Mvc;

namespace Song_Selector_app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly IHealthChecker _healthChecker;
        private readonly ILogger<HealthController> _logger; 

        public HealthController(IHealthChecker healthChecker, ILogger<HealthController> logger) 
        {
            _healthChecker = healthChecker;
            _logger = logger;
        }

        [HttpGet("/health")]
        
        public IActionResult CheckHealth()
        {
            var healthStatus = _healthChecker.CheckHealth();

            // Log health status
            _logger.LogInformation($"Health Check: {healthStatus.Message}");

            if (healthStatus.IsHealthy)
            {
                return Ok(new { Status = "Healthy" });
            }
            else
            {
                return StatusCode(500, new { Status = "Unhealthy", Message = healthStatus.Message });
            }
        }
    }

    public interface IHealthChecker
    {
        HealthStatus CheckHealth();
    }

    public class HealthStatus
    {
        public HealthStatus(string message)
        {
            Message = message;
        }

        public HealthStatus()
        {
            throw new NotImplementedException();
        }

        public bool IsHealthy { get; set; }
        public string Message { get; set; }
    }

    public class BasicHealthChecker : IHealthChecker
    {
        public HealthStatus CheckHealth()
        {
            
            bool isHealthy = true; 
            string message = isHealthy ? "Healthy" : "Unhealthy";

            return new HealthStatus { IsHealthy = isHealthy, Message = message };
        }
    }
}