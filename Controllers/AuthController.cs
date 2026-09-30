using IOT.Models;
using IOT.Repositories;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace IOT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenRepository _tokenRepository;
        private IUserRepository _userRepository;

        public AuthController(ITokenRepository tokenRepository, IUserRepository userRepo)
        {
            _tokenRepository = tokenRepository;
            _userRepository = userRepo;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            var user = await _userRepository.GetByNameAsync(request.UserName);

            if(user == null)
            {
                return Unauthorized("User not found.");
            }   

            // Simple mock check (replace with DbContext user validation)
            if (request.UserName == user.UserName && request.MobileNumber == user.MobileNumber)
            {
                var token = _tokenRepository.GenerateToken(
                    UserName: "101",
                    MobileNumber: request.MobileNumber,
                    UserId:user.Id
                );

                return Ok(new { Token = token });
            }
            else
            {
                return Unauthorized("Invalid credentials.");
            }
        }
    }
}
