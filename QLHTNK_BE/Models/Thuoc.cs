using System;
using System.Collections.Generic;

namespace QLHTNK_BE.Models
{
    public partial class Thuoc
    {
        public int MaThuoc { get; set; }
        public string? TenThuoc { get; set; }
        public int? MaChiNhanh { get; set; }
        public string? NgayNhap { get; set; }
        public int? SoLuongNhap { get; set; }
        public int? SoLuongTonKho { get; set; }
        public decimal? DonGiaNhap { get; set; }
        public decimal? DonGiaBan { get; set; }
        public string? HanSuDung { get; set; }
        public bool? An { get; set; }

        public virtual ChiNhanh? MaChiNhanhNavigation { get; set; }
    }
}
