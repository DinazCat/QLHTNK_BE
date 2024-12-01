using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLHTNK_BE.Models;

namespace QLHTNK_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BonusController : ControllerBase
    {
        private readonly DentalCentreManagementContext _context;

        public BonusController(DentalCentreManagementContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBonus([FromBody] LuongThuong bonus)
        {
            if (bonus == null)
                return BadRequest(new { Message = "Invalid bonus data." });

            try
            {
                _context.LuongThuongs.Add(bonus);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetBonusById), new { id = bonus.MaLT }, bonus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the bonus.", Error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBonuses()
        {
            try
            {
                var bonuses = await _context.LuongThuongs.Where(e => e.An != true).ToListAsync();
                return Ok(bonuses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching bonuses.", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBonusById(int id)
        {
            if (id < 0)
                return BadRequest(new { Message = "Invalid bonus ID." });

            try
            {
                var bonus = await _context.LuongThuongs.FindAsync(id);
                if (bonus == null)
                    return NotFound(new { Message = "Bonus not found." });

                return Ok(bonus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching the bonus.", Error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBonus(int id, [FromBody] LuongThuong bonus)
        {
            if (id < 0 || bonus == null || id != bonus.MaLT)
                return BadRequest(new { Message = "Invalid data provided." });

            try
            {
                _context.Entry(bonus).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetBonusById), new { id = bonus.MaLT }, bonus);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(new { Message = "Bonus not found for update." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the bonus.", Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBonus(int id)
        {
            if (id < 0)
                return BadRequest(new { Message = "Invalid bonus ID." });

            try
            {
                //var bonus = await _context.LuongThuongs.FindAsync(id);
                //if (bonus == null)
                //    return NotFound(new { Message = "Bonus not found." });

                //_context.LuongThuongs.Remove(bonus);
                //await _context.SaveChangesAsync();
                var bonus = await _context.LuongThuongs.FindAsync(id);
                if (bonus == null)
                    return NotFound(new { Message = "Bonus not found." });
                //xóa mềm
                bonus.An = true;
                _context.Entry(bonus).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Bonus deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the bonus.", Error = ex.Message });
            }
        }
    }
}
