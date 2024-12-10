using System;
using System.Collections.Generic;

namespace QLHTNK_BE.Models
{
    public partial class NhanVien
    {
        public NhanVien()
        {
            ChiTietHsdts = new HashSet<ChiTietHsdt>();
            HoaDons = new HashSet<HoaDon>();
            LichHens = new HashSet<LichHen>();
            TaiKhoans = new TaiKhoan();
        }

        public int MaNv { get; set; }
        public string? TenNv { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public string? Cccd { get; set; }
        public string? GioiTinh { get; set; }
        public string? NgaySinh { get; set; }
        public string? ChucVu { get; set; }
        public string? BangCap { get; set; }
        public int? KinhNghiem { get; set; }
        public decimal? LuongCoBan { get; set; }
        public int? MaChiNhanh { get; set; }
        public bool? An { get; set; }

        public virtual ChiNhanh? MaChiNhanhNavigation { get; set; }
        public virtual TaiKhoan? TaiKhoans { get; set; }
        public virtual ICollection<ChiTietHsdt> ChiTietHsdts { get; set; }
        public virtual ICollection<HoaDon> HoaDons { get; set; }
        public virtual ICollection<LichHen> LichHens { get; set; }
    }
}
