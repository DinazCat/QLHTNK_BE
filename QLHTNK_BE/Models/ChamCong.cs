using System;
using System.Collections.Generic;

namespace QLHTNK_BE.Models
{
    public partial class ChamCong
    {
        public int MaCc { get; set; }
        public int MaNv { get; set; }
        public string? Ngay { get; set; }
        public decimal? SoGioLam { get; set; }
    }
}
