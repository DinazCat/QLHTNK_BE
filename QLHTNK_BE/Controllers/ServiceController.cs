using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLHTNK_BE.Models;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ServiceController : ControllerBase
{
    private readonly DentalCentreManagementContext _context;

    public ServiceController(DentalCentreManagementContext context)
    {
        _context = context;
    }

    // Create a new service
    [HttpPost]
    public async Task<IActionResult> CreateService([FromBody] DichVu service)
    {
        if (service == null)
            return BadRequest(new { Message = "Invalid service data." });

        try
        {
            _context.DichVus.Add(service);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetServiceById), new { id = service.MaDv }, service);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while creating the service.", Error = ex.Message });
        }
    }

    // Read all services
    [HttpGet]
    public async Task<IActionResult> GetAllServices()
    {
        try
        {
            var services = await _context.DichVus.ToListAsync();
            return Ok(services);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching services.", Error = ex.Message });
        }
    }

    // Read a specific service by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetServiceById(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid service ID." });

        try
        {
            var service = await _context.DichVus.FindAsync(id);
            if (service == null)
                return NotFound(new { Message = "Service not found." });

            return Ok(service);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching the service.", Error = ex.Message });
        }
    }

    // Update a service
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateService(int id, [FromBody] DichVu service)
    {
        if (id <= 0 || service == null || id != service.MaDv)
            return BadRequest(new { Message = "Invalid data provided." });

        try
        {
            _context.Entry(service).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound(new { Message = "Service not found for update." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while updating the service.", Error = ex.Message });
        }
    }

    // Delete a service
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteService(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid service ID." });

        try
        {
            var service = await _context.DichVus.FindAsync(id);
            if (service == null)
                return NotFound(new { Message = "Service not found." });

            _context.DichVus.Remove(service);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while deleting the service.", Error = ex.Message });
        }
    }
}
