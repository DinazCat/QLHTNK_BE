using System;
using System.Collections.Generic;

namespace QLHTNK_BE.Models
{
    public partial class GiamGium
    {
        public GiamGium()
        {
            HoaDons = new HashSet<HoaDon>();
        }

        public int MaGiamGia { get; set; }
        public string? TenGiamGia { get; set; }
        public decimal? SoTienGiam { get; set; }
        public decimal? PhanTramGiam { get; set; }
        public string? DieuKienApDung { get; set; }
        public string? NgayBatDau { get; set; }
        public string? NgayKetThuc { get; set; }
        public bool? An { get; set; }

        public virtual ICollection<HoaDon> HoaDons { get; set; }
    }
}
