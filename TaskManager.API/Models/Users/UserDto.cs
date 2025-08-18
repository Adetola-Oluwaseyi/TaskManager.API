namespace TaskManager.API.Models.Users
{
    public class UserDto : LoginDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

    }
}
