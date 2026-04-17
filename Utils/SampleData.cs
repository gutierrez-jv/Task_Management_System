using Task_Management_System.Models;

namespace Task_Management_System.Utils
{
    public class SampleData
    {
        // this class simply acts as a placeholder for the requirements
        // this is simply like a mock database to test the API endpoints without needing a real database connection
        public static class MockData
        {
            public static List<User> Users = new()
        {
            new User { Id = 1, Username = "admin", Password = "admin123", Role = "Admin" },
            new User { Id = 2, Username = "manager", Password = "manager123", Role = "Manager" },
            new User { Id = 3, Username = "employee", Password = "employee123", Role = "Employee" }
        };

            public static List<TaskItem> Tasks = new()
        {
            new TaskItem { Id = 1, Title = "Prepare Report", Description = "Prepare weekly report", Status = "Pending" },
            new TaskItem { Id = 2, Title = "Check Emails", Description = "Review company emails", Status = "In Progress" }
        };
        }
    }
}
