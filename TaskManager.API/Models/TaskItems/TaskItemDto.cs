using System.ComponentModel.DataAnnotations;
using TaskManager.API.Data;

namespace TaskManager.API.Models.TaskItems
{
    public class TaskItemDto
    {
        [Required]
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskExecutionStatus Status { get; set; }

    }
}
