using System;
using System.Collections.Generic;

namespace QLHTNK_BE.Models
{
    public partial class AnhSauDieuTri
    {
        public int MaAnh { get; set; }
        public int? MaCthsdt { get; set; }
        public int? MaBn { get; set; }
        public byte[]? Anh { get; set; }
        public string? MoTa { get; set; }

        public virtual ChiTietHsdt? MaCthsdtNavigation { get; set; }
    }
}
