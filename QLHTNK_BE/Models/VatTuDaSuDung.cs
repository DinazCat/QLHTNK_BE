using System;
using System.Collections.Generic;

namespace QLHTNK_BE.Models
{
    public partial class VatTuDaSuDung
    {
        public int? MaVatTu { get; set; }
        public int? MaChiNhanh { get; set; }
        public string? NgaySuDung { get; set; }
        public int? SoLuong { get; set; }

        public virtual ChiNhanh? MaChiNhanhNavigation { get; set; }
        public virtual VatTu? MaVatTuNavigation { get; set; }
    }
}
