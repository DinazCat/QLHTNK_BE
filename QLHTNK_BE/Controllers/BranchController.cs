using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLHTNK_BE.Models;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class BranchController : ControllerBase
{
    private readonly DentalCentreManagementContext _context;

    public BranchController(DentalCentreManagementContext context)
    {
        _context = context;
    }

    // Create a new branch
    [HttpPost]
    public async Task<IActionResult> CreateBranch([FromBody] ChiNhanh branch)
    {
        if (branch == null)
            return BadRequest(new { Message = "Invalid branch data." });

        try
        {
            _context.ChiNhanhs.Add(branch);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBranchById), new { id = branch.MaCn }, branch);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while creating the branch.", Error = ex.Message });
        }
    }

    // Read all branches
    [HttpGet]
    public async Task<IActionResult> GetAllBranches()
    {
        try
        {
            var branches = await _context.ChiNhanhs.ToListAsync();
            return Ok(branches);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching branches.", Error = ex.Message });
        }
    }

    // Read a specific branch by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBranchById(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid branch ID." });

        try
        {
            var branch = await _context.ChiNhanhs.FindAsync(id);
            if (branch == null)
                return NotFound(new { Message = "Branch not found." });

            return Ok(branch);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching the branch.", Error = ex.Message });
        }
    }

    // Update a branch
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBranch(int id, [FromBody] ChiNhanh branch)
    {
        if (id <= 0 || branch == null || id != branch.MaCn)
            return BadRequest(new { Message = "Invalid data provided." });

        try
        {
            _context.Entry(branch).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound(new { Message = "Branch not found for update." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while updating the branch.", Error = ex.Message });
        }
    }

    // Delete a branch
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBranch(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid branch ID." });

        try
        {
            var branch = await _context.ChiNhanhs.FindAsync(id);
            if (branch == null)
                return NotFound(new { Message = "Branch not found." });

            _context.ChiNhanhs.Remove(branch);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while deleting the branch.", Error = ex.Message });
        }
    }
}
