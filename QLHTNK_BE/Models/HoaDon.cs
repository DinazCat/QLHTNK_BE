using System;
using System.Collections.Generic;

namespace QLHTNK_BE.Models
{
    public partial class HoaDon
    {
        public int MaHd { get; set; }
        public int? MaBn { get; set; }
        public int? MaNv { get; set; }
        public int? MaCthsdt { get; set; }
        public int? MaGiamGia { get; set; }
        public string? NgayLap { get; set; }
        public decimal? SoTienGiam { get; set; }
        public decimal? PhanTramGiam { get; set; }
        public string? LyDoGiam { get; set; }
        public decimal? SoTienDaThanhToan { get; set; }
        public decimal? SoTienConNo { get; set; }
        public string? TinhTrang { get; set; }

        public virtual BenhNhan? MaBnNavigation { get; set; }
        public virtual ChiTietHsdt? MaCthsdtNavigation { get; set; }
        public virtual GiamGia? MaGiamGiaNavigation { get; set; }
        public virtual NhanVien? MaNvNavigation { get; set; }
    }
}
