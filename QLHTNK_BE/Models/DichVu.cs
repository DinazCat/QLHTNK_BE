using System;
using System.Collections.Generic;

namespace QLHTNK_BE.Models
{
    public partial class DichVu
    {
        public int MaDv { get; set; }
        public string? TenDv { get; set; }
        public string? LoaiDv { get; set; }
        public decimal? GiaThapNhat { get; set; }
        public decimal? GiaCaoNhat { get; set; }
        public string? MoTa { get; set; }
        public string? ChinhSachBaoHanh { get; set; }
        public bool? An { get; set; }
    }
}
