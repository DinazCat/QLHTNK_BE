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
            var appointments = await _context.LichHens
            .ToListAsync();

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

    // Get appointments by search criteria
    [HttpGet("search")]
    public async Task<IActionResult> GetAppointmentsBySearch(
    [FromQuery] string? MaNS,
    [FromQuery] string? MaBN,
    [FromQuery] string? TenNS,
    [FromQuery] string? TenBN,
    [FromQuery] string? DichVu,
    [FromQuery] string? NgayHen,
    [FromQuery] string? LoaiLichHen,
    [FromQuery] string? TrangThai)
    {
        try
        {
            // Fetch all appointments
            var appointmentsQuery = _context.LichHens.AsQueryable();

            // Filter by MaNS (staff ID)
            if (!string.IsNullOrEmpty(MaNS))
            {
                appointmentsQuery = appointmentsQuery.Where(a => a.MaNs.ToString().Contains(MaNS));
            }

            // Filter by TenNS (staff name)
            if (!string.IsNullOrEmpty(TenNS))
            {
                appointmentsQuery = appointmentsQuery.Where(a => a.MaNsNavigation.TenNv.ToLower().Contains(TenNS.ToLower()));
            }

            // Filter by MaBN
            if (!string.IsNullOrEmpty(MaBN))
            {
                appointmentsQuery = appointmentsQuery.Where(a => a.MaBn.ToString().Contains(MaBN));
            }

            // Filter by TenBN (patient name)
            if (!string.IsNullOrEmpty(TenBN))
            {
                appointmentsQuery = appointmentsQuery.Where(a => a.HoTen.Contains(TenBN.ToLower()));
            }

            // Filter by DichVu (service)
            if (!string.IsNullOrEmpty(DichVu))
            {
                appointmentsQuery = appointmentsQuery.Where(a => a.LyDoKham.ToLower().Contains(DichVu.ToLower()));
            }

            // Filter by NgayHen (appointment date)
            if (!string.IsNullOrEmpty(NgayHen))
            {
                appointmentsQuery = appointmentsQuery.Where(a => a.Ngay == NgayHen);
            }

            // Filter by LoaiLichHen (appointment type)
            if (!string.IsNullOrEmpty(LoaiLichHen))
            {
                appointmentsQuery = appointmentsQuery.Where(a => a.LoaiLichHen == LoaiLichHen);
            }

            // Filter by TrangThai (status)
            if (!string.IsNullOrEmpty(TrangThai))
            {
                appointmentsQuery = appointmentsQuery.Where(a => a.TrangThai == TrangThai);
            }

            // Execute the query
            var appointments = await appointmentsQuery.ToListAsync();

            // Sort the appointments based on appointment date and time (descending)
            var sortedAppointments = appointments.OrderByDescending(a => a.Ngay).ThenByDescending(a => a.Gio).ToList();

            // Return the filtered and sorted result
            return Ok(sortedAppointments);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching appointments.", Error = ex.Message });
        }
    }


[HttpPost("DoctorSchedule")]
    public async Task<IActionResult> CreateDoctorSchedule([FromBody] LichLamViec doctorSchedule)
    {
        if (doctorSchedule == null)
            return BadRequest(new { Message = "Invalid data." });

        try
        {
            _context.LichLamViecs.Add(doctorSchedule);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAppointmentById), new { id = doctorSchedule.MaLichLamViec }, doctorSchedule);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while creating the LichLamViec.", Error = ex.Message });
        }
    }

    [HttpGet("DoctorSchedule")]
    public async Task<IActionResult> GetAllDoctorSchedule()
    {
        try
        {
            var appointments = await _context.LichLamViecs
            .ToListAsync();

            return Ok(appointments);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching LichLamViecs.", Error = ex.Message });
        }
    }

    [HttpPut("DoctorSchedule/{id}")]
    public async Task<IActionResult> UpdateDoctorSchedule(int id, [FromBody] LichLamViec doctorSchedule)
    {
        if (id <= 0 || doctorSchedule == null || id != doctorSchedule.MaLichLamViec)
            return BadRequest(new { Message = "Invalid data provided." });

        try
        {
            _context.Entry(doctorSchedule).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound(new { Message = "DoctorSchedule not found for update." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while updating the DoctorSchedule.", Error = ex.Message });
        }
    }

}
