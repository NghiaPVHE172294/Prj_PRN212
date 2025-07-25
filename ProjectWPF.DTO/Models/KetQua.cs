using System;
using System.Collections.Generic;

namespace ProjectWPF.DTO.Models;

public partial class KetQua
{
    public string MaSo { get; set; } = null!;

    public string? MaMh { get; set; }

    public int? Diem { get; set; }

    public virtual Mon? MaMhNavigation { get; set; }

    public virtual SinhVien MaSoNavigation { get; set; } = null!;
}
