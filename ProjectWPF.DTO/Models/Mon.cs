using System;
using System.Collections.Generic;

namespace ProjectWPF.DTO.Models;

public partial class Mon
{
    public string MaMh { get; set; } = null!;

    public string? TenMh { get; set; }

    public int? SoTiet { get; set; }

    public string? MaGv { get; set; }

    public virtual GiangVien? MaGvNavigation { get; set; }

    // Helper cho DataGrid: hiển thị tên giảng viên
    public string TenGiangVien => MaGvNavigation?.HoTen ?? string.Empty;
}
