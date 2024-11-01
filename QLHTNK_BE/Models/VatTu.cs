using System;
using System.Collections.Generic;

namespace QLHTNK_BE.Models
{
    public partial class VatTu
    {
        public int MaVt { get; set; }
        public string? TenVt { get; set; }
        public int? MaChiNhanh { get; set; }
        public string? NgayNhap { get; set; }
        public int? SoLuongNhap { get; set; }
        public int? SoLuongTonKho { get; set; }
        public decimal? DonGiaNhap { get; set; }
        public bool? An { get; set; }

        public virtual ChiNhanh? MaChiNhanhNavigation { get; set; }
    }
}
