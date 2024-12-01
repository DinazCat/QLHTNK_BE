using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using QLHTNK_BE.Models;

[ApiController]
[Route("api/[controller]")]
public class BillController : ControllerBase
{
    private readonly DentalCentreManagementContext _context;

    public BillController(DentalCentreManagementContext context)
    {
        _context = context;
    }

    // Create a new bill
    [HttpPost]
    public async Task<IActionResult> CreateBill([FromBody] HoaDon bill)
    {
        if (bill == null)
            return BadRequest(new { Message = "Invalid bill data." });

        try
        {
            _context.HoaDons.Add(bill);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBillById), new { id = bill.MaHd }, bill);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while creating the bill.", Error = ex.Message });
        }
    }

    // Read all bills
    [HttpGet]
    public async Task<IActionResult> GetAllBills()
    {
        try
        {
            var bills = await _context.HoaDons.ToListAsync();
            return Ok(bills);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching bills.", Error = ex.Message });
        }
    }

    // Read a specific bill by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBillById(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid bill ID." });

        try
        {
            var bill = await _context.HoaDons.FindAsync(id);
            if (bill == null)
                return NotFound(new { Message = "Bill not found." });

            return Ok(bill);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching the bill.", Error = ex.Message });
        }
    }

    // Update an existing bill
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBill(int id, [FromBody] HoaDon bill)
    {
        if (id <= 0 || bill == null || id != bill.MaHd)
            return BadRequest(new { Message = "Invalid data provided." });

        try
        {
            _context.Entry(bill).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound(new { Message = "Bill not found for update." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while updating the bill.", Error = ex.Message });
        }
    }

    // Delete a bill
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBill(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid bill ID." });

        try
        {
            var bill = await _context.HoaDons.FindAsync(id);
            if (bill == null)
                return NotFound(new { Message = "Bill not found." });

            _context.HoaDons.Remove(bill);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while deleting the bill.", Error = ex.Message });
        }
    }

    // Get bills by search criteria
    [HttpGet("search")]
    public async Task<IActionResult> GetBillsBySearch(
        [FromQuery] int? maHoaDon = null,
        [FromQuery] int? maBenhNhan = null,
        [FromQuery] string? tenBenhNhan = "",
        [FromQuery] string? ngayLap = "",
        [FromQuery] string? tinhTrang = "Tất cả")
    {
        try
        {
            // Begin with the full collection of bills
            IQueryable<HoaDon> query = _context.HoaDons.Include(b => b.MaBnNavigation);

            // Filter by MaHoaDon
            if (maHoaDon.HasValue)
            {
                query = query.Where(bill => bill.MaHd == maHoaDon.Value);
            }

            // Filter by MaBenhNhan
            if (maBenhNhan.HasValue)
            {
                query = query.Where(bill => bill.MaBn == maBenhNhan.Value);
            }

            // Filter by TenBenhNhan if provided
            if (!string.IsNullOrWhiteSpace(tenBenhNhan))
            {
                query = query.Where(bill => bill.MaBnNavigation.TenBn.Contains(tenBenhNhan));
            }

            // Filter by NgayLap if provided
            if (!string.IsNullOrWhiteSpace(ngayLap))
            {
                query = query.Where(bill =>
                    string.Equals(bill.NgayLap.Trim(), ngayLap.Trim(), StringComparison.OrdinalIgnoreCase)
                );
            }                 

            // Filter by TinhTrang if not "Tất cả"
            if (!string.Equals(tinhTrang, "Tất cả", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(bill => bill.TinhTrang == tinhTrang);
            }

            // Order by MaHoaDon
            var bills = await query.OrderBy(bill => bill.MaHd).ToListAsync();

            return Ok(new { Success = true, Bills = bills });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Success = false, Message = "An error occurred while searching for bills.", Error = ex.Message });
        }
    }
}
