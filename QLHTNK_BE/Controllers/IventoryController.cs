using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLHTNK_BE.Models;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly DentalCentreManagementContext _context;

    public InventoryController(DentalCentreManagementContext context)
    {
        _context = context;
    }

    // CRUD operations for Thuoc (Medicine)

    // Create a new medicine
    [HttpPost("medicine")]
    public async Task<IActionResult> CreateMedicine([FromBody] Thuoc medicine)
    {
        if (medicine == null)
            return BadRequest(new { Message = "Invalid medicine data." });

        try
        {
            _context.Thuocs.Add(medicine);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMedicineById), new { id = medicine.MaThuoc }, medicine);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while creating the medicine.", Error = ex.Message });
        }
    }

    // Read all medicines
    [HttpGet("medicine")]
    public async Task<IActionResult> GetAllMedicines()
    {
        try
        {
            var medicines = await _context.Thuocs.ToListAsync();
            return Ok(medicines);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching medicines.", Error = ex.Message });
        }
    }

    // Read a specific medicine by ID
    [HttpGet("medicine/{id}")]
    public async Task<IActionResult> GetMedicineById(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid medicine ID." });

        try
        {
            var medicine = await _context.Thuocs.FindAsync(id);
            if (medicine == null)
                return NotFound(new { Message = "Medicine not found." });

            return Ok(medicine);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching the medicine.", Error = ex.Message });
        }
    }

    // Update a medicine
    [HttpPut("medicine/{id}")]
    public async Task<IActionResult> UpdateMedicine(int id, [FromBody] Thuoc medicine)
    {
        if (id <= 0 || medicine == null || id != medicine.MaThuoc)
            return BadRequest(new { Message = "Invalid data provided." });

        try
        {
            _context.Entry(medicine).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound(new { Message = "Medicine not found for update." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while updating the medicine.", Error = ex.Message });
        }
    }

    // Delete a medicine
    [HttpDelete("medicine/{id}")]
    public async Task<IActionResult> DeleteMedicine(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid medicine ID." });

        try
        {
            var medicine = await _context.Thuocs.FindAsync(id);
            if (medicine == null)
                return NotFound(new { Message = "Medicine not found." });

            _context.Thuocs.Remove(medicine);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while deleting the medicine.", Error = ex.Message });
        }
    }

    // CRUD operations for VatTu (Supplies)

    // Create a new supply item
    [HttpPost("supply")]
    public async Task<IActionResult> CreateSupply([FromBody] VatTu supply)
    {
        if (supply == null)
            return BadRequest(new { Message = "Invalid supply data." });

        try
        {
            _context.VatTus.Add(supply);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSupplyById), new { id = supply.MaVt }, supply);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while creating the supply.", Error = ex.Message });
        }
    }

    // Read all supplies
    [HttpGet("supply")]
    public async Task<IActionResult> GetAllSupplies()
    {
        try
        {
            var supplies = await _context.VatTus.ToListAsync();
            return Ok(supplies);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching supplies.", Error = ex.Message });
        }
    }

    // Read a specific supply by ID
    [HttpGet("supply/{id}")]
    public async Task<IActionResult> GetSupplyById(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid supply ID." });

        try
        {
            var supply = await _context.VatTus.FindAsync(id);
            if (supply == null)
                return NotFound(new { Message = "Supply not found." });

            return Ok(supply);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching the supply.", Error = ex.Message });
        }
    }

    // Update a supply
    [HttpPut("supply/{id}")]
    public async Task<IActionResult> UpdateSupply(int id, [FromBody] VatTu supply)
    {
        if (id <= 0 || supply == null || id != supply.MaVt)
            return BadRequest(new { Message = "Invalid data provided." });

        try
        {
            _context.Entry(supply).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound(new { Message = "Supply not found for update." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while updating the supply.", Error = ex.Message });
        }
    }

    // Delete a supply
    [HttpDelete("supply/{id}")]
    public async Task<IActionResult> DeleteSupply(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid supply ID." });

        try
        {
            var supply = await _context.VatTus.FindAsync(id);
            if (supply == null)
                return NotFound(new { Message = "Supply not found." });

            _context.VatTus.Remove(supply);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while deleting the supply.", Error = ex.Message });
        }
    }
}
