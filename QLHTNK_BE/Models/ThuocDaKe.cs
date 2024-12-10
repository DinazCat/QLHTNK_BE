using System;
using System.Collections.Generic;

namespace QLHTNK_BE.Models
{
    public partial class ThuocDaKe
    {
        public int? MaTDK { get; set; }
        public int? MaCthsdt { get; set; }
        public int? MaThuoc { get; set; }
        public int? SoLuong { get; set; }
        public decimal? Gia { get; set; }
        public decimal? DonGia { get; set; }
        public string? GhiChu { get; set; }

        public virtual ChiTietHsdt? MaCthsdtNavigation { get; set; }
        public virtual Thuoc? MaThuocNavigation { get; set; }
    }
}
