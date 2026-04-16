namespace Task_Management_System.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // Default status
        public int AssignedToUserId { get; set; } 
    }
}
