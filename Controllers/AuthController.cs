using Microsoft.AspNetCore.Mvc;
using TodoBackend.Data.Entities;
using TodoBackend.Dtos;
using TodoBackend.Services;
using TodoBackend.Utils;

namespace TodoBackend.Controllers
{
    // Controller to manage user authentication-related actions (register, login)
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        // Constructor to initialize the user service for interacting with user data
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        // Accepts a RegisterRequestDto containing the necessary data (username, email, password)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var hashedPassword = PasswordUtils.EncryptPassword(request.Password);
            var user = new UserEntity { Username = request.Username, Email = request.Email, Password = hashedPassword };
            var createUser = await _userService.CreateUser(user);

            return CreatedAtAction(nameof(Register), new { id = createUser.Uuid }, createUser);
        }

        // Accepts a LoginRequestDto containing the user's email and password
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            // Validate the request body to ensure required fields are present
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Invalid input" });
            }

            // Retrieve the user by email
            var user = await _userService.GetOneUserByEmail(request.Email);

            // Check if the user exists, is not deleted, and if the password matches
            if (user == null || user.DeletedAt != null || !PasswordUtils.ComparePassword(request.Password, user.Password))
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var token = TokenUtils.GenerateToken(user.Uuid.ToString());

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(60)
            });

            // Return a success response with the username of the logged-in user
            return Ok(new { Message = "Login successful", Success = true, Name = user.Username });
        }
    }
}
