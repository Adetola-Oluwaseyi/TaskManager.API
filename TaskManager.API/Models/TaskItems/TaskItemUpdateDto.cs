using TaskManager.API.Data;

namespace TaskManager.API.Models.TaskItems
{
    public class TaskItemUpdateDto
    {
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskExecutionStatus Status { get; set; }

    }
}
