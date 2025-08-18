using TaskManager.API.Contracts;
using TaskManager.API.Data;

namespace TaskManager.API.Repository
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly TaskManagerDbContext _context;
        public AuditLogRepository(TaskManagerDbContext context)
        {
            _context = context;
        }

        public async Task LogDb(string process, string userId)
        {
            var auditLog = new AuditLog { Process = process, UserId = userId };
            await _context.AddAsync(auditLog);
            await _context.SaveChangesAsync();
        }
    }
}
