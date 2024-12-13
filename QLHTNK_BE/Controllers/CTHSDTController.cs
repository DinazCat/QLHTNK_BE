using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLHTNK_BE.Models;

namespace QLHTNK_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CTHSDTController : ControllerBase
    {
        private readonly DentalCentreManagementContext _context;

        public CTHSDTController(DentalCentreManagementContext context)
        {
            _context = context;
        }
        public class TreatmentRequest
        {
            public ChiTietHsdt ChiTietHsdt { get; set; }
            public List<DichVuDaSuDung>? DichVus { get; set; }
            public List<ThuocDaKe>? Thuocs { get; set; }
            public List<AnhSauDieuTri>? Anhs { get; set; }
            public HoaDon? HoaDon { get; set; }
        }

        //chứa record trước khi cập nhật
        public class UpdateRequest
        {
            public List<DichVuDaSuDung>? DichVusOld { get; set; }
            public List<ThuocDaKe>? ThuocsOld { get; set; }
            public List<AnhSauDieuTri>? AnhsOld { get; set; }
            public ChiTietHsdt ChiTietHsdt { get; set; }
            public List<DichVuDaSuDung>? DichVus { get; set; }
            public List<ThuocDaKe>? Thuocs { get; set; }
            public List<AnhSauDieuTri>? Anhs { get; set; }
        }
        // Add a new treatment record (ChiTietHsdt)
        [HttpPost]
        public async Task<IActionResult> AddTreatmentDetail([FromBody] TreatmentRequest treatmentDetail)
        {
            if (treatmentDetail == null || treatmentDetail.ChiTietHsdt == null || treatmentDetail.HoaDon == null)
                return BadRequest(new { Message = "Invalid treatment detail data." });

            try
            {
                // Kiểm tra bệnh nhân
                var patient = await _context.BenhNhans.FindAsync(treatmentDetail.ChiTietHsdt.MaBn);
                if (patient == null)
                    return NotFound(new { Message = "Patient not found." });

                // Thêm chi tiết hồ sơ điều trị
                _context.ChiTietHsdts.Add(treatmentDetail.ChiTietHsdt);
                await _context.SaveChangesAsync();

                // Lấy mã ChiTietHsdt vừa được thêm
                var maCthsdt = treatmentDetail.ChiTietHsdt.MaCthsdt;

                // Thêm dịch vụ đã sử dụng
                if (treatmentDetail.DichVus != null && treatmentDetail.DichVus.Any())
                {
                    foreach (var dichVu in treatmentDetail.DichVus)
                    {
                        dichVu.MaCthsdt = maCthsdt;
                        _context.DichVuDaSuDungs.Add(dichVu);
                    }
                }

                // Thêm thuốc đã sử dụng
                if (treatmentDetail.Thuocs != null && treatmentDetail.Thuocs.Any())
                {
                    foreach (var thuoc in treatmentDetail.Thuocs)
                    {
                        var thuocDB = await _context.Thuocs.FirstOrDefaultAsync(x => x.MaThuoc == thuoc.MaThuoc);
                        if (thuocDB == null)
                        {
                            return NotFound(new { Message = "Một trong các loại thuốc không tồn tại." });
                        }

                        if (thuocDB.SoLuongTonKho < thuoc.SoLuong)
                        {
                            return BadRequest(new { Message = "Số lượng trong thuốc kho không đủ." });
                        }

                        thuocDB.SoLuongTonKho -= (int)thuoc.SoLuong;
                        thuoc.MaCthsdt = maCthsdt;
                        _context.ThuocDaKes.Add(thuoc);
                    }
                }
                //Cập nhật thuốc trong kho

                // Thêm hình ảnh
                if (treatmentDetail.Anhs != null && treatmentDetail.Anhs.Any())
                {
                    foreach (var anh in treatmentDetail.Anhs)
                    {
                        anh.MaCthsdt = maCthsdt;
                        _context.AnhSauDieuTris.Add(anh);
                    }
                }

                // Thêm hóa đơn
                treatmentDetail.HoaDon.MaCthsdt = maCthsdt;
                _context.HoaDons.Add(treatmentDetail.HoaDon);

                // Lưu thay đổi
                await _context.SaveChangesAsync();
                var treatmentDetails = await _context.ChiTietHsdts
                                                  .Include(t => t.MaNhaSiNavigation) // Load related NhanVien details
                                                  .Include(t => t.DichVuDaSuDungs) // Load DichVuDaSuDungs list
                                                  .Include(t => t.ThuocDaKes) // Load ThuocDaKes list
                                                  .Include(t => t.AnhSauDieuTris)
                                                  .Include(t => t.HoaDons)
                                                  .FirstOrDefaultAsync(t => t.MaCthsdt == maCthsdt); ;

                return Ok(treatmentDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while adding the treatment detail.", Error = ex.Message });
            }
        }

        // Get treatment detail by ID
        [HttpGet("{id}")]
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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTreatmentDetail(int id, [FromBody] UpdateRequest oldVersion)
        {
            if (id <= 0 || oldVersion == null || id != oldVersion.ChiTietHsdt.MaCthsdt)
                return BadRequest(new { Message = "Invalid data provided." });

            try
            {
                // Check if the treatment record exists
                var existingTreatmentDetail = await _context.ChiTietHsdts
            .Include(t => t.MaNhaSiNavigation)
             .Include(t => t.HoaDons)
            .FirstOrDefaultAsync(t => t.MaCthsdt == id);
                if (existingTreatmentDetail == null)
                    return NotFound(new { Message = "Treatment detail not found." });
                if (existingTreatmentDetail.HoaDons.Count > 0 && existingTreatmentDetail.HoaDons.FirstOrDefault().TinhTrang == "Đã thanh toán")
                {
                    return BadRequest(new { Message = "Treatment cannot be edit because the bill has been paid completely." });
                }
                // cập nhật thuốc đã kê
                if (oldVersion.ThuocsOld != null && oldVersion.Thuocs != null && oldVersion.Thuocs.Count > 0)
                {
                    // Xóa hết thuốc đã kê lúc trước
                    foreach (var thuoc in oldVersion.ThuocsOld)
                    {
                        var thuoctrongKho = await _context.Thuocs.FirstOrDefaultAsync(x => x.MaThuoc == thuoc.MaThuoc);
                        if (thuoctrongKho != null)
                        {
                            thuoctrongKho.SoLuongTonKho = (int)thuoctrongKho.SoLuongTonKho + (int)thuoc.SoLuong;
                            _context.ThuocDaKes.Remove(thuoc);
                            _context.Entry(thuoctrongKho).State = EntityState.Modified; // Cập nhật trạng thái
                        }
                    }
                    await _context.SaveChangesAsync();
                    // Cập nhật
                    foreach (var thuoc in oldVersion.Thuocs)
                    {
                        //thuốc trong kho
                        var thuoctrongKho = await _context.Thuocs.FirstOrDefaultAsync(x => x.MaThuoc == thuoc.MaThuoc);
                        if (thuoctrongKho != null)
                        {
                            thuoctrongKho.SoLuongTonKho = (int)thuoctrongKho.SoLuongTonKho - (int)thuoc.SoLuong;
                            if (thuoctrongKho.SoLuongTonKho < 0)
                            {
                                return BadRequest(new { Message = "Số lượng trong thuốc kho không đủ." });
                            }
                            _context.Entry(thuoctrongKho).State = EntityState.Modified; // Cập nhật trạng thái
                            thuoc.MaCthsdt = id;
                            _context.ThuocDaKes.Add(thuoc);

                        }
                    }
                }

                //xóa list dv đã dùng rồi add lại xem như cập nhật
                if (oldVersion.DichVusOld != null && oldVersion.DichVus != null && oldVersion.DichVus.Count > 0)
                {
                    // Xóa hết dịch vụ đã dùng lúc trước
                    _context.DichVuDaSuDungs.RemoveRange(oldVersion.DichVusOld);
                    await _context.SaveChangesAsync();
                    // Cập nhật
                    foreach (var dichvu in oldVersion.DichVus)
                    {
                        dichvu.MaCthsdt = id;
                        _context.DichVuDaSuDungs.Add(dichvu);
                    }

                }
                _context.Entry(existingTreatmentDetail).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetTreatmentDetailById), new { id = id }, existingTreatmentDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the treatment detail.", Error = ex.Message });
            }
        }

        // Delete a treatment record (ChiTietHsdt)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTreatmentDetail(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid treatment detail ID." });

            try
            {
                var treatmentDetail = await _context.ChiTietHsdts.Include(t => t.DichVuDaSuDungs) // Load DichVuDaSuDungs list
                                                                 .Include(t => t.ThuocDaKes)
                                                                 .Include(t => t.AnhSauDieuTris)
                                                                 .Include(t => t.HoaDons)
                                                                 .FirstOrDefaultAsync(x => x.MaCthsdt == id);

                if (treatmentDetail == null)
                    return NotFound(new { Message = "Treatment detail not found." });
                if (treatmentDetail.HoaDons.Count > 0 && treatmentDetail.HoaDons.FirstOrDefault().TinhTrang != null)
                {
                    return BadRequest(new { Message = "Treatment cannot be deleted because the bill has been paid." });
                }
                //xóa ảnh sau điều trị
                if (treatmentDetail.AnhSauDieuTris.Count > 0)
                {
                    _context.AnhSauDieuTris.RemoveRange(treatmentDetail.AnhSauDieuTris);
                    await _context.SaveChangesAsync();
                }
                //xóa dịch vụ đã sử dụng
                if (treatmentDetail.DichVuDaSuDungs.Count > 0)
                {
                    _context.DichVuDaSuDungs.RemoveRange(treatmentDetail.DichVuDaSuDungs);
                    await _context.SaveChangesAsync();
                }
                //xóa thuốc đã kê và cập nhật lại kho
                if (treatmentDetail.DichVuDaSuDungs.Count > 0)
                {
                    foreach (var thuoc in treatmentDetail.ThuocDaKes)
                    {
                        var thuoctrongKho = await _context.Thuocs.FirstOrDefaultAsync(x => x.MaThuoc == thuoc.MaThuoc);
                        if (thuoctrongKho != null)
                        {
                            thuoctrongKho.SoLuongTonKho = (int)thuoctrongKho.SoLuongTonKho + (int)thuoc.SoLuong;
                            _context.ThuocDaKes.Remove(thuoc);
                            _context.Entry(thuoctrongKho).State = EntityState.Modified; // Cập nhật trạng thái
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                //xóa hóa đơn
                if (treatmentDetail.HoaDons.Count > 0)
                {
                    _context.HoaDons.RemoveRange(treatmentDetail.HoaDons);
                    await _context.SaveChangesAsync();
                }
                //xóa cthsdt
                _context.ChiTietHsdts.Remove(treatmentDetail);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "TreatmentDetail deleted successfully." });
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
                // Fetch treatment details for a specific patient, including NhanVien details
                var treatmentDetails = await _context.ChiTietHsdts
                                                     .Where(t => t.MaBn == patientId)
                                                     .Include(t => t.MaNhaSiNavigation) // Load related NhanVien details
                                                     .Include(t => t.DichVuDaSuDungs) // Load DichVuDaSuDungs list
                                                     .Include(t => t.ThuocDaKes) // Load ThuocDaKes list
                                                     .Include(t => t.AnhSauDieuTris)
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
        // Get patients by search criteria
        [HttpGet("search")]
        public async Task<IActionResult> GetCTHSDTsBySearch(
        [FromQuery] int? MaNhaSi = null,
        [FromQuery] string? TenNhaSi = null,
        [FromQuery] string? NgayDieuTri = null)
        {
            try
            {
                // Start with the full set of patients
                IQueryable<ChiTietHsdt> query = _context.ChiTietHsdts.Include(b => b.MaNhaSiNavigation);

                // Apply filters based on provided criteria
                if (MaNhaSi.HasValue)
                {
                    query = query.Where(bn => bn.MaNhaSi == MaNhaSi.Value);
                }

                if (!string.IsNullOrEmpty(TenNhaSi))
                {
                    query = query.Where(bn => bn.MaNhaSiNavigation.TenNv.Contains(TenNhaSi));
                }

                if (!string.IsNullOrEmpty(NgayDieuTri))
                {
                    query = query.Where(bn => bn.NgayDieuTri == NgayDieuTri
                    );
                }


                // Execute the query and sort by MaBn
                var cthsdt = await query.OrderBy(bn => bn.MaCthsdt).Include(t => t.MaNhaSiNavigation) // Load related NhanVien details
                                                     .Include(t => t.DichVuDaSuDungs) // Load DichVuDaSuDungs list
                                                     .Include(t => t.ThuocDaKes) // Load ThuocDaKes list
                                                     .Include(t => t.AnhSauDieuTris).ToListAsync();

                return Ok(new { Success = true, Cthsdts = cthsdt });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "An error occurred while fetching treatment list.", Error = ex.Message });
            }
        }
    }

}
