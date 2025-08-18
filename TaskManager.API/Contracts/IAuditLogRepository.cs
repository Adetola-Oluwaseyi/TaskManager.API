namespace TaskManager.API.Contracts
{
    public interface IAuditLogRepository
    {
        Task LogDb(string process, string userId);
    }
}
