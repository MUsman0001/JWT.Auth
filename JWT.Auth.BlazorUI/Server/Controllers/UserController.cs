using JWT.Auth.BlazorUI.Shared.Models;
using JWT.Auth.BlazorUI.Server.Service;
using JWT.Auth.BlazorUI.Shared.registration;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using JWT.Auth.BlazorUI.Shared.login;

namespace JWT.Auth.BlazorUI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserRegistrationDto userRegistration)
        {
            var result = await _userService.RegisterNewUser(userRegistration);
            if (result.IsUserRegistered)
            {
                return Ok(result.Message);
            }
            ModelState.AddModelError("Email", result.Message);
            return BadRequest(ModelState);
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto loginPayload)
        {
            var result = await _userService.LoginAsync(loginPayload);
            if (result.IsLoginSucess)
            {
                return Ok(result.TokenResponse);
            }

            ModelState.AddModelError("LoginError", "Invalid email or password");
            return BadRequest(ModelState);
        }

    }
}
