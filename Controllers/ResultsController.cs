using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public ResultsController(RaceDayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetResults()
        {
            var results = await _context.Results
                .AsNoTracking()
                .OrderBy(r => r.ResultID)
                .ToListAsync();

            return Ok(results);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetResult(int id)
        {
            var result = await _context.Results
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.ResultID == id);

            if (result == null)
            {
                return NotFound(new { message = "Result not found." });
            }

            return Ok(result);
        }

        [HttpGet("enrolment/{enrolmentId:int}")]
        public async Task<IActionResult> GetResultByEnrolment(int enrolmentId)
        {
            var enrolmentExists = await _context.Enrolments
                .AnyAsync(e => e.EnrolmentID == enrolmentId);

            if (!enrolmentExists)
            {
                return NotFound(new { message = "Enrolment not found." });
            }

            var result = await _context.Results
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.EnrolmentID == enrolmentId);

            if (result == null)
            {
                return NotFound(new { message = "No result has been recorded for this enrolment." });
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateResult([FromBody] Result result)
        {
            if (result.EnrolmentID <= 0)
            {
                return BadRequest(new { message = "A valid EnrolmentID is required." });
            }

            var enrolment = await _context.Enrolments
                .FirstOrDefaultAsync(e => e.EnrolmentID == result.EnrolmentID);

            if (enrolment == null)
            {
                return BadRequest(new { message = "The enrolment does not exist." });
            }

            var existingResult = await _context.Results
                .AnyAsync(r => r.EnrolmentID == result.EnrolmentID);

            if (existingResult)
            {
                return Conflict(new { message = "A result already exists for this enrolment." });
            }

            if (result.FinishPosition.HasValue && result.FinishPosition.Value <= 0)
            {
                return BadRequest(new { message = "FinishPosition must be greater than zero." });
            }

            result.RecordedAt = DateTime.UtcNow;

            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetResult),
                new { id = result.ResultID },
                result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateResult(int id, [FromBody] Result updatedResult)
        {
            var existingResult = await _context.Results
                .FirstOrDefaultAsync(r => r.ResultID == id);

            if (existingResult == null)
            {
                return NotFound(new { message = "Result not found." });
            }

            if (updatedResult.EnrolmentID <= 0)
            {
                return BadRequest(new { message = "A valid EnrolmentID is required." });
            }

            var enrolmentExists = await _context.Enrolments
                .AnyAsync(e => e.EnrolmentID == updatedResult.EnrolmentID);

            if (!enrolmentExists)
            {
                return BadRequest(new { message = "The enrolment does not exist." });
            }

            var duplicate = await _context.Results.AnyAsync(r =>
                r.ResultID != id &&
                r.EnrolmentID == updatedResult.EnrolmentID);

            if (duplicate)
            {
                return Conflict(new { message = "This enrolment already has a result." });
            }

            if (updatedResult.FinishPosition.HasValue &&
                updatedResult.FinishPosition.Value <= 0)
            {
                return BadRequest(new { message = "FinishPosition must be greater than zero." });
            }

            existingResult.EnrolmentID = updatedResult.EnrolmentID;
            existingResult.FinishTime = updatedResult.FinishTime;
            existingResult.FinishPosition = updatedResult.FinishPosition;

            await _context.SaveChangesAsync();

            return Ok(existingResult);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteResult(int id)
        {
            var result = await _context.Results
                .FirstOrDefaultAsync(r => r.ResultID == id);

            if (result == null)
            {
                return NotFound(new { message = "Result not found." });
            }

            _context.Results.Remove(result);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Result deleted successfully." });
        }
    }
}