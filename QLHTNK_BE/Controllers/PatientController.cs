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

    // Get patients by search criteria
    [HttpGet("search")]
    public async Task<IActionResult> GetPatientsBySearch(
    [FromQuery] int? maBn = null,
    [FromQuery] string? tenBn = null,
    [FromQuery] string? soDienThoai = null,
    [FromQuery] string? cccd = null)
    {
        try
        {
            // Start with the full set of patients
            var query = _context.BenhNhans.AsQueryable();

            // Apply filters based on provided criteria
            if (maBn.HasValue)
            {
                query = query.Where(bn => bn.MaBn == maBn.Value);
            }

            if (!string.IsNullOrEmpty(tenBn))
            {
                query = query.Where(bn => bn.TenBn.Contains(tenBn));
            }

            if (!string.IsNullOrEmpty(soDienThoai))
            {
                query = query.Where(bn => bn.SoDienThoai.Contains(soDienThoai));
            }

            if (!string.IsNullOrEmpty(cccd))
            {
                query = query.Where(bn => bn.Cccd.Contains(cccd));
            }

            // Execute the query and sort by MaBn
            var patients = await query.OrderBy(bn => bn.MaBn).ToListAsync();

            return Ok(new { Success = true, Patients = patients });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Success = false, Message = "An error occurred while fetching patients.", Error = ex.Message });
        }
    }

    // Add a new treatment record (ChiTietHsdt)
    [HttpPost("add-treatment-detail")]
    public async Task<IActionResult> AddTreatmentDetail([FromBody] ChiTietHsdt treatmentDetail)
    {
        if (treatmentDetail == null)
            return BadRequest(new { Message = "Invalid treatment detail data." });

        try
        {
            // Check if patient exists
            var patient = await _context.BenhNhans.FindAsync(treatmentDetail.MaBn);
            if (patient == null)
                return NotFound(new { Message = "Patient not found." });

            _context.ChiTietHsdts.Add(treatmentDetail);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTreatmentDetailById), new { id = treatmentDetail.MaCthsdt }, treatmentDetail);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while adding the treatment detail.", Error = ex.Message });
        }
    }

    // Get treatment detail by ID
    [HttpGet("treatment-detail/{id}")]
    public async Task<IActionResult> GetTreatmentDetailById(int id)
    {
        try
        {
            var treatmentDetail = await _context.ChiTietHsdts.FindAsync(id);
            if (treatmentDetail == null)
                return NotFound(new { Message = "Treatment detail not found." });

            return Ok(treatmentDetail);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching the treatment detail.", Error = ex.Message });
        }
    }

    // Update treatment record (ChiTietHsdt)
    [HttpPut("update-treatment-detail/{id}")]
    public async Task<IActionResult> UpdateTreatmentDetail(int id, [FromBody] ChiTietHsdt treatmentDetail)
    {
        if (id <= 0 || treatmentDetail == null || id != treatmentDetail.MaCthsdt)
            return BadRequest(new { Message = "Invalid data provided." });

        try
        {
            // Check if the treatment record exists
            var existingTreatmentDetail = await _context.ChiTietHsdts.FindAsync(id);
            if (existingTreatmentDetail == null)
                return NotFound(new { Message = "Treatment detail not found." });

            _context.Entry(existingTreatmentDetail).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while updating the treatment detail.", Error = ex.Message });
        }
    }

    // Delete a treatment record (ChiTietHsdt)
    [HttpDelete("delete-treatment-detail/{id}")]
    public async Task<IActionResult> DeleteTreatmentDetail(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid treatment detail ID." });

        try
        {
            var treatmentDetail = await _context.ChiTietHsdts.FindAsync(id);
            if (treatmentDetail == null)
                return NotFound(new { Message = "Treatment detail not found." });

            _context.ChiTietHsdts.Remove(treatmentDetail);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while deleting the treatment detail.", Error = ex.Message });
        }
    }

    // Get all treatment details for a patient (ChiTietHsdt by MaBn)
    [HttpGet("{patientId}/treatment-details")]
    public async Task<IActionResult> GetTreatmentDetailsByPatientId(int patientId)
    {
        try
        {
            // Fetch treatment details for a specific patient
            var treatmentDetails = await _context.ChiTietHsdts
                                                 .Where(t => t.MaBn == patientId)
                                                 .ToListAsync();

            if (treatmentDetails == null || treatmentDetails.Count == 0)
                return NotFound(new { Message = "No treatment details found for this patient." });

            return Ok(treatmentDetails);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching the treatment details.", Error = ex.Message });
        }
    }


}
