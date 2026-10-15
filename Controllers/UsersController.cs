using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Attributes;
using RaceDayAPI.Data;
using RaceDayAPI.Extensions;

namespace RaceDayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public UsersController(RaceDayContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AuthorizeRole("Organiser")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                .AsNoTracking()
                .Select(u => new
                {
                    u.UserID,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.Role,
                    u.PhoneNumber,
                    u.CreatedAt
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id:int}")]
        [AuthorizeRole]
        public async Task<IActionResult> GetUser(int id)
        {
            int userId = HttpContext.GetUserId();

            if (HttpContext.IsParticipant() && id != userId)
            {
                return StatusCode(403, new { message = "You can only view your own profile." });
            }

            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.UserID == id)
                .Select(u => new
                {
                    u.UserID,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.Role,
                    u.PhoneNumber,
                    u.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            return Ok(user);
        }

        [HttpPut("{id:int}")]
        [AuthorizeRole]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            int userId = HttpContext.GetUserId();

            if (id != userId)
            {
                return StatusCode(403, new { message = "You can only update your own profile." });
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == id);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            if (!string.IsNullOrWhiteSpace(request.FirstName))
            {
                if (request.FirstName.Trim().Length > 50)
                {
                    return BadRequest(new
                    {
                        message = "First name must not exceed 50 characters."
                    });
                }

                user.FirstName = request.FirstName.Trim();
            }

            if (!string.IsNullOrWhiteSpace(request.LastName))
            {
                if (request.LastName.Trim().Length > 50)
                {
                    return BadRequest(new
                    {
                        message = "Last name must not exceed 50 characters."
                    });
                }

                user.LastName = request.LastName.Trim();
            }

            if (request.PhoneNumber != null)
            {
                if (request.PhoneNumber.Length > 20)
                {
                    return BadRequest(new
                    {
                        message = "Phone number must not exceed 20 characters."
                    });
                }

                user.PhoneNumber = request.PhoneNumber.Trim();
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                user.UserID,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Role,
                user.PhoneNumber,
                user.CreatedAt
            });
        }
    }

    public class UpdateUserRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}