using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLHTNK_BE.Models;

[ApiController]
[Route("api/[controller]")]
public class DiscountController : Controller
{
    private readonly DentalCentreManagementContext _context;

    public DiscountController(DentalCentreManagementContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDiscount([FromBody] GiamGia discount)
    {
        if (discount == null)
            return BadRequest(new { Message = "Invalid discount data." });

        try
        {
            _context.GiamGias.Add(discount);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDiscountById), new { id = discount.MaGiamGia }, discount);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while creating the discount.", Error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDiscounts()
    {
        try
        {
            var discounts = await _context.GiamGias.ToListAsync();
            return Ok(discounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching discounts.", Error = ex.Message });
        }
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetDiscountById(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid discount ID." });

        try
        {
            var discount = await _context.GiamGias.FindAsync(id);
            if (discount == null)
                return NotFound(new { Message = "Discount not found." });

            return Ok(discount);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching the discount.", Error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDiscount(int id, [FromBody] GiamGia discount)
    {
        if (id <= 0 || discount == null || id != discount.MaGiamGia)
            return BadRequest(new { Message = "Invalid data provided." });

        try
        {
            _context.Entry(discount).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound(new { Message = "Discount not found for update." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while updating the discount.", Error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDiscount(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid discount ID." });

        try
        {
            var discount = await _context.GiamGias.FindAsync(id);
            if (discount == null)
                return NotFound(new { Message = "Discount not found." });

            _context.GiamGias.Remove(discount);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while deleting the discount.", Error = ex.Message });
        }
    }

    // Get discounts by search criteria
    [HttpGet("search")]
    public async Task<IActionResult> GetDiscountsBySearch(
        [FromQuery] int? maGiamGia = null,
        [FromQuery] string? tenGiamGia = null,
        [FromQuery] decimal? giaDau = null,
        [FromQuery] decimal? giaCuoi = null,
        [FromQuery] string? ngayDau = null,
        [FromQuery] string? ngayCuoi = null,
        [FromQuery] int? maChiNhanh = null)
    {
        try
        {
            // Initialize query with the full collection
            var query = _context.GiamGias.AsQueryable();

            // Apply filters
            if (maGiamGia.HasValue)
            {
                query = query.Where(gd => gd.MaGiamGia == maGiamGia.Value);
            }

            if (!string.IsNullOrEmpty(tenGiamGia))
            {
                query = query.Where(gd => gd.TenGiamGia.Contains(tenGiamGia));
            }

            if (giaDau.HasValue || giaCuoi.HasValue)
            {
                query = query.Where(gd => (!giaDau.HasValue || gd.SoTienGiam >= giaDau) &&
                                            (!giaCuoi.HasValue || gd.SoTienGiam <= giaCuoi));
            }

            if (!string.IsNullOrEmpty(ngayDau) || !string.IsNullOrEmpty(ngayCuoi))
            {
                DateTime? ngayDauDate = !string.IsNullOrEmpty(ngayDau) ? DateTime.Parse(ngayDau) : (DateTime?)null;
                DateTime? ngayCuoiDate = !string.IsNullOrEmpty(ngayCuoi) ? DateTime.Parse(ngayCuoi) : (DateTime?)null;

                query = query.Where(gd => (!ngayDauDate.HasValue || DateTime.Parse(gd.NgayBatDau) >= ngayDauDate) &&
                                            (!ngayCuoiDate.HasValue || DateTime.Parse(gd.NgayKetThuc) <= ngayCuoiDate));
            }

            /*if (maChiNhanh.HasValue)
            {
                query = query.Where(gd => gd.MaChiNhanh == maChiNhanh.Value);
            }*/

            // Sort results by MaGiamGia
            var discounts = await query.OrderBy(gd => gd.MaGiamGia).ToListAsync();

            return Ok(new { Success = true, Discounts = discounts });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Success = false, Message = "An error occurred while fetching discounts.", Error = ex.Message });
        }
    }



}
