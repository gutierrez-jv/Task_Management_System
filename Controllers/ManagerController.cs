using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Task_Management_System.Utils;
using Task_Management_System.Models;
using static Task_Management_System.Utils.SampleData;

namespace Task_Management_System.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [ApiKeyClassAuthorize]
    [Authorize(Roles = "Admin,Manager")]
    public class ManagerController : ControllerBase
    {
        [HttpPost]
        [EnableRateLimiting("taskWritePolicy")]
        public IActionResult CreateTask([FromBody] CreateTaskRequest request)
        {
            var newTask = new TaskItem
            {
                Id = MockData.Tasks.Any() ? MockData.Tasks.Max(t => t.Id) + 1 : 1,
                Title = request.Title,
                Description = request.Description,
                Status = "Pending"
            };

            MockData.Tasks.Add(newTask);

            return Ok(newTask);
        }

        [HttpPut("{id}")]
        [EnableRateLimiting("taskWritePolicy")]
        public IActionResult UpdateTask(int id, [FromBody] UpdateTaskRequest request)
        {
            var task = MockData.Tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return NotFound("Task not found.");
            }

            task.Title = request.Title;
            task.Description = request.Description;
            task.Status = request.Status;

            return Ok(task);
        }
    }
}