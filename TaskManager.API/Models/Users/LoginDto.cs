using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Models.Users
{
    public class LoginDto
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
