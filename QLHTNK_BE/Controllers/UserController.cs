using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLHTNK_BE.Models;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly DentalCentreManagementContext _context;

    public UserController(DentalCentreManagementContext context)
    {
        _context = context;
    }

    // Create a new user
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] TaiKhoan user)
    {
        if (user == null)
            return BadRequest(new { Message = "Invalid user data." });

        try
        {
            _context.TaiKhoans.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserById), new { id = user.MaTk }, user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while creating the user.", Error = ex.Message });
        }
    }

    // Read all users
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _context.TaiKhoans.ToListAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching users.", Error = ex.Message });
        }
    }

    // Read a specific user by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid user ID." });

        try
        {
            var user = await _context.TaiKhoans.FindAsync(id);
            if (user == null)
                return NotFound(new { Message = "User not found." });

            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching the user.", Error = ex.Message });
        }
    }

    // Update a user
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] TaiKhoan user)
    {
        if (id <= 0 || user == null || id != user.MaTk)
            return BadRequest(new { Message = "Invalid data provided." });

        try
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound(new { Message = "User not found for update." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while updating the user.", Error = ex.Message });
        }
    }

    // Delete a user
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid user ID." });

        try
        {
            var user = await _context.TaiKhoans.FindAsync(id);
            if (user == null)
                return NotFound(new { Message = "User not found." });

            _context.TaiKhoans.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while deleting the user.", Error = ex.Message });
        }
    }
}
