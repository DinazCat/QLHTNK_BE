using System;
using System.Collections.Generic;

namespace QLHTNK_BE.Models
{
    public partial class PhanHoi
    {
        public int MaPh { get; set; }
        public int? MaChiNhanh { get; set; }
        public string? Ngay { get; set; }
        public string? Gio { get; set; }
        public string? NoiDung { get; set; }
        public int? MaBn { get; set; }

        public virtual BenhNhan? MaBnNavigation { get; set; }
        public virtual ChiNhanh? MaChiNhanhNavigation { get; set; }
    }
}
