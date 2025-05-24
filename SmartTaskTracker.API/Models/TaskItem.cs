using System.ComponentModel.DataAnnotations;

namespace SmartTaskTracker.API.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public int UserId { get; set; }
        public string OwnerUserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? Deadline { get; set; }
        public string Priority { get; set; } = "Normal"; // Low, Normal, High
    }
}
