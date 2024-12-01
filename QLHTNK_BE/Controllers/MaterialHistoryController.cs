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
                _context.VatTuDaSuDungs.Add(item);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetItemById), new { id = item.MaVatTu }, item);
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
            if (id < 0 || item == null || id != item.MaVatTu)
                return BadRequest(new { Message = "Invalid data provided." });

            try
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetItemById), new { id = item.MaVatTu }, item);
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
                //xóa mềm
                item.An = true;
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Item deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the item.", Error = ex.Message });
            }
        }
    }
}
