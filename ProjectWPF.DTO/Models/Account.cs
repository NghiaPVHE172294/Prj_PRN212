using System;
using System.Collections.Generic;

namespace ProjectWPF.DTO.Models;

public partial class Account
{
    public string MaSo { get; set; } = null!;

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string Role { get; set; } = null!;
}
