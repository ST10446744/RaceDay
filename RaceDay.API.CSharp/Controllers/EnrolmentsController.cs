using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.CSharp.Data;
using RaceDay.API.CSharp.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace RaceDay.API.CSharp.Controllers
{
    [ApiController]
    [Route("api")]
    public class EnrolmentsController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public EnrolmentsController(RaceDayContext context)
        {
            _context = context;
        }

        // POST: api/enrolments
        // Participants can enrol themselves in a category.
        [HttpPost("enrolments")]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> CreateEnrolment(
            CreateEnrolmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            if (!int.TryParse(userIdClaim, out int participantId))
            {
                return Unauthorized();
            }

            var category = await _context.Categories
                .Include(c => c.Event)
                .FirstOrDefaultAsync(
                    c => c.CategoryID == request.CategoryID
                );

            if (category == null)
            {
                return NotFound("Race category not found.");
            }

            if (category.Event == null ||
                category.Event.Status != "Upcoming")
            {
                return BadRequest(
                    "This event is not available for enrolment."
                );
            }

            bool alreadyEnrolled = await _context.Enrolments
                .AnyAsync(e =>
                    e.ParticipantID == participantId &&
                    e.CategoryID == request.CategoryID &&
                    e.EnrolmentStatus == "Active"
                );

            if (alreadyEnrolled)
            {
                return Conflict(
                    "You are already enrolled in this category."
                );
            }

            var enrolment = new Enrolment
            {
                ParticipantID = participantId,
                CategoryID = request.CategoryID,
                EmergencyContactName =
                    request.EmergencyContactName.Trim(),
                EmergencyContactPhone =
                    request.EmergencyContactPhone.Trim(),
                EnrolmentDate = DateTime.Now,
                PaymentStatus = "Pending",
                EnrolmentStatus = "Active"
            };

            _context.Enrolments.Add(enrolment);

            await _context.SaveChangesAsync();

            return StatusCode(201, new
            {
                enrolment.EnrolmentID,
                enrolment.ParticipantID,
                enrolment.CategoryID,
                enrolment.EnrolmentDate,
                enrolment.EmergencyContactName,
                enrolment.EmergencyContactPhone,
                enrolment.PaymentStatus,
                enrolment.EnrolmentStatus
            });
        }

        // GET: api/enrolments/my
        // Participants can view their own enrolments.
        [HttpGet("enrolments/my")]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> GetMyEnrolments()
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            if (!int.TryParse(userIdClaim, out int participantId))
            {
                return Unauthorized();
            }

            var enrolments = await _context.Enrolments
                .AsNoTracking()
                .Where(e => e.ParticipantID == participantId)
                .Select(e => new
                {
                    e.EnrolmentID,
                    e.ParticipantID,
                    e.CategoryID,
                    CategoryName = e.Category!.CategoryName,
                    EventID = e.Category!.EventID,
                    EventName = e.Category!.Event!.EventName,
                    e.EnrolmentDate,
                    e.EmergencyContactName,
                    e.EmergencyContactPhone,
                    e.PaymentStatus,
                    e.EnrolmentStatus
                })
                .ToListAsync();

            return Ok(enrolments);
        }

        // GET: api/enrolments/1
        // Participants can view their own enrolments.
        // Organisers can view enrolments for their own events.
        [HttpGet("enrolments/{id:int}")]
        [Authorize(Roles = "Participant,Organiser")]
        public async Task<IActionResult> GetEnrolment(int id)
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            var enrolment = await _context.Enrolments
                .AsNoTracking()
                .Include(e => e.Category)
                .ThenInclude(c => c!.Event)
                .FirstOrDefaultAsync(e => e.EnrolmentID == id);

            if (enrolment == null)
            {
                return NotFound("Enrolment not found.");
            }

            bool isParticipant =
                User.IsInRole("Participant") &&
                enrolment.ParticipantID == userId;

            bool isOrganiser =
                User.IsInRole("Organiser") &&
                enrolment.Category?.Event?.OrganiserID == userId;

            if (!isParticipant && !isOrganiser)
            {
                return Forbid();
            }

            return Ok(new
            {
                enrolment.EnrolmentID,
                enrolment.ParticipantID,
                enrolment.CategoryID,
                CategoryName = enrolment.Category?.CategoryName,
                EventName = enrolment.Category?.Event?.EventName,
                enrolment.EnrolmentDate,
                enrolment.EmergencyContactName,
                enrolment.EmergencyContactPhone,
                enrolment.PaymentStatus,
                enrolment.EnrolmentStatus
            });
        }

        // DELETE: api/enrolments/1
        // Participants can cancel their own enrolments.
        [HttpDelete("enrolments/{id:int}")]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> CancelEnrolment(int id)
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            if (!int.TryParse(userIdClaim, out int participantId))
            {
                return Unauthorized();
            }

            var enrolment = await _context.Enrolments
                .FirstOrDefaultAsync(e => e.EnrolmentID == id);

            if (enrolment == null)
            {
                return NotFound("Enrolment not found.");
            }

            if (enrolment.ParticipantID != participantId)
            {
                return Forbid();
            }

            enrolment.EnrolmentStatus = "Cancelled";

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Enrolment cancelled successfully.",
                enrolment.EnrolmentID,
                enrolment.EnrolmentStatus
            });
        }

        // GET: api/events/1/enrolments
        // Organisers can view enrolments for their own events.
        [HttpGet("events/{eventId:int}/enrolments")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> GetEventEnrolments(
            int eventId)
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            if (!int.TryParse(userIdClaim, out int organiserId))
            {
                return Unauthorized();
            }

            var eventItem = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EventID == eventId);

            if (eventItem == null)
            {
                return NotFound("Event not found.");
            }

            if (eventItem.OrganiserID != organiserId)
            {
                return Forbid();
            }

            var enrolments = await _context.Enrolments
                .AsNoTracking()
                .Where(e => e.Category!.EventID == eventId)
                .Select(e => new
                {
                    e.EnrolmentID,
                    e.ParticipantID,
                    ParticipantName =
                        e.Participant!.FirstName + " " +
                        e.Participant.LastName,
                    e.CategoryID,
                    CategoryName = e.Category!.CategoryName,
                    e.EnrolmentDate,
                    e.EmergencyContactName,
                    e.EmergencyContactPhone,
                    e.PaymentStatus,
                    e.EnrolmentStatus
                })
                .ToListAsync();

            return Ok(enrolments);
        }
    }

    public class CreateEnrolmentRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int CategoryID { get; set; }

        [Required]
        [MaxLength(100)]
        public string EmergencyContactName { get; set; }
            = string.Empty;

        [Required]
        [MaxLength(20)]
        public string EmergencyContactPhone { get; set; }
            = string.Empty;
    }
}