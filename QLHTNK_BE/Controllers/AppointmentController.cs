using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLHTNK_BE.Models;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly DentalCentreManagementContext _context;

    public AppointmentController(DentalCentreManagementContext context)
    {
        _context = context;
    }

    // Create a new appointment
    [HttpPost]
    public async Task<IActionResult> CreateAppointment([FromBody] LichHen appointment)
    {
        if (appointment == null)
            return BadRequest(new { Message = "Invalid appointment data." });

        try
        {
            _context.LichHens.Add(appointment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.MaLichHen }, appointment);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while creating the appointment.", Error = ex.Message });
        }
    }

    // Read all appointments
    [HttpGet]
    public async Task<IActionResult> GetAllAppointments()
    {
        try
        {
            var appointments = await _context.LichHens.ToListAsync();
            return Ok(appointments);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching appointments.", Error = ex.Message });
        }
    }

    // Read a specific appointment by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAppointmentById(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid appointment ID." });

        try
        {
            var appointment = await _context.LichHens.FindAsync(id);
            if (appointment == null)
                return NotFound(new { Message = "Appointment not found." });

            return Ok(appointment);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching the appointment.", Error = ex.Message });
        }
    }

    // Update an appointment
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAppointment(int id, [FromBody] LichHen appointment)
    {
        if (id <= 0 || appointment == null || id != appointment.MaLichHen)
            return BadRequest(new { Message = "Invalid data provided." });

        try
        {
            _context.Entry(appointment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound(new { Message = "Appointment not found for update." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while updating the appointment.", Error = ex.Message });
        }
    }

    // Delete an appointment
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid appointment ID." });

        try
        {
            var appointment = await _context.LichHens.FindAsync(id);
            if (appointment == null)
                return NotFound(new { Message = "Appointment not found." });

            _context.LichHens.Remove(appointment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while deleting the appointment.", Error = ex.Message });
        }
    }
}
