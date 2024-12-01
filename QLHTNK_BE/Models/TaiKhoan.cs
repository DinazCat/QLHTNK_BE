using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace QLHTNK_BE.Models
{
    public partial class TaiKhoan : IdentityUser
    {
        public string? MaTk { get; set; } 
        public string? MaNguoiDung { get; set; }
        public string? Ten { get; set; }
        public string? SoDienThoai { get; set; }
        public int? Tuoi { get; set; }
        public string? Email { get; set; }
        public string? MatKhau { get; set; }
        public string? LoaiNguoiDung { get; set; }
        public int? XacNhan {  get; set; }
        public string? Token { get; set; }
        public int? MaNV { get; set; }
    }
}
