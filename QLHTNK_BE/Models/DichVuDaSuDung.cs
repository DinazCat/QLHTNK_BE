using System;
using System.Collections.Generic;

namespace QLHTNK_BE.Models
{
    public partial class DichVuDaSuDung
    {
        public int? MaCthsdt { get; set; }
        public int? MaDv { get; set; }
        public decimal? GiaDichVu { get; set; }
        public int? SoLuong { get; set; }
        public decimal? ChietKhau { get; set; }
        public decimal? DonGia { get; set; }
        public string? GhiChu { get; set; }
        public string? TaiKham { get; set; }

        public virtual ChiTietHsdt? MaCthsdtNavigation { get; set; }
        public virtual DichVu? MaDvNavigation { get; set; }
    }
}
