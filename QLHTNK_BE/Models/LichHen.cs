using System;
using System.Collections.Generic;

namespace QLHTNK_BE.Models
{
    public partial class LichHen
    {
        public int MaLichHen { get; set; }
        public int? MaChiNhanh { get; set; }
        public int? MaBn { get; set; }
        public int? MaNs { get; set; }
        public string? HoTen { get; set; }
        public string? SoDienThoai { get; set; }
        public string? DiaChi { get; set; }
        public string? Email { get; set; }
        public string? Ngay { get; set; }
        public string? Gio { get; set; }
        public string? LyDoKham { get; set; }
        public string? GhiChu { get; set; }
        public string? LoaiLichHen { get; set; }
        public string? TrangThai { get; set; }

        public virtual BenhNhan? MaBnNavigation { get; set; }
        public virtual ChiNhanh? MaChiNhanhNavigation { get; set; }
        public virtual NhanVien? MaNsNavigation { get; set; }
    }
}
