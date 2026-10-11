using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.CSharp.Data;
using RaceDay.API.CSharp.Models;
using System.Security.Claims;

namespace RaceDay.API.CSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public EventsController(RaceDayContext context)
        {
            _context = context;
        }

        // GET: api/events
        // Anyone can view all events.
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events
                .AsNoTracking()
                .ToListAsync();

            return Ok(events);
        }

        // GET: api/events/1
        // Anyone can view one event.
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var eventItem = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (eventItem == null)
            {
                return NotFound("Event not found.");
            }

            return Ok(eventItem);
        }

        // POST: api/events
        // Only organisers can create events.
        [HttpPost]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> CreateEvent(Event eventItem)
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

            eventItem.OrganiserID = organiserId;
            eventItem.Organiser = null;

            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEvent),
                new { id = eventItem.EventID },
                eventItem
            );
        }

        // PUT: api/events/1
        // Organisers can update only their own events.
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> UpdateEvent(
            int id,
            Event updatedEvent)
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

            var existingEvent = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (existingEvent == null)
            {
                return NotFound("Event not found.");
            }

            if (existingEvent.OrganiserID != organiserId)
            {
                return Forbid();
            }

            existingEvent.EventName = updatedEvent.EventName;
            existingEvent.Description = updatedEvent.Description;
            existingEvent.EventType = updatedEvent.EventType;
            existingEvent.EventDate = updatedEvent.EventDate;
            existingEvent.StartTime = updatedEvent.StartTime;
            existingEvent.Venue = updatedEvent.Venue;
            existingEvent.City = updatedEvent.City;
            existingEvent.Province = updatedEvent.Province;
            existingEvent.DistanceKM = updatedEvent.DistanceKM;
            existingEvent.EntryFee = updatedEvent.EntryFee;
            existingEvent.MaximumParticipants =
                updatedEvent.MaximumParticipants;
            existingEvent.Status = updatedEvent.Status;

            await _context.SaveChangesAsync();

            return Ok(existingEvent);
        }

        // DELETE: api/events/1
        // Organisers can delete only their own events.
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            if (!int.TryParse(userIdClaim, out int organiserId))
            {
                return Unauthorized();
            }

            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (eventItem == null)
            {
                return NotFound("Event not found.");
            }

            if (eventItem.OrganiserID != organiserId)
            {
                return Forbid();
            }

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
