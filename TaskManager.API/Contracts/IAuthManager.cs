using Microsoft.AspNetCore.Identity;
using TaskManager.API.Models.Users;

namespace TaskManager.API.Contracts
{
    public interface IAuthManager
    {
        Task<ResponseDto> LoginUser(LoginDto login);
        Task<IEnumerable<IdentityError>> RegisterUser(UserDto user);
    }
}
