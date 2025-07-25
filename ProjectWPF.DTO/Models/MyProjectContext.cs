using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ProjectWPF.DTO.Models;

public partial class MyProjectContext : DbContext
{
    public MyProjectContext()
    {
    }

    public MyProjectContext(DbContextOptions<MyProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<GiangVien> GiangViens { get; set; }

    public virtual DbSet<KetQua> KetQuas { get; set; }

    public virtual DbSet<Khoa> Khoas { get; set; }

    public virtual DbSet<Mon> Mons { get; set; }

    public virtual DbSet<SinhVien> SinhViens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.MaSo);

            entity.ToTable("Account");

            entity.Property(e => e.MaSo).HasMaxLength(20);
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .HasColumnName("password");
            entity.Property(e => e.Role).HasMaxLength(10);
            entity.Property(e => e.Username)
                .HasMaxLength(30)
                .HasColumnName("username");
        });

        modelBuilder.Entity<GiangVien>(entity =>
        {
            entity.HasKey(e => e.MaSo).HasName("PK__GiangVie__2725087DDC4C6548");

            entity.ToTable("GiangVien");

            entity.Property(e => e.MaSo).HasMaxLength(20);
            entity.Property(e => e.DiaChi).HasMaxLength(50);
            entity.Property(e => e.DienThoai)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.MaKhoa)
                .HasMaxLength(6)
                .IsFixedLength();

            entity.HasOne(d => d.MaKhoaNavigation).WithMany(p => p.GiangViens)
                .HasForeignKey(d => d.MaKhoa)
                .HasConstraintName("FK__GiangVien__MaKho__300424B4");
        });

        modelBuilder.Entity<KetQua>(entity =>
        {
            entity.HasKey(e => new { e.MaSo, e.MaMh });
            entity.ToTable("KetQua");

            entity.Property(e => e.MaMh)
                .HasMaxLength(6)
                .IsFixedLength()
                .HasColumnName("MaMH");
            entity.Property(e => e.MaSo).HasMaxLength(20);

            entity.HasOne(d => d.MaMhNavigation).WithMany()
                .HasForeignKey(d => d.MaMh)
                .HasConstraintName("MaMH_in_Mon");

            entity.HasOne(d => d.MaSoNavigation).WithMany()
                .HasForeignKey(d => d.MaSo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MaSo_in_SinhVien");
        });

        modelBuilder.Entity<Khoa>(entity =>
        {
            entity.HasKey(e => e.MaKhoa).HasName("PK__Khoa__65390405BDA57D38");

            entity.ToTable("Khoa");

            entity.Property(e => e.MaKhoa)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.TenKhoa)
                .HasMaxLength(30)
                .IsFixedLength();
        });

        modelBuilder.Entity<Mon>(entity =>
        {
            entity.HasKey(e => e.MaMh).HasName("PK__Mon__2725DFD958DC2E6A");

            entity.ToTable("Mon");

            entity.Property(e => e.MaMh)
                .HasMaxLength(6)
                .IsFixedLength()
                .HasColumnName("MaMH");
            entity.Property(e => e.MaGv)
                .HasMaxLength(20)
                .HasColumnName("MaGV");
            entity.Property(e => e.TenMh)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("TenMH");

            entity.HasOne(d => d.MaGvNavigation).WithMany(p => p.Mons)
                .HasForeignKey(d => d.MaGv)
                .HasConstraintName("FK_Mon_GiangVien");
        });

        modelBuilder.Entity<SinhVien>(entity =>
        {
            entity.HasKey(e => e.MaSo).HasName("PK__SinhVien__2725087DF2C18BAD");

            entity.ToTable("SinhVien");

            entity.Property(e => e.MaSo).HasMaxLength(20);
            entity.Property(e => e.DiaChi).HasMaxLength(50);
            entity.Property(e => e.DienThoai)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.MaKhoa)
                .HasMaxLength(6)
                .IsFixedLength();

            entity.HasOne(d => d.MaKhoaNavigation).WithMany(p => p.SinhViens)
                .HasForeignKey(d => d.MaKhoa)
                .HasConstraintName("MaKhoa_in_Khoa");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
