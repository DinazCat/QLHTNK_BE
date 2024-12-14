using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QLHTNK_BE.Models;
using System;
using System.Security.Principal;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly DentalCentreManagementContext _context;

    public EmployeeController(DentalCentreManagementContext context)
    {
        _context = context;
    }

    // Create a new employee
    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] NhanVien employee)
    {
        if (employee == null)
            return BadRequest(new { Message = "Invalid employee data." });

        try
        {
            _context.NhanViens.Add(employee);
            await _context.SaveChangesAsync();
            if (employee?.MaNv != null)
            {
                DateTime birthDate = DateTime.Parse(employee?.NgaySinh);

                // Tính năm sinh
                int birthYear = birthDate.Year;

                // Lấy năm hiện tại
                int currentYear = DateTime.Now.Year;

                // Tính tuổi
                int age = currentYear - birthYear;

                TaiKhoan acc = new()
                {
                    MaNguoiDung = employee?.Cccd,
                    Ten = employee?.TenNv,
                    SoDienThoai = employee?.SoDienThoai,
                    Tuoi = age,
                    Email = employee?.Email,
                    MatKhau = "123456",
                    LoaiNguoiDung = employee?.ChucVu,
                    XacNhan = 1,
                    MaNV = employee?.MaNv
                };
                if (!string.IsNullOrEmpty(acc.MaNguoiDung) &&
    !string.IsNullOrEmpty(acc.Ten) &&
    !string.IsNullOrEmpty(acc.Email))
                {
                    _context.TaiKhoans.Add(acc);
                    await _context.SaveChangesAsync();
                }
            }
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.MaNv }, employee);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while creating the employee.", Error = ex.Message });
        }
    }

    // Read all employees
    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        try
        {
            var employees = await _context.NhanViens
            .Where(e => e.An != true)
            .ToListAsync();
            return Ok(employees);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching employees.", Error = ex.Message });
        }
    }

    // Read a specific employee by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid employee ID." });

        try
        {
            var employee = await _context.NhanViens.FindAsync(id);
            if (employee == null)
                return NotFound(new { Message = "Employee not found." });

            return Ok(employee);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching the employee.", Error = ex.Message });
        }
    }

    // Update an employee
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] NhanVien employee)
    {
        if (id <= 0 || employee == null || id != employee.MaNv)
            return BadRequest(new { Message = "Invalid data provided." });

        try
        {
            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var existingAccount = await _context.TaiKhoans.FirstOrDefaultAsync(acc => acc.MaNV == employee.MaNv);

            if (existingAccount != null)
            {
                DateTime birthDate = DateTime.Parse(employee?.NgaySinh);

                // Tính năm sinh
                int birthYear = birthDate.Year;

                // Lấy năm hiện tại
                int currentYear = DateTime.Now.Year;

                // Tính tuổi
                int age = currentYear - birthYear;
                // Cập nhật các giá trị
                existingAccount.MaNguoiDung = employee?.Cccd;
                existingAccount.Ten = employee?.TenNv;
                existingAccount.SoDienThoai = employee?.SoDienThoai;
                existingAccount.Tuoi = age;
                existingAccount.LoaiNguoiDung = employee?.ChucVu;
                existingAccount.Email = employee?.Email;

                // Lưu thay đổi
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.MaNv }, employee);
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound(new { Message = "Employee not found for update." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while updating the employee.", Error = ex.Message });
        }
    }

    // Delete an employee
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid employee ID." });

        try
        {
            var employee = await _context.NhanViens.FindAsync(id);
            if (employee == null)
                return NotFound(new { Message = "Employee not found." });
            var user = await _context.TaiKhoans.FirstOrDefaultAsync(x => x.MaNguoiDung == employee.Cccd);
            if (user == null)
                return NotFound(new { Message = "User not found." });
            //xóa tài khoản, xóa mềm employee
            employee.An = true;
            _context.Entry(employee).State = EntityState.Modified;
            _context.TaiKhoans.Remove(user);
            await _context.SaveChangesAsync();

            //_context.NhanViens.Remove(employee);
            //await _context.SaveChangesAsync();
            return Ok(new { Message = "Employee deleted successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while deleting the employee.", Error = ex.Message });
        }
    }

    // Get employees by search criteria
    [HttpGet("search")]
    public async Task<IActionResult> GetEmployeesBySearch(
        [FromQuery] int? maNhanVien = null,
        [FromQuery] string? tenNhanVien = "",
        [FromQuery] string? chucVu = "Tất cả",
        [FromQuery] string? chiNhanh = "Tất cả",
        [FromQuery] decimal? luongDau = null,
        [FromQuery] decimal? luongCuoi = null)
    {
        try
        {
            // Bắt đầu với toàn bộ dữ liệu
            IQueryable<NhanVien> query = _context.NhanViens;

            // Lọc theo MaNv nếu có
            if (maNhanVien.HasValue)
            {
                query = query.Where(nv => nv.MaNv == maNhanVien.Value);
            }

            // Lọc theo tenNhanVien nếu không trống
            if (!string.IsNullOrWhiteSpace(tenNhanVien))
            {
                string normalizedTen = tenNhanVien.Trim().ToLower();
                query = query.Where(nv => nv.TenNv.ToLower().Contains(normalizedTen));
            }

            // Lọc theo chucVu nếu không phải "Tất cả"
            if (!string.Equals(chucVu, "Tất cả", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(nv => nv.ChucVu == chucVu);
            }

            // Lọc theo chiNhanh nếu không phải "Tất cả"
            if (!string.Equals(chiNhanh, "Tất cả", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(nv => nv.MaChiNhanhNavigation.TenCn == chiNhanh);
            }

            // Lọc theo luongCoBan
            if (luongDau.HasValue && luongCuoi.HasValue)
            {
                query = query.Where(nv => nv.LuongCoBan >= luongDau.Value && nv.LuongCoBan <= luongCuoi.Value);
            }
            else if (luongDau.HasValue)
            {
                query = query.Where(nv => nv.LuongCoBan >= luongDau.Value);
            }
            else if (luongCuoi.HasValue)
            {
                query = query.Where(nv => nv.LuongCoBan <= luongCuoi.Value);
            }

            // Sắp xếp theo MaNv
            var searchResults = await query.OrderBy(nv => nv.MaNv).Where(e => e.An != true).ToListAsync();

            return Ok(new { success = true, staffs = searchResults });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred while searching for employees.", Error = ex.Message });
        }
    }

    // Create a new attendance record
    [HttpPost("Attendance")]
    public async Task<IActionResult> CreateAttendance([FromBody] ChamCong attendance)
    {
        if (attendance == null)
            return BadRequest(new { Message = "Invalid attendance data." });

        try
        {
            _context.ChamCongs.Add(attendance);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAttendanceById), new { id = attendance.MaCc }, attendance);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while creating the attendance record.", Error = ex.Message });
        }
    }

    // Update an attendance record
    [HttpPut("Attendance/{id}")]
    public async Task<IActionResult> UpdateAttendance(int id, [FromBody] ChamCong attendance)
    {
        if (id <= 0 || attendance == null || id != attendance.MaCc)
            return BadRequest(new { Message = "Invalid data provided." });

        try
        {
            _context.Entry(attendance).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound(new { Message = "Attendance record not found for update." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while updating the attendance record.", Error = ex.Message });
        }
    }
    // Read all attendance records
    [HttpGet("Attendance")]
    public async Task<IActionResult> GetAllAttendances()
    {
        try
        {
            var attendances = await _context.ChamCongs.ToListAsync();
            return Ok(attendances);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching attendance records.", Error = ex.Message });
        }
    }
    // Read a specific attendance record by ID
    [HttpGet("Attendance/{id}")]
    public async Task<IActionResult> GetAttendanceById(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid attendance ID." });

        try
        {
            var attendance = await _context.ChamCongs.FindAsync(id);
            if (attendance == null)
                return NotFound(new { Message = "Attendance record not found." });

            return Ok(attendance);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching the attendance record.", Error = ex.Message });
        }
    }


}
