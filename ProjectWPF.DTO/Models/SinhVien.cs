using System;

namespace ProjectWPF.DTO.Models;

public partial class SinhVien : System.ComponentModel.INotifyPropertyChanged
{
    private string _maSo = string.Empty;
public string MaSo
{
    get => _maSo;
    set
    {
        if (_maSo != value)
        {
            _maSo = value;
            OnPropertyChanged(nameof(MaSo));
        }
    }
}

    public string? HoTen { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public bool? GioiTinh { get; set; }

    public string? DiaChi { get; set; }

    public string? DienThoai { get; set; }

    public string? MaKhoa { get; set; }

    public virtual Khoa? MaKhoaNavigation { get; set; }

    // ✅ Giới tính dạng chữ để binding DataGrid
    public string GioiTinhText => GioiTinh == true ? "Nam"
                              : GioiTinh == false ? "Nữ"
                              : "";

    // ✅ Ngày sinh hiển thị đẹp
    public string NgaySinhText => NgaySinh.HasValue
        ? NgaySinh.Value.ToString("dd/MM/yyyy")
        : "";

    public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
    }
}
