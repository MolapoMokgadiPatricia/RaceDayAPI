using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public EventsController(RaceDayContext context)
        {
            _context = context;
        }

        private int? GetUserID()
        {
            return HttpContext.Session.GetInt32("UserID");
        }

        private string? GetUserRole()
        {
            return HttpContext.Session.GetString("Role");
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events
                .AsNoTracking()
                .OrderBy(e => e.EventDate)
                .ToListAsync();

            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var raceEvent = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (raceEvent == null)
            {
                return NotFound(new
                {
                    message = "Event not found."
                });
            }

            return Ok(raceEvent);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(
            [FromBody] Event newEvent)
        {
            int? userID = GetUserID();

            if (userID == null)
            {
                return Unauthorized(new
                {
                    message = "Please log in first."
                });
            }

            if (GetUserRole() != "Organiser")
            {
                return StatusCode(403, new
                {
                    message = "Only organisers can create events."
                });
            }

            bool organiserExists = await _context.Organisers
                .AnyAsync(o => o.OrganiserID == userID.Value);

            if (!organiserExists)
            {
                return BadRequest(new
                {
                    message = "An organiser profile must exist before creating events."
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            newEvent.EventID = 0;
            newEvent.OrganiserID = userID.Value;
            newEvent.CreatedAt = DateTime.UtcNow;

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEvent),
                new { id = newEvent.EventID },
                newEvent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(
            int id,
            [FromBody] Event updatedEvent)
        {
            int? userID = GetUserID();

            if (userID == null)
            {
                return Unauthorized(new
                {
                    message = "Please log in first."
                });
            }

            if (GetUserRole() != "Organiser")
            {
                return StatusCode(403, new
                {
                    message = "Only organisers can update events."
                });
            }

            var existingEvent = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (existingEvent == null)
            {
                return NotFound(new
                {
                    message = "Event not found."
                });
            }

            if (existingEvent.OrganiserID != userID.Value)
            {
                return StatusCode(403, new
                {
                    message = "You can only update your own events."
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            existingEvent.Name = updatedEvent.Name;
            existingEvent.Description = updatedEvent.Description;
            existingEvent.EventDate = updatedEvent.EventDate;
            existingEvent.Location = updatedEvent.Location;
            existingEvent.DistanceKm = updatedEvent.DistanceKm;
            existingEvent.EventType = updatedEvent.EventType;
            existingEvent.BannerImageUrl = updatedEvent.BannerImageUrl;

            await _context.SaveChangesAsync();

            return Ok(existingEvent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            int? userID = GetUserID();

            if (userID == null)
            {
                return Unauthorized(new
                {
                    message = "Please log in first."
                });
            }

            if (GetUserRole() != "Organiser")
            {
                return StatusCode(403, new
                {
                    message = "Only organisers can delete events."
                });
            }

            var raceEvent = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (raceEvent == null)
            {
                return NotFound(new
                {
                    message = "Event not found."
                });
            }

            if (raceEvent.OrganiserID != userID.Value)
            {
                return StatusCode(403, new
                {
                    message = "You can only delete your own events."
                });
            }

            _context.Events.Remove(raceEvent);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Event deleted successfully."
            });
        }
    }
}