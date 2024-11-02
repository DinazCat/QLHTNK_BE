using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLHTNK_BE.Models;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class FeedbackController : ControllerBase
{
    private readonly DentalCentreManagementContext _context;

    public FeedbackController(DentalCentreManagementContext context)
    {
        _context = context;
    }

    // Create a new feedback
    [HttpPost]
    public async Task<IActionResult> CreateFeedback([FromBody] PhanHoi feedback)
    {
        if (feedback == null)
            return BadRequest(new { Message = "Invalid feedback data." });

        try
        {
            _context.PhanHois.Add(feedback);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllFeedbacks), feedback);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while creating the feedback.", Error = ex.Message });
        }
    }

    // Read all feedbacks
    [HttpGet]
    public async Task<IActionResult> GetAllFeedbacks()
    {
        try
        {
            var feedbacks = await _context.PhanHois.ToListAsync();
            return Ok(feedbacks);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching feedbacks.", Error = ex.Message });
        }
    }

    // Delete a feedback
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFeedback(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid feedback ID." });

        try
        {
            var feedback = await _context.PhanHois.FindAsync(id);
            if (feedback == null)
                return NotFound(new { Message = "Feedback not found." });

            _context.PhanHois.Remove(feedback);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while deleting the feedback.", Error = ex.Message });
        }
    }
}
