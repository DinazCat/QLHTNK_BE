using System;
using System.Collections.Generic;

namespace QLHTNK_BE.Models
{
    public partial class ChiTietHsdt
    {
        public ChiTietHsdt()
        {
            AnhSauDieuTris = new HashSet<AnhSauDieuTri>();
            HoaDons = new HashSet<HoaDon>();
        }

        public int MaCthsdt { get; set; }
        public int? MaBn { get; set; }
        public string? LyDoKham { get; set; }
        public string? ChanDoan { get; set; }
        public string? NgayDieuTri { get; set; }
        public int? MaNhaSi { get; set; }
        public int? MaChiNhanh { get; set; }
        public string? GhiChu { get; set; }

        public virtual BenhNhan? MaBnNavigation { get; set; }
        public virtual NhanVien? MaNhaSiNavigation { get; set; }
        public virtual ICollection<AnhSauDieuTri> AnhSauDieuTris { get; set; }
        public virtual ICollection<HoaDon> HoaDons { get; set; }
    }
}
