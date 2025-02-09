using Microsoft.AspNetCore.Mvc;

namespace HvP.examify_take_exam.service.Controllers
{
    [ApiController]
    [Route("api/health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHealth()
        {
            return Ok(new { status = "Healthy", time = DateTime.UtcNow });
        }
    }
}
