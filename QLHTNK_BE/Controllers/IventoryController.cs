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

    // Get medicines by search criteria
    [HttpGet("medicine/search")]
    public async Task<IActionResult> GetMedicinesBySearch(
    string? maThuoc = null,
    string? tenThuoc = null,
    int? slnDau = null,
    int? slnCuoi = null,
    int? sltkDau = null,
    int? sltkCuoi = null,
    decimal? giaNhapDau = null,
    decimal? giaNhapCuoi = null,
    decimal? giaDau = null,
    decimal? giaCuoi = null,
    string? hsdDau = null,
    string? hsdCuoi = null,
    string? ngayDau = null,
    string? ngayCuoi = null,
    int? chiNhanh = null)
    {
        try
        {
            var query = _context.Thuocs.AsQueryable();

            if (!string.IsNullOrEmpty(maThuoc))
                query = query.Where(t => t.MaThuoc.ToString().Contains(maThuoc));

            if (!string.IsNullOrEmpty(tenThuoc))
                query = query.Where(t => t.TenThuoc != null && t.TenThuoc.Contains(tenThuoc, StringComparison.OrdinalIgnoreCase));

            if (slnDau.HasValue)
                query = query.Where(t => t.SoLuongNhap >= slnDau.Value);
            if (slnCuoi.HasValue)
                query = query.Where(t => t.SoLuongNhap <= slnCuoi.Value);

            if (sltkDau.HasValue)
                query = query.Where(t => t.SoLuongTonKho >= sltkDau.Value);
            if (sltkCuoi.HasValue)
                query = query.Where(t => t.SoLuongTonKho <= sltkCuoi.Value);

            if (giaNhapDau.HasValue)
                query = query.Where(t => t.DonGiaNhap >= giaNhapDau.Value);
            if (giaNhapCuoi.HasValue)
                query = query.Where(t => t.DonGiaNhap <= giaNhapCuoi.Value);

            if (giaDau.HasValue)
                query = query.Where(t => t.DonGiaBan >= giaDau.Value);
            if (giaCuoi.HasValue)
                query = query.Where(t => t.DonGiaBan <= giaCuoi.Value);

            if (chiNhanh.HasValue)
                query = query.Where(t => t.MaChiNhanh == chiNhanh.Value);

            if (DateTime.TryParse(hsdDau, out var parsedHsdDau))
                query = query.Where(t => DateTime.Parse(t.HanSuDung) >= parsedHsdDau);

            if (DateTime.TryParse(hsdCuoi, out var parsedHsdCuoi))
                query = query.Where(t => DateTime.Parse(t.HanSuDung) <= parsedHsdCuoi);

            if (DateTime.TryParse(ngayDau, out var parsedNgayDau))
                query = query.Where(t => DateTime.Parse(t.NgayNhap) >= parsedNgayDau);

            if (DateTime.TryParse(ngayCuoi, out var parsedNgayCuoi))
                query = query.Where(t => DateTime.Parse(t.NgayNhap) <= parsedNgayCuoi);


            var medicines = await query.ToListAsync();
            return Ok(medicines);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching medicines.", Error = ex.Message });
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

    // Get supplies by search criteria
    [HttpGet("supply/search")]
    public async Task<IActionResult> GetSuppliesBySearch(
    [FromQuery] int? maVt = null,
    [FromQuery] string? tenVt = null,
    [FromQuery] int? slnDau = null,
    [FromQuery] int? slnCuoi = null,
    [FromQuery] int? sltkDau = null,
    [FromQuery] int? sltkCuoi = null,
    [FromQuery] decimal? giaDau = null,
    [FromQuery] decimal? giaCuoi = null,
    [FromQuery] string? ngayDau = null,
    [FromQuery] string? ngayCuoi = null,
    [FromQuery] int? maChiNhanh = null)
    {
        try
        {
            // Initialize query with the full collection
            var query = _context.VatTus.AsQueryable();

            // Apply filters
            if (maVt.HasValue)
            {
                query = query.Where(vt => vt.MaVt == maVt.Value);
            }

            if (!string.IsNullOrEmpty(tenVt))
            {
                query = query.Where(vt => vt.TenVt.Contains(tenVt));
            }

            if (slnDau.HasValue || slnCuoi.HasValue)
            {
                query = query.Where(vt => (!slnDau.HasValue || vt.SoLuongNhap >= slnDau) &&
                                          (!slnCuoi.HasValue || vt.SoLuongNhap <= slnCuoi));
            }

            if (sltkDau.HasValue || sltkCuoi.HasValue)
            {
                query = query.Where(vt => (!sltkDau.HasValue || vt.SoLuongTonKho >= sltkDau) &&
                                          (!sltkCuoi.HasValue || vt.SoLuongTonKho <= sltkCuoi));
            }

            if (giaDau.HasValue || giaCuoi.HasValue)
            {
                query = query.Where(vt => (!giaDau.HasValue || vt.DonGiaNhap >= giaDau) &&
                                          (!giaCuoi.HasValue || vt.DonGiaNhap <= giaCuoi));
            }

            if (!string.IsNullOrEmpty(ngayDau) || !string.IsNullOrEmpty(ngayCuoi))
            {
                DateTime? ngayDauDate = !string.IsNullOrEmpty(ngayDau) ? DateTime.Parse(ngayDau) : (DateTime?)null;
                DateTime? ngayCuoiDate = !string.IsNullOrEmpty(ngayCuoi) ? DateTime.Parse(ngayCuoi) : (DateTime?)null;

                query = query.Where(vt => (!ngayDauDate.HasValue || DateTime.Parse(vt.NgayNhap) >= ngayDauDate) &&
                                          (!ngayCuoiDate.HasValue || DateTime.Parse(vt.NgayNhap) <= ngayCuoiDate));
            }

            if (maChiNhanh.HasValue)
            {
                query = query.Where(vt => vt.MaChiNhanh == maChiNhanh.Value);
            }

            // Sort results by MaVt
            var materials = await query.OrderBy(vt => vt.MaVt).ToListAsync();

            return Ok(new { Success = true, Materials = materials });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Success = false, Message = "An error occurred while fetching materials.", Error = ex.Message });
        }
    }
}
