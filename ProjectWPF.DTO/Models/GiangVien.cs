using System;
using System.Collections.Generic;

namespace ProjectWPF.DTO.Models;

public partial class GiangVien
{
    public string MaSo { get; set; } = null!;

    public string? HoTen { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public bool? GioiTinh { get; set; }

    public string? DiaChi { get; set; }

    public string? DienThoai { get; set; }

    public string? MaKhoa { get; set; }

    public virtual Khoa? MaKhoaNavigation { get; set; }

    public virtual ICollection<Mon> Mons { get; set; } = new List<Mon>();
    // ✅ Giới tính dạng chữ để binding DataGrid
    public string GioiTinhText => GioiTinh == true ? "Nam"
                              : GioiTinh == false ? "Nữ"
                              : "";

    // ✅ Ngày sinh hiển thị đẹp
    public string NgaySinhText => NgaySinh.HasValue
        ? NgaySinh.Value.ToString("dd/MM/yyyy")
        : "";
}
