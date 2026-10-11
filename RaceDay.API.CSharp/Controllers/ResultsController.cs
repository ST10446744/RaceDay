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
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public ResultsController(RaceDayContext context)
        {
            _context = context;
        }

        // POST: api/results
        // Organisers can record results for their own events.
        [HttpPost("results")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> CreateResult(
            CreateResultRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int organiserId))
                return Unauthorized();

            var enrolment = await _context.Enrolments
                .Include(e => e.Category)
                .ThenInclude(c => c!.Event)
                .FirstOrDefaultAsync(
                    e => e.EnrolmentID == request.EnrolmentID);

            if (enrolment == null)
                return NotFound("Enrolment not found.");

            if (enrolment.Category?.Event?.OrganiserID != organiserId)
                return Forbid();

            bool resultExists = await _context.Results
                .AnyAsync(r => r.EnrolmentID == request.EnrolmentID);

            if (resultExists)
                return Conflict("A result already exists for this enrolment.");

            var result = new Result
            {
                EnrolmentID = request.EnrolmentID,
                FinishPosition = request.FinishPosition,
                FinishTime = request.FinishTime,
                ResultStatus = request.ResultStatus,
                RecordedDate = DateTime.Now
            };

            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            return StatusCode(201, new
            {
                result.ResultID,
                result.EnrolmentID,
                result.FinishPosition,
                result.FinishTime,
                result.ResultStatus,
                result.RecordedDate
            });
        }

        // PUT: api/results/1
        // Organisers can update results for their own events.
        [HttpPut("results/{id:int}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> UpdateResult(
            int id,
            UpdateResultRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int organiserId))
                return Unauthorized();

            var result = await _context.Results
                .Include(r => r.Enrolment)
                .ThenInclude(e => e!.Category)
                .ThenInclude(c => c!.Event)
                .FirstOrDefaultAsync(r => r.ResultID == id);

            if (result == null)
                return NotFound("Result not found.");

            if (result.Enrolment?.Category?.Event?.OrganiserID
                != organiserId)
                return Forbid();

            result.FinishPosition = request.FinishPosition;
            result.FinishTime = request.FinishTime;
            result.ResultStatus = request.ResultStatus;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                result.ResultID,
                result.EnrolmentID,
                result.FinishPosition,
                result.FinishTime,
                result.ResultStatus,
                result.RecordedDate
            });
        }

        // GET: api/results/my
        // Participants can view their own results.
        [HttpGet("results/my")]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> GetMyResults()
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int participantId))
                return Unauthorized();

            var results = await _context.Results
                .AsNoTracking()
                .Where(r => r.Enrolment!.ParticipantID == participantId)
                .Select(r => new
                {
                    r.ResultID,
                    r.EnrolmentID,
                    EventName = r.Enrolment!.Category!.Event!.EventName,
                    CategoryName = r.Enrolment!.Category!.CategoryName,
                    r.FinishPosition,
                    r.FinishTime,
                    r.ResultStatus,
                    r.RecordedDate
                })
                .ToListAsync();

            return Ok(results);
        }

        // GET: api/events/1/results
        // Organisers can view results for their own events.
        [HttpGet("events/{eventId:int}/results")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> GetEventResults(int eventId)
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int organiserId))
                return Unauthorized();

            var eventItem = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EventID == eventId);

            if (eventItem == null)
                return NotFound("Event not found.");

            if (eventItem.OrganiserID != organiserId)
                return Forbid();

            var results = await _context.Results
                .AsNoTracking()
                .Where(r => r.Enrolment!.Category!.EventID == eventId)
                .Select(r => new
                {
                    r.ResultID,
                    r.EnrolmentID,
                    ParticipantName =
                        r.Enrolment!.Participant!.FirstName + " " +
                        r.Enrolment.Participant.LastName,
                    CategoryName = r.Enrolment!.Category!.CategoryName,
                    r.FinishPosition,
                    r.FinishTime,
                    r.ResultStatus,
                    r.RecordedDate
                })
                .ToListAsync();

            return Ok(results);
        }

        // GET: api/results/1
        // Participants can view their own results.
        // Organisers can view results for their own events.
        [HttpGet("results/{id:int}")]
        [Authorize(Roles = "Participant,Organiser")]
        public async Task<IActionResult> GetResult(int id)
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var result = await _context.Results
                .AsNoTracking()
                .Include(r => r.Enrolment)
                .ThenInclude(e => e!.Category)
                .ThenInclude(c => c!.Event)
                .FirstOrDefaultAsync(r => r.ResultID == id);

            if (result == null)
                return NotFound("Result not found.");

            bool isParticipant =
                User.IsInRole("Participant") &&
                result.Enrolment?.ParticipantID == userId;

            bool isOrganiser =
                User.IsInRole("Organiser") &&
                result.Enrolment?.Category?.Event?.OrganiserID == userId;

            if (!isParticipant && !isOrganiser)
                return Forbid();

            return Ok(new
            {
                result.ResultID,
                result.EnrolmentID,
                EventName = result.Enrolment?.Category?.Event?.EventName,
                CategoryName = result.Enrolment?.Category?.CategoryName,
                result.FinishPosition,
                result.FinishTime,
                result.ResultStatus,
                result.RecordedDate
            });
        }
    }

    public class CreateResultRequest
    {
        [Range(1, int.MaxValue)]
        public int EnrolmentID { get; set; }

        [Range(1, int.MaxValue)]
        public int? FinishPosition { get; set; }

        public TimeSpan? FinishTime { get; set; }

        [Required]
        [MaxLength(20)]
        public string ResultStatus { get; set; } = "Finished";
    }

    public class UpdateResultRequest
    {
        [Range(1, int.MaxValue)]
        public int? FinishPosition { get; set; }

        public TimeSpan? FinishTime { get; set; }

        [Required]
        [MaxLength(20)]
        public string ResultStatus { get; set; } = "Finished";
    }
}
