using System.Threading.Tasks;
using HomeschoolHelperApi.Data;
using HomeschoolHelperApi.DTOs.Users;
using HomeschoolHelperApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeschoolHelperApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepo _authRepo;
        public AuthenticationController(IAuthenticationRepo authRepo)
        {
            this._authRepo = authRepo;

        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(UserRegisterDTO user)
        {
            ServerResponse<int> response = await _authRepo.Register(
                new User  { Name = user.Name, Email = user.Email }, user.Password);
            
            if(response.Success == false) 
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> AuthenticateUser(UserLoginDTO user)
        {
            ServerResponse<string> response = await _authRepo.Login(user.Email, user.Password);
            if(response.Success == false) 
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }

    }
}