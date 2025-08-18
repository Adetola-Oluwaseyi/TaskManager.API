using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Data
{
    public class AuditLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public required string Process { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required]
        public required string UserId { get; set; }
    }
}
