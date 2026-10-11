using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.CSharp.Data;
using RaceDay.API.CSharp.Models;
using System.Security.Claims;

namespace RaceDay.API.CSharp.Controllers
{
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public CategoriesController(RaceDayContext context)
        {
            _context = context;
        }

        // GET: api/events/1/categories
        [HttpGet("api/events/{eventId:int}/categories")]
        [Authorize]
        public async Task<IActionResult> GetCategories(int eventId)
        {
            var eventExists = await _context.Events
                .AnyAsync(e => e.EventID == eventId);

            if (!eventExists)
            {
                return NotFound("Event not found.");
            }

            var categories = await _context.Categories
                .AsNoTracking()
                .Where(c => c.EventID == eventId)
                .ToListAsync();

            return Ok(categories);
        }

        // POST: api/events/1/categories
        [HttpPost("api/events/{eventId:int}/categories")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> CreateCategory(
            int eventId,
            Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            if (!int.TryParse(userIdClaim, out int organiserId))
            {
                return Unauthorized();
            }

            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == eventId);

            if (eventItem == null)
            {
                return NotFound("Event not found.");
            }

            if (eventItem.OrganiserID != organiserId)
            {
                return Forbid();
            }

            category.CategoryID = 0;
            category.EventID = eventId;
            category.Event = null;

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Created(
                $"/api/events/{eventId}/categories",
                category
            );
        }

        // PUT: api/categories/1
        [HttpPut("api/categories/{id:int}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> UpdateCategory(
            int id,
            Category updatedCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            if (!int.TryParse(userIdClaim, out int organiserId))
            {
                return Unauthorized();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryID == id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            var eventItem = await _context.Events
                .FirstOrDefaultAsync(
                    e => e.EventID == category.EventID
                );

            if (eventItem == null)
            {
                return NotFound("The category's event was not found.");
            }

            if (eventItem.OrganiserID != organiserId)
            {
                return Forbid();
            }

            category.CategoryName = updatedCategory.CategoryName;
            category.Description = updatedCategory.Description;
            category.AgeMinimum = updatedCategory.AgeMinimum;
            category.AgeMaximum = updatedCategory.AgeMaximum;
            category.CategoryFee = updatedCategory.CategoryFee;

            await _context.SaveChangesAsync();

            return Ok(category);
        }

        // DELETE: api/categories/1
        [HttpDelete("api/categories/{id:int}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            if (!int.TryParse(userIdClaim, out int organiserId))
            {
                return Unauthorized();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryID == id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            var eventItem = await _context.Events
                .FirstOrDefaultAsync(
                    e => e.EventID == category.EventID
                );

            if (eventItem == null)
            {
                return NotFound("The category's event was not found.");
            }

            if (eventItem.OrganiserID != organiserId)
            {
                return Forbid();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
