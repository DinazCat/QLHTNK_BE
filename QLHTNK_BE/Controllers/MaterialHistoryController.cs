using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLHTNK_BE.Models;

namespace QLHTNK_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialHistoryController : ControllerBase
    {
        private readonly DentalCentreManagementContext _context;

        public MaterialHistoryController(DentalCentreManagementContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VatTuDaSuDung item)
        {
            if (item == null)
                return BadRequest(new { Message = "Invalid useditem data." });

            try
            {
                var vattu = await _context.VatTus.FirstOrDefaultAsync(x => x.MaVt == item.MaVatTu);
                if (vattu == null)
                {
                    return NotFound(new { Message = "Vật tư không tồn tại." });
                }

                if (vattu.SoLuongTonKho < item.SoLuong)
                {
                    return BadRequest(new { Message = "Số lượng trong kho không đủ." });
                }

                vattu.SoLuongTonKho -= (int)item.SoLuong;
                _context.VatTuDaSuDungs.Add(item);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetItemById), new { id = item.MaVTDSD }, item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the used material.", Error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var item = await _context.VatTuDaSuDungs.Where(e => e.An != true).ToListAsync();
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching used item.", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            if (id < 0)
                return BadRequest(new { Message = "Invalid item ID." });

            try
            {
                var item = await _context.VatTuDaSuDungs.FindAsync(id);
                if (item == null)
                    return NotFound(new { Message = "Item not found." });

                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching the item.", Error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VatTuDaSuDung item)
        {
            if (id < 0 || item == null || id != item.MaVTDSD)
                return BadRequest(new { Message = "Invalid data provided." });

            try
            {
                var vattu = await _context.VatTus.FirstOrDefaultAsync(x => x.MaVt == item.MaVatTu);
                var itemInDB = await _context.VatTuDaSuDungs.FindAsync(id);
                if (vattu == null || itemInDB == null)
                {
                    return NotFound(new { Message = "Vật tư không tồn tại." });
                }
                if (itemInDB.MaVatTu == item.MaVatTu)
                {
                    var tonkho = vattu.SoLuongTonKho + itemInDB.SoLuong;
                    if (tonkho < item.SoLuong)
                    {
                        return BadRequest(new { Message = "Số lượng trong kho không đủ." });
                    }

                    vattu.SoLuongTonKho = tonkho - item.SoLuong;
                }
                else
                {
                    if (vattu.SoLuongTonKho < item.SoLuong)
                    {
                        return BadRequest(new { Message = "Số lượng trong kho không đủ." });
                    }
                    vattu.SoLuongTonKho -= item.SoLuong;
                }
                _context.Entry(itemInDB).State = EntityState.Detached; // Tách rời itemInDB
                _context.Entry(item).State = EntityState.Modified;     // Theo dõi item mới

                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetItemById), new { id = item.MaVTDSD }, item);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(new { Message = "Item not found for update." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the item.", Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 0)
                return BadRequest(new { Message = "Invalid item ID." });

            try
            {
                var item = await _context.VatTuDaSuDungs.FindAsync(id);

                if (item == null)
                    return NotFound(new { Message = "Item not found." });

                var vattu = await _context.VatTus.FirstOrDefaultAsync(x => x.MaVt == item.MaVatTu);

                if (vattu == null) return NotFound(new { Message = "Material not found." });

                vattu.SoLuongTonKho = vattu.SoLuongTonKho + item.SoLuong;

                _context.VatTuDaSuDungs.Remove(item);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Item deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the item.", Error = ex.Message });
            }
        }

        //maVatTu: "",
        //tenVatTu: "",
        //NgaySuDung: "",
        [HttpGet("search")]
        public async Task<IActionResult> GetItemsBySearch(
            [FromQuery] int? maVatTu = null,
            [FromQuery] string? tenVatTu = "",
            [FromQuery] string? NgaySuDung = "")
        {
            try
            {
                // Bắt đầu với toàn bộ dữ liệu
                IQueryable<VatTuDaSuDung> query = _context.VatTuDaSuDungs;


                if (maVatTu.HasValue)
                {
                    query = query.Where(nv => nv.MaVatTu == maVatTu.Value);
                }


                if (!string.IsNullOrWhiteSpace(tenVatTu))
                {
                    var vattu = await _context.VatTus.FirstOrDefaultAsync(x => x.TenVt == tenVatTu);
                    if (vattu != null)
                    {
                        query = query.Where(nv => nv.MaVatTu == vattu.MaVt);
                    }
                }

                if (!string.IsNullOrWhiteSpace(NgaySuDung))
                {
                    query = query.Where(nv => nv.NgaySuDung == NgaySuDung);
                }
                // Sắp xếp theo MaNv
                var searchResults = await query.OrderBy(nv => nv.MaVTDSD).Where(e => e.An != true).ToListAsync();

                return Ok(new { success = true, items = searchResults });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while searching for used its.", Error = ex.Message });
            }
        }
    }
}
