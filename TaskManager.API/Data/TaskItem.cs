using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Data
{
    public class TaskItem
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }//set a default value for this
        public TaskExecutionStatus Status { get; set; }//set a default value for this
        [Required]
        public string UserId { get; set; } = default!;
        public ApiUser User { get; set; } = default!;
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum TaskExecutionStatus
    {
        NotStarted,
        InProgress,
        Completed,
        OnHold,
        Cancelled
    }
}
