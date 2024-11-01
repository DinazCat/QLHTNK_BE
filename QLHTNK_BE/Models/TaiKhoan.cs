using System;
using System.Collections.Generic;

namespace QLHTNK_BE.Models
{
    public partial class TaiKhoan
    {
        public int MaTk { get; set; }
        public int? MaNguoiDung { get; set; }
        public string? Ten { get; set; }
        public string? SoDienThoai { get; set; }
        public int? Tuoi { get; set; }
        public string? Email { get; set; }
        public string? MatKhau { get; set; }
        public string? LoaiNguoiDung { get; set; }
    }
}
