using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QLHTNK_BE.Models
{
    public partial class LuongThuong
    {
        public int MaLT { get; set; }
        public string LoaiLT { get; set; } = string.Empty;
        public string LoaiNV { get; set; } = string.Empty;
        public int? MaNV { get; set; }
        public int Nam { get; set; }
        public int Thang { get; set; }
        public decimal Tien { get; set; }
        public int? MaCN { get; set; }
        public string? GhiChu { get; set; }
        public bool? An { get; set; }
    }
}
