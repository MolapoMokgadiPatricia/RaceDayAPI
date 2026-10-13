using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrolmentsController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public EnrolmentsController(RaceDayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEnrolments()
        {
            var enrolments = await _context.Enrolments
                .AsNoTracking()
                .OrderBy(e => e.EnrolmentID)
                .ToListAsync();

            return Ok(enrolments);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEnrolment(int id)
        {
            var enrolment = await _context.Enrolments
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EnrolmentID == id);

            if (enrolment == null)
            {
                return NotFound(new { message = "Enrolment not found." });
            }

            return Ok(enrolment);
        }

        [HttpGet("participant/{participantId:int}")]
        public async Task<IActionResult> GetParticipantEnrolments(int participantId)
        {
            var participantExists = await _context.Participants
                .AnyAsync(p => p.ParticipantID == participantId);

            if (!participantExists)
            {
                return NotFound(new { message = "Participant not found." });
            }

            var enrolments = await _context.Enrolments
                .AsNoTracking()
                .Where(e => e.ParticipantID == participantId)
                .OrderByDescending(e => e.EnrolmentDate)
                .ToListAsync();

            return Ok(enrolments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEnrolment(
            [FromBody] Enrolment enrolment)
        {
            if (enrolment.ParticipantID <= 0 ||
                enrolment.EventID <= 0 ||
                enrolment.CategoryID <= 0)
            {
                return BadRequest(new
                {
                    message = "Valid ParticipantID, EventID and CategoryID are required."
                });
            }

            var participantExists = await _context.Participants
                .AnyAsync(p => p.ParticipantID == enrolment.ParticipantID);

            if (!participantExists)
            {
                return BadRequest(new { message = "Participant does not exist." });
            }

            var eventExists = await _context.Events
                .AnyAsync(e => e.EventID == enrolment.EventID);

            if (!eventExists)
            {
                return BadRequest(new { message = "Event does not exist." });
            }

            var categoryExists = await _context.Categories
                .AnyAsync(c => c.CategoryID == enrolment.CategoryID
                            && c.EventID == enrolment.EventID);

            if (!categoryExists)
            {
                return BadRequest(new
                {
                    message = "The category does not belong to the selected event."
                });
            }

            var alreadyEnrolled = await _context.Enrolments.AnyAsync(e =>
                e.ParticipantID == enrolment.ParticipantID &&
                e.EventID == enrolment.EventID);

            if (alreadyEnrolled)
            {
                return Conflict(new
                {
                    message = "This participant is already enrolled in this event."
                });
            }

            enrolment.EnrolmentDate = DateTime.UtcNow;
            enrolment.Status = "Pending";

            _context.Enrolments.Add(enrolment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEnrolment),
                new { id = enrolment.EnrolmentID },
                enrolment);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEnrolment(
            int id,
            [FromBody] Enrolment updatedEnrolment)
        {
            var existingEnrolment = await _context.Enrolments
                .FirstOrDefaultAsync(e => e.EnrolmentID == id);

            if (existingEnrolment == null)
            {
                return NotFound(new { message = "Enrolment not found." });
            }

            if (updatedEnrolment.ParticipantID <= 0 ||
                updatedEnrolment.EventID <= 0 ||
                updatedEnrolment.CategoryID <= 0)
            {
                return BadRequest(new
                {
                    message = "Valid ParticipantID, EventID and CategoryID are required."
                });
            }

            var participantExists = await _context.Participants
                .AnyAsync(p => p.ParticipantID == updatedEnrolment.ParticipantID);

            var eventExists = await _context.Events
                .AnyAsync(e => e.EventID == updatedEnrolment.EventID);

            var categoryExists = await _context.Categories.AnyAsync(c =>
                c.CategoryID == updatedEnrolment.CategoryID &&
                c.EventID == updatedEnrolment.EventID);

            if (!participantExists || !eventExists || !categoryExists)
            {
                return BadRequest(new
                {
                    message = "Check that the participant, event and matching category exist."
                });
            }

            var duplicate = await _context.Enrolments.AnyAsync(e =>
                e.EnrolmentID != id &&
                e.ParticipantID == updatedEnrolment.ParticipantID &&
                e.EventID == updatedEnrolment.EventID);

            if (duplicate)
            {
                return Conflict(new
                {
                    message = "This participant is already enrolled in this event."
                });
            }

            existingEnrolment.ParticipantID = updatedEnrolment.ParticipantID;
            existingEnrolment.EventID = updatedEnrolment.EventID;
            existingEnrolment.CategoryID = updatedEnrolment.CategoryID;

            if (!string.IsNullOrWhiteSpace(updatedEnrolment.Status))
            {
                var allowedStatuses = new[] { "Pending", "Approved", "Rejected", "Cancelled" };

                if (!allowedStatuses.Contains(updatedEnrolment.Status))
                {
                    return BadRequest(new
                    {
                        message = "Status must be Pending, Approved, Rejected or Cancelled."
                    });
                }

                existingEnrolment.Status = updatedEnrolment.Status;
            }

            await _context.SaveChangesAsync();

            return Ok(existingEnrolment);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEnrolment(int id)
        {
            var enrolment = await _context.Enrolments
                .FirstOrDefaultAsync(e => e.EnrolmentID == id);

            if (enrolment == null)
            {
                return NotFound(new { message = "Enrolment not found." });
            }

            var hasResult = await _context.Results
                .AnyAsync(r => r.EnrolmentID == id);

            if (hasResult)
            {
                return Conflict(new
                {
                    message = "This enrolment has a result and cannot be deleted."
                });
            }

            _context.Enrolments.Remove(enrolment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Enrolment deleted successfully." });
        }
    }
}