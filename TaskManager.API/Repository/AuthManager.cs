using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TaskManager.API.Contracts;
using TaskManager.API.Data;
using TaskManager.API.Models.Users;

namespace TaskManager.API.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IAuditLogRepository _auditLog;
        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration,
            IAuditLogRepository auditLog)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _auditLog = auditLog;
        }
        public async Task<IEnumerable<IdentityError>> RegisterUser(UserDto userDto)
        {
            //include confirm password in the userdto model and include the check
            var _user = _mapper.Map<ApiUser>(userDto);
            _user.UserName = userDto.Email;
            var result = await _userManager.CreateAsync(_user, userDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, "User");
                await _auditLog.LogDb("Create User", _user.Id);
            }

            return result.Errors;
        }

        public async Task<ResponseDto> LoginUser(LoginDto login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            var response = new ResponseDto();

            if (user == null)
            {
                response.Error = "User not found!";
                return response;
            }

            var tryLogin = await _userManager.CheckPasswordAsync(user, login.Password);

            if (tryLogin is false)
            {
                response.Error = "Invalid password!";
                return response;
            }
            await _auditLog.LogDb("User Login", user.Id);

            return await GenerateToken(user);

        }

        private async Task<ResponseDto> GenerateToken(ApiUser user)
        {
            var response = new ResponseDto();
            try
            {
                //byte[] key = new byte[32];
                //RandomNumberGenerator.Fill(key);
                //string base64key = Convert.ToBase64String(key); // key generation
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var roles = await _userManager.GetRolesAsync(user);

                var roleClaim = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

                var claims = new List<Claim>
                {
                    new (JwtRegisteredClaimNames.Sub, user.Id),
                    new (JwtRegisteredClaimNames.Email, user.Email),
                    new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }.Union(roleClaim);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddHours(1),
                    claims: claims,
                    signingCredentials: credentials
                    );

                response.Token = new JwtSecurityTokenHandler().WriteToken(token);

                return response;
            }

            catch (ArgumentOutOfRangeException e)
            {
                response.Exception = $"Ensure key length is greater than 32 bytes. Error: {e.Message}";
                return response;
            }
            catch (Exception e)
            {
                response.Exception ??= "";
                response.Exception += "\n" + e.Message;
                return response;
            }
        }
    }
}
