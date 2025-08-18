using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Contracts;
using TaskManager.API.Models.Users;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthManager _authManager;
        public AuthController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Register([FromBody] UserDto user)
        {
            //if(!)

            var errors = await _authManager.RegisterUser(user);

            if (errors.Any())
            {
                foreach (var item in errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Login([FromBody] LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isValid = await _authManager.LoginUser(login);

            if (isValid.Error is not null)
            {
                return Unauthorized(new { error = isValid.Error }); //for unauthenticated requests
                //return Forbid(); //for unathourized requests
            }

            if (isValid.Exception is not null)
            {
                return StatusCode(500, new { error = isValid.Exception });
            }

            return Ok(new { isValid.Token });


        }
    }
}
