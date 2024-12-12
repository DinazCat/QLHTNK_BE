using System;
using System.Collections.Generic;

namespace QLHTNK_BE.Models
{
    public partial class LichLamViec
    {
        public int MaLichLamViec { get; set; }
        public int? MaNs { get; set; }
        public string? Ngay { get; set; }
        public string? GioBatDau { get; set; }
        public string? GioKetThuc { get; set; }
        public string? GhiChu { get; set; }

        public virtual NhanVien? MaNsNavigation { get; set; }
    }
}
