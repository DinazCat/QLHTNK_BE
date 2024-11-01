using System;
using System.Collections.Generic;

namespace QLHTNK_BE.Models
{
    public partial class BenhNhan
    {
        public BenhNhan()
        {
            ChiTietHsdts = new HashSet<ChiTietHsdt>();
            HoaDons = new HashSet<HoaDon>();
            LichHens = new HashSet<LichHen>();
            PhanHois = new HashSet<PhanHoi>();
        }

        public int MaBn { get; set; }
        public string? TenBn { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Cccd { get; set; }
        public string? DiaChi { get; set; }
        public string? GioiTinh { get; set; }
        public string? NgaySinh { get; set; }
        public string? TienSuBenhLy { get; set; }
        public decimal? TongChi { get; set; }
        public decimal? ChietKhau { get; set; }
        public decimal? DaThanhToan { get; set; }
        public decimal? CongNo { get; set; }

        public virtual ICollection<ChiTietHsdt> ChiTietHsdts { get; set; }
        public virtual ICollection<HoaDon> HoaDons { get; set; }
        public virtual ICollection<LichHen> LichHens { get; set; }
        public virtual ICollection<PhanHoi> PhanHois { get; set; }
    }
}
