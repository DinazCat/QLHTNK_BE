using System;
using System.Collections.Generic;

namespace QLHTNK_BE.Models
{
    public partial class ChiNhanh
    {
        public ChiNhanh()
        {
            LichHens = new HashSet<LichHen>();
            NhanViens = new HashSet<NhanVien>();
            PhanHois = new HashSet<PhanHoi>();
            Thuocs = new HashSet<Thuoc>();
            VatTus = new HashSet<VatTu>();
        }

        public int MaCn { get; set; }
        public string? TenCn { get; set; }
        public string? DiaChi { get; set; }
        public string? MoTa { get; set; }
        public bool? An { get; set; }

        public virtual ICollection<LichHen> LichHens { get; set; }
        public virtual ICollection<NhanVien> NhanViens { get; set; }
        public virtual ICollection<PhanHoi> PhanHois { get; set; }
        public virtual ICollection<Thuoc> Thuocs { get; set; }
        public virtual ICollection<VatTu> VatTus { get; set; }
    }
}
