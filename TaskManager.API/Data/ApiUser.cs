using Microsoft.AspNetCore.Identity;

namespace TaskManager.API.Data;

public class ApiUser : IdentityUser
{
    public IList<TaskItem>? Tasks { get; set; }
}
