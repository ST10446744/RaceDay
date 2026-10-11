using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.CSharp.Data;
using RaceDay.API.CSharp.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
namespace RaceDay.API.CSharp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly RaceDayContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new();


    public AuthController(RaceDayContext context)
        {
            _context = context;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            request.Email = request.Email.Trim().ToLowerInvariant();

            if (request.Role != "Organiser" && request.Role != "Participant")
                return BadRequest("Role must be Organiser or Participant.");

            if (string.IsNullOrWhiteSpace(request.Password) ||
                request.Password.Length < 8)
            {
                return BadRequest("Password must be at least 8 characters long.");
            }

            bool emailExists = await _context.Users
                .AnyAsync(u => u.Email == request.Email);

            if (emailExists)
                return BadRequest("This email address is already registered.");

            var user = new User
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Email = request.Email,
                Role = request.Role,
                PhoneNumber = request.PhoneNumber?.Trim()
            };

            user.PasswordHash = _passwordHasher.HashPassword(
                user,
                request.Password
            );

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return StatusCode(201, new
            {
                user.UserID,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Role,
                user.PhoneNumber
            });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string email = request.Email.Trim().ToLowerInvariant();

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return Unauthorized("Invalid email or password.");

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password
            );

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid email or password.");

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var identity = new ClaimsIdentity(
                claims,
                "RaceDayCookie"
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                "RaceDayCookie",
                principal
            );

            return Ok(new
            {
                Message = "Login successful.",
                user.UserID,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Role
            });
        }

        // POST: api/auth/logout
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("RaceDayCookie");

            return Ok(new { Message = "Logout successful." });
        }
    }

    public class RegisterRequest
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Participant";

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
    }

    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }


}

