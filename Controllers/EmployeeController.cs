using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Task_Management_System.Utils;
using static Task_Management_System.Utils.SampleData;

namespace Task_Management_System.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [ApiKeyClassAuthorize]
    [Authorize(Roles = "Admin,Manager,Employee")]
    public class EmployeeController : ControllerBase
    {
        [HttpGet]
        [EnableRateLimiting("taskGetPolicy")]
        public IActionResult GetTasks()
        {
            return Ok(MockData.Tasks);
        }
    }
}