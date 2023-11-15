using EmpowerIdMicroservice.Application.Services;
using EmpowerIdMicroservice.WebApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace EmpowerIdMicroservice.WebApi.Controllers
{
     
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly AuthService _authService;

        public AuthController(TokenService tokenService, AuthService authService)
        {
            _tokenService = tokenService;
            _authService = authService;
        }
          
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            bool result = await _authService.Login(model.Username, model.Password);

            if (result)
            {
                string token = _tokenService.GenerateToken();
                return Ok(token);
            }

            return Unauthorized(new { Message = "Invalid username or password" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            var result = await _authService.RegisterUser(model.Fullname, model.Username, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Registration successful" });
            }

            return BadRequest(new { Message = "Registration failed", result.Errors });
        }
    }
}
