using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.CSharp.Data;
using RaceDay.API.CSharp.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace RaceDay.API.CSharp.Controllers
{
    [Route("api/profile")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public ProfileController(RaceDayContext context)
        {
            _context = context;
        }

        // GET: api/profile
        // View the currently logged-in user's profile.
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.UserID == userId)
                .Select(u => new
                {
                    u.UserID,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.Role,
                    u.PhoneNumber,
                    u.DateRegistered
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("User profile not found.");
            }

            return Ok(user);
        }

        // PUT: api/profile
        // Update the currently logged-in user's profile.
        [HttpPut]
        public async Task<IActionResult> UpdateProfile(
            UpdateProfileRequest request)
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == userId);

            if (user == null)
            {
                return NotFound("User profile not found.");
            }

            if (string.IsNullOrWhiteSpace(request.FirstName) ||
                string.IsNullOrWhiteSpace(request.LastName))
            {
                return BadRequest(
                    "First name and last name are required."
                );
            }

            user.FirstName = request.FirstName.Trim();
            user.LastName = request.LastName.Trim();
            user.PhoneNumber = request.PhoneNumber?.Trim();

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Profile updated successfully.",
                user.UserID,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Role,
                user.PhoneNumber
            });
        }
    }

    public class UpdateProfileRequest
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
    }
}