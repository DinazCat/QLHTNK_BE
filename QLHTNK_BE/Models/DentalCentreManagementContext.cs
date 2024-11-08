using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QLHTNK_BE.Models
{
    public partial class DentalCentreManagementContext : DbContext
    {
        public DentalCentreManagementContext()
        {
        }

        public DentalCentreManagementContext(DbContextOptions<DentalCentreManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AnhSauDieuTri> AnhSauDieuTris { get; set; } = null!;
        public virtual DbSet<BenhNhan> BenhNhans { get; set; } = null!;
        public virtual DbSet<ChiNhanh> ChiNhanhs { get; set; } = null!;
        public virtual DbSet<ChiTietHsdt> ChiTietHsdts { get; set; } = null!;
        public virtual DbSet<ChiTietThanhToan> ChiTietThanhToans { get; set; } = null!;
        public virtual DbSet<DichVu> DichVus { get; set; } = null!;
        public virtual DbSet<DichVuDaSuDung> DichVuDaSuDungs { get; set; } = null!;
        public virtual DbSet<GiamGia> GiamGias { get; set; } = null!;
        public virtual DbSet<HoaDon> HoaDons { get; set; } = null!;
        public virtual DbSet<LichHen> LichHens { get; set; } = null!;
        public virtual DbSet<NhanVien> NhanViens { get; set; } = null!;
        public virtual DbSet<PhanHoi> PhanHois { get; set; } = null!;
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; } = null!;
        public virtual DbSet<Thuoc> Thuocs { get; set; } = null!;
        public virtual DbSet<ThuocDaKe> ThuocDaKes { get; set; } = null!;
        public virtual DbSet<VatTu> VatTus { get; set; } = null!;
        public virtual DbSet<VatTuDaSuDung> VatTuDaSuDungs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=DentalCentreManagement;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnhSauDieuTri>(entity =>
            {
                entity.HasKey(e => e.MaAnh)
                    .HasName("PK__AnhSauDi__356240DFD04E0549");

                entity.ToTable("AnhSauDieuTri");

                entity.Property(e => e.MaAnh).ValueGeneratedNever();

                entity.Property(e => e.MaBn).HasColumnName("MaBN");

                entity.Property(e => e.MaCthsdt).HasColumnName("MaCTHSDT");

                entity.Property(e => e.MoTa).HasMaxLength(255);

                entity.HasOne(d => d.MaCthsdtNavigation)
                    .WithMany(p => p.AnhSauDieuTris)
                    .HasForeignKey(d => d.MaCthsdt)
                    .HasConstraintName("FK__AnhSauDie__MaCTH__534D60F1");
            });

            modelBuilder.Entity<BenhNhan>(entity =>
            {
                entity.HasKey(e => e.MaBn)
                    .HasName("PK__BenhNhan__272475AD9E5324A6");

                entity.ToTable("BenhNhan");

                entity.Property(e => e.MaBn)
                    .ValueGeneratedNever()
                    .HasColumnName("MaBN");

                entity.Property(e => e.Cccd)
                    .HasMaxLength(12)
                    .HasColumnName("CCCD");

                entity.Property(e => e.ChietKhau).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CongNo).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DaThanhToan).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DiaChi).HasMaxLength(255);

                entity.Property(e => e.GioiTinh).HasMaxLength(5);

                entity.Property(e => e.NgaySinh).HasMaxLength(10);

                entity.Property(e => e.SoDienThoai).HasMaxLength(15);

                entity.Property(e => e.TenBn)
                    .HasMaxLength(100)
                    .HasColumnName("TenBN");

                entity.Property(e => e.TienSuBenhLy).HasMaxLength(255);

                entity.Property(e => e.TongChi).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<ChiNhanh>(entity =>
            {
                entity.HasKey(e => e.MaCn)
                    .HasName("PK__ChiNhanh__27258E0ECB34AE49");

                entity.ToTable("ChiNhanh");

                entity.Property(e => e.MaCn)
                    .ValueGeneratedNever()
                    .HasColumnName("MaCN");

                entity.Property(e => e.An).HasDefaultValueSql("((0))");

                entity.Property(e => e.DiaChi).HasMaxLength(255);

                entity.Property(e => e.MoTa).HasMaxLength(255);

                entity.Property(e => e.TenCn)
                    .HasMaxLength(100)
                    .HasColumnName("TenCN");
            });

            modelBuilder.Entity<ChiTietHsdt>(entity =>
            {
                entity.HasKey(e => e.MaCthsdt)
                    .HasName("PK__ChiTietH__D1B6184E09716A0A");

                entity.ToTable("ChiTietHSDT");

                entity.Property(e => e.MaCthsdt)
                    .ValueGeneratedNever()
                    .HasColumnName("MaCTHSDT");

                entity.Property(e => e.ChanDoan).HasMaxLength(255);

                entity.Property(e => e.GhiChu).HasMaxLength(255);

                entity.Property(e => e.LyDoKham).HasMaxLength(255);

                entity.Property(e => e.MaBn).HasColumnName("MaBN");

                entity.Property(e => e.NgayDieuTri).HasMaxLength(10);

                entity.HasOne(d => d.MaBnNavigation)
                    .WithMany(p => p.ChiTietHsdts)
                    .HasForeignKey(d => d.MaBn)
                    .HasConstraintName("FK__ChiTietHSD__MaBN__4F7CD00D");

                entity.HasOne(d => d.MaNhaSiNavigation)
                    .WithMany(p => p.ChiTietHsdts)
                    .HasForeignKey(d => d.MaNhaSi)
                    .HasConstraintName("FK__ChiTietHS__MaNha__5070F446");
            });

            modelBuilder.Entity<ChiTietThanhToan>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ChiTietThanhToan");

                entity.Property(e => e.HinhThucThanhToan).HasMaxLength(50);

                entity.Property(e => e.MaHd).HasColumnName("MaHD");

                entity.Property(e => e.NgayThanhToan).HasMaxLength(10);

                entity.Property(e => e.SoTienThanhToan).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.MaHdNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.MaHd)
                    .HasConstraintName("FK__ChiTietTha__MaHD__6A30C649");
            });

            modelBuilder.Entity<DichVu>(entity =>
            {
                entity.HasKey(e => e.MaDv)
                    .HasName("PK__DichVu__2725865770F9286E");

                entity.ToTable("DichVu");

                entity.Property(e => e.MaDv)
                    .ValueGeneratedNever()
                    .HasColumnName("MaDV");

                entity.Property(e => e.An).HasDefaultValueSql("((0))");

                entity.Property(e => e.ChinhSachBaoHanh).HasMaxLength(255);

                entity.Property(e => e.GiaCaoNhat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.GiaThapNhat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LoaiDv)
                    .HasMaxLength(50)
                    .HasColumnName("LoaiDV");

                entity.Property(e => e.MoTa).HasMaxLength(255);

                entity.Property(e => e.TenDv)
                    .HasMaxLength(100)
                    .HasColumnName("TenDV");
            });

            modelBuilder.Entity<DichVuDaSuDung>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DichVuDaSuDung");

                entity.Property(e => e.ChietKhau).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DonGia)
                    .HasColumnType("decimal(29, 2)")
                    .HasComputedColumnSql("([GiaDichVu]*[SoLuong])", false);

                entity.Property(e => e.GhiChu).HasMaxLength(255);

                entity.Property(e => e.GiaDichVu).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MaCthsdt).HasColumnName("MaCTHSDT");

                entity.Property(e => e.MaDv).HasColumnName("MaDV");

                entity.Property(e => e.TaiKham).HasMaxLength(20);

                entity.HasOne(d => d.MaCthsdtNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.MaCthsdt)
                    .HasConstraintName("FK__DichVuDaS__MaCTH__5535A963");

                entity.HasOne(d => d.MaDvNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.MaDv)
                    .HasConstraintName("FK__DichVuDaSu__MaDV__5629CD9C");
            });

            modelBuilder.Entity<GiamGia>(entity =>
            {
                entity.HasKey(e => e.MaGiamGia)
                    .HasName("PK__GiamGia__EF9458E4BA4D07E4");

                entity.Property(e => e.MaGiamGia).ValueGeneratedNever();

                entity.Property(e => e.An).HasDefaultValueSql("((0))");

                entity.Property(e => e.DieuKienApDung).HasMaxLength(255);

                entity.Property(e => e.NgayBatDau).HasMaxLength(10);

                entity.Property(e => e.NgayKetThuc).HasMaxLength(10);

                entity.Property(e => e.PhanTramGiam).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.SoTienGiam).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TenGiamGia).HasMaxLength(100);
            });

            modelBuilder.Entity<HoaDon>(entity =>
            {
                entity.HasKey(e => e.MaHd)
                    .HasName("PK__HoaDon__2725A6E0574173FC");

                entity.ToTable("HoaDon");

                entity.Property(e => e.MaHd)
                    .ValueGeneratedNever()
                    .HasColumnName("MaHD");

                entity.Property(e => e.LyDoGiam).HasMaxLength(255);

                entity.Property(e => e.MaBn).HasColumnName("MaBN");

                entity.Property(e => e.MaCthsdt).HasColumnName("MaCTHSDT");

                entity.Property(e => e.MaNv).HasColumnName("MaNV");

                entity.Property(e => e.NgayLap).HasMaxLength(10);

                entity.Property(e => e.PhanTramGiam).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.SoTienConNo).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SoTienDaThanhToan).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SoTienGiam).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TinhTrang).HasMaxLength(50);

                entity.HasOne(d => d.MaBnNavigation)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.MaBn)
                    .HasConstraintName("FK__HoaDon__MaBN__656C112C");

                entity.HasOne(d => d.MaCthsdtNavigation)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.MaCthsdt)
                    .HasConstraintName("FK__HoaDon__MaCTHSDT__6754599E");

                entity.HasOne(d => d.MaGiamGiaNavigation)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.MaGiamGia)
                    .HasConstraintName("FK__HoaDon__MaGiamGi__68487DD7");

                entity.HasOne(d => d.MaNvNavigation)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.MaNv)
                    .HasConstraintName("FK__HoaDon__MaNV__66603565");
            });

            modelBuilder.Entity<LichHen>(entity =>
            {
                entity.HasKey(e => e.MaLichHen)
                    .HasName("PK__LichHen__150F264F73956887");

                entity.ToTable("LichHen");

                entity.Property(e => e.MaLichHen).ValueGeneratedNever();

                entity.Property(e => e.DiaChi).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.GhiChu).HasMaxLength(255);

                entity.Property(e => e.Gio).HasMaxLength(5);

                entity.Property(e => e.HoTen).HasMaxLength(100);

                entity.Property(e => e.LoaiLichHen).HasMaxLength(50);

                entity.Property(e => e.LyDoKham).HasMaxLength(255);

                entity.Property(e => e.MaBn).HasColumnName("MaBN");

                entity.Property(e => e.MaNs).HasColumnName("MaNS");

                entity.Property(e => e.Ngay).HasMaxLength(10);

                entity.Property(e => e.SoDienThoai).HasMaxLength(15);

                entity.Property(e => e.TrangThai).HasMaxLength(50);

                entity.HasOne(d => d.MaBnNavigation)
                    .WithMany(p => p.LichHens)
                    .HasForeignKey(d => d.MaBn)
                    .HasConstraintName("FK__LichHen__MaBN__4BAC3F29");

                entity.HasOne(d => d.MaNsNavigation)
                    .WithMany(p => p.LichHens)
                    .HasForeignKey(d => d.MaNs)
                    .HasConstraintName("FK__LichHen__MaNS__4CA06362");
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.HasKey(e => e.MaNv)
                    .HasName("PK__NhanVien__2725D70ADE521880");

                entity.ToTable("NhanVien");

                entity.Property(e => e.MaNv)
                    .ValueGeneratedNever()
                    .HasColumnName("MaNV");

                entity.Property(e => e.An).HasDefaultValueSql("((0))");

                entity.Property(e => e.BangCap).HasMaxLength(50);

                entity.Property(e => e.Cccd)
                    .HasMaxLength(12)
                    .HasColumnName("CCCD");

                entity.Property(e => e.ChucVu).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.GioiTinh).HasMaxLength(5);

                entity.Property(e => e.LuongCoBan).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NgaySinh).HasMaxLength(10);

                entity.Property(e => e.SoDienThoai).HasMaxLength(15);

                entity.Property(e => e.TenNv)
                    .HasMaxLength(100)
                    .HasColumnName("TenNV");

                entity.HasOne(d => d.MaChiNhanhNavigation)
                    .WithMany(p => p.NhanViens)
                    .HasForeignKey(d => d.MaChiNhanh)
                    .HasConstraintName("FK__NhanVien__MaChiN__440B1D61");
            });

            modelBuilder.Entity<PhanHoi>(entity =>
            {
                entity.HasKey(e => e.MaPh)
                    .HasName("PK__PhanHoi__2725E7FA3AB7A14C");

                entity.ToTable("PhanHoi");

                entity.Property(e => e.MaPh)
                    .ValueGeneratedNever()
                    .HasColumnName("MaPH");

                entity.Property(e => e.Gio).HasMaxLength(5);

                entity.Property(e => e.MaBn).HasColumnName("MaBN");

                entity.Property(e => e.Ngay).HasMaxLength(10);

                entity.Property(e => e.NoiDung).HasMaxLength(500);

                entity.HasOne(d => d.MaBnNavigation)
                    .WithMany(p => p.PhanHois)
                    .HasForeignKey(d => d.MaBn)
                    .HasConstraintName("FK__PhanHoi__MaBN__48CFD27E");
            });

            modelBuilder.Entity<TaiKhoan>(entity =>
            {
                entity.HasKey(e => e.MaTk)
                    .HasName("PK__TaiKhoan__27250070AFCB866D");

                entity.ToTable("TaiKhoan");

                entity.Property(e => e.MaTk)
                    .ValueGeneratedNever()
                    .HasColumnName("MaTK");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.LoaiNguoiDung).HasMaxLength(10);

                entity.Property(e => e.MatKhau).HasMaxLength(255);

                entity.Property(e => e.SoDienThoai).HasMaxLength(15);

                entity.Property(e => e.Ten).HasMaxLength(100);
            });

            modelBuilder.Entity<Thuoc>(entity =>
            {
                entity.HasKey(e => e.MaThuoc)
                    .HasName("PK__Thuoc__4BB1F620F9B6E34F");

                entity.ToTable("Thuoc");

                entity.Property(e => e.MaThuoc).ValueGeneratedNever();

                entity.Property(e => e.An).HasDefaultValueSql("((0))");

                entity.Property(e => e.DonGiaBan).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DonGiaNhap).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HanSuDung).HasMaxLength(10);

                entity.Property(e => e.NgayNhap).HasMaxLength(10);

                entity.Property(e => e.TenThuoc).HasMaxLength(100);

                entity.HasOne(d => d.MaChiNhanhNavigation)
                    .WithMany(p => p.Thuocs)
                    .HasForeignKey(d => d.MaChiNhanh)
                    .HasConstraintName("FK__Thuoc__MaChiNhan__403A8C7D");
            });

            modelBuilder.Entity<ThuocDaKe>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ThuocDaKe");

                entity.Property(e => e.DonGia)
                    .HasColumnType("decimal(29, 2)")
                    .HasComputedColumnSql("([Gia]*[SoLuong])", false);

                entity.Property(e => e.GhiChu).HasMaxLength(255);

                entity.Property(e => e.Gia).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MaCthsdt).HasColumnName("MaCTHSDT");

                entity.HasOne(d => d.MaCthsdtNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.MaCthsdt)
                    .HasConstraintName("FK__ThuocDaKe__MaCTH__5812160E");

                entity.HasOne(d => d.MaThuocNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.MaThuoc)
                    .HasConstraintName("FK__ThuocDaKe__MaThu__59063A47");
            });

            modelBuilder.Entity<VatTu>(entity =>
            {
                entity.HasKey(e => e.MaVt)
                    .HasName("PK__VatTu__2725103ECC45B274");

                entity.ToTable("VatTu");

                entity.Property(e => e.MaVt)
                    .ValueGeneratedNever()
                    .HasColumnName("MaVT");

                entity.Property(e => e.An).HasDefaultValueSql("((0))");

                entity.Property(e => e.DonGiaNhap).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NgayNhap).HasMaxLength(10);

                entity.Property(e => e.TenVt)
                    .HasMaxLength(100)
                    .HasColumnName("TenVT");

                entity.HasOne(d => d.MaChiNhanhNavigation)
                    .WithMany(p => p.VatTus)
                    .HasForeignKey(d => d.MaChiNhanh)
                    .HasConstraintName("FK__VatTu__MaChiNhan__5CD6CB2B");
            });

            modelBuilder.Entity<VatTuDaSuDung>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("VatTuDaSuDung");

                entity.Property(e => e.NgaySuDung).HasMaxLength(10);

                entity.HasOne(d => d.MaChiNhanhNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.MaChiNhanh)
                    .HasConstraintName("FK__VatTuDaSu__MaChi__5FB337D6");

                entity.HasOne(d => d.MaVatTuNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.MaVatTu)
                    .HasConstraintName("FK__VatTuDaSu__MaVat__5EBF139D");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
