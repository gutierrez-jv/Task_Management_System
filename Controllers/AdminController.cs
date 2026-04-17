using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Task_Management_System.Utils;
using static Task_Management_System.Utils.SampleData;

namespace Task_Management_System.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    [EnableRateLimiting("adminPolicy")]
    public class AdminController : ControllerBase
    {
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            return Ok(MockData.Users);
        }

        [HttpDelete("users/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = MockData.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            MockData.Users.Remove(user);
            return Ok("User deleted successfully.");
        }
    }
}
