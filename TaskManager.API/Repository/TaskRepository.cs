using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Contracts;
using TaskManager.API.Data;
using TaskManager.API.Models.TaskItems;

namespace TaskManager.API.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagerDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuditLogRepository _auditLog;

        public TaskRepository(TaskManagerDbContext context, IMapper mapper,
            IAuditLogRepository auditLog)
        {
            _context = context;
            _mapper = mapper;
            _auditLog = auditLog;
        }

        public async Task<IEnumerable<TaskItemDto?>> GetAllTaskItems(string userId)
        {
            var tasks = await _context.Tasks
                .Where(c => c.UserId == userId)
                .ProjectTo<TaskItemDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            if (tasks is not null)
            {
                await _auditLog.LogDb("Get Tasks", userId);
            }

            return tasks;
        }

        public async Task<TaskItemDto?> GetTaskItem(string userId, int id, bool forDelete)
        {
            var task = await _context.Tasks
                .Where(c => c.UserId == userId && c.Id == id)
                .ProjectTo<TaskItemDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (task is not null && forDelete is false)
            {
                await _auditLog.LogDb("Get Task", userId);
            }

            return task;
        }

        public async Task CreateTaskItem(string userId, TaskItemDto taskItem)
        {
            var task = _mapper.Map<TaskItem>(taskItem);
            task.UserId = userId;
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            await _auditLog.LogDb("Create Task", userId);
        }

        public async Task DeleteTaskItem(string userId, TaskItemDto taskItem, int id)
        {
            var task = _mapper.Map<TaskItem>(taskItem);

            task.Id = id;

            _context.Remove(task);

            await _context.SaveChangesAsync();

            await _auditLog.LogDb("Delete Task", userId);
        }
        public async Task<bool> UpdateTaskItem(string userId, TaskItemDto taskItem, int id)
        {
            var exists = _context.Tasks.Any(e => e.Id == id && e.UserId == userId);

            if (!exists)
                return exists;

            var task = _mapper.Map<TaskItem>(taskItem);
            task.Id = id;
            task.UserId = userId;


            _context.Update(task);

            await _context.SaveChangesAsync();

            await _auditLog.LogDb("Update Task", userId);

            return exists;
        }
    }
}
