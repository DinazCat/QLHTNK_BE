using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLHTNK_BE.Models;
using System;
using System.Xml.Linq;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly DentalCentreManagementContext _context;

    public PatientController(DentalCentreManagementContext context)
    {
        _context = context;
    }

    // Create a new patient
    [HttpPost]
    public async Task<IActionResult> CreatePatient([FromBody] BenhNhan patient)
    {
        if (patient == null)
            return BadRequest(new { Message = "Invalid patient data." });

        try
        {
            _context.BenhNhans.Add(patient);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPatientById), new { id = patient.MaBn }, patient);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while creating the patient.", Error = ex.Message });
        }
    }

    // Read all patients
    [HttpGet]
    public async Task<IActionResult> GetAllPatients()
    {
        try
        {
            var patients = await _context.BenhNhans.ToListAsync();
            return Ok(patients);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching patients.", Error = ex.Message });
        }
    }

    // Read a specific patient by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientById(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid patient ID." });

        try
        {
            var patient = await _context.BenhNhans.FindAsync(id);
            if (patient == null)
                return NotFound(new { Message = "Patient not found." });

            return Ok(patient);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching the patient.", Error = ex.Message });
        }
    }

    // Update a patient
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePatient(int id, [FromBody] BenhNhan patient)
    {
        if (id <= 0 || patient == null || id != patient.MaBn)
            return BadRequest(new { Message = "Invalid data provided." });

        try
        {
            _context.Entry(patient).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound(new { Message = "Patient not found for update." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while updating the patient.", Error = ex.Message });
        }
    }

    // Delete a patient
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid patient ID." });

        try
        {
            var patient = await _context.BenhNhans.FindAsync(id);
            if (patient == null)
                return NotFound(new { Message = "Patient not found." });

            _context.BenhNhans.Remove(patient);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while deleting the patient.", Error = ex.Message });
        }
    }
}
