using System;
using System.Collections.Generic;

namespace QLHTNK_BE.Models
{
    public partial class ChiTietThanhToan
    {
        public int? MaHd { get; set; }
        public string? NgayThanhToan { get; set; }
        public decimal? SoTienThanhToan { get; set; }
        public string? HinhThucThanhToan { get; set; }

        public virtual HoaDon? MaHdNavigation { get; set; }
    }
}
