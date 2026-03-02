using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
namespace TMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authservice;
        public AuthController(IAuthService authservice)
        {
            _authservice = authservice;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto logindto)
        {
                var token = await _authservice.LoginAsync(logindto);
                return Ok(new { Token = token });            
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerdto)
        {
            var result = await _authservice.RegisterAsync(registerdto);
            return Ok(result);
        }
        [HttpGet("GetallUsers")]
        [Authorize]
        public async Task<IActionResult> GetAllUsers() 
        {
            var result = await _authservice.GetAllUserAsync();
            return Ok(result);
        }
    }
}
