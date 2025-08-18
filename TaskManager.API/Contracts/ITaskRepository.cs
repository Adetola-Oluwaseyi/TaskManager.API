using TaskManager.API.Models.TaskItems;

namespace TaskManager.API.Contracts
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItemDto?>> GetAllTaskItems(string userId);
        Task<TaskItemDto?> GetTaskItem(string userId, int id, bool forDelete);
        Task CreateTaskItem(string userId, TaskItemDto taskItem);
        Task DeleteTaskItem(string userId, TaskItemDto taskItem, int id);
        Task<bool> UpdateTaskItem(string userId, TaskItemDto taskItem, int id);


    }
}
