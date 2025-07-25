using ProjectWPF.DTO.Models;
using ProjectWPF.Service.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System;
using System.Windows;

namespace ProjectWPF.StudentManage.ViewModels
{
    public class DiemViewModel : INotifyPropertyChanged
    {
        private readonly IKetQuaService _service;
        private readonly ISinhVienService _sinhVienService;
        private readonly IMonService _monService;
        public ObservableCollection<string> DanhSachSinhVien { get; set; } = new();
        private bool _isRefreshing = false;
        public ObservableCollection<Mon> DanhSachMon { get; set; } = new();
        public ObservableCollection<KetQua> DanhSachDiem { get; set; } = new();
        private string? _selectedSinhVien;
        public string? SelectedSinhVien
        {
            get => _selectedSinhVien;
            set {
    _selectedSinhVien = value;
    SelectedDiem = null;
    Diem = null;
    OnPropertyChanged();
    if (!_isRefreshing) LoadCommand.Execute(null);
}
        }
        private Mon? _selectedMon;
        public Mon? SelectedMon
        {
            get => _selectedMon;
            set {
    _selectedMon = value;
    SelectedDiem = null;
    Diem = null;
    OnPropertyChanged();
    if (!_isRefreshing) LoadCommand.Execute(null);
}
        }
        private int? _diem;
        public int? Diem
        {
            get => _diem;
            set { _diem = value; OnPropertyChanged(); }
        }
        private KetQua? _selectedDiem;
        public KetQua? SelectedDiem
        {
            get => _selectedDiem;
            set
            {
                _selectedDiem = value;
                if (_selectedDiem != null && _selectedDiem.Diem.HasValue)
                    Diem = _selectedDiem.Diem;
                if (_selectedDiem?.MaMh != null)
                {
                    var mon = DanhSachMon.FirstOrDefault(m => m.MaMh == _selectedDiem.MaMh);
                    if (mon != null && !Equals(SelectedMon, mon))
                        SelectedMon = mon;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(Diem));
            }
        }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RefreshCommand { get; }

        private async Task RefreshAllAsync()
        {
            _isRefreshing = true;
            SelectedDiem = null;
            Diem = null;
            SelectedMon = null;
            SelectedSinhVien = null;
            _isRefreshing = false;
            await LoadDiemAsync();
        }
        public ICommand LoadCommand { get; }
        private readonly string _maGv;
        public DiemViewModel(IKetQuaService service, ISinhVienService sinhVienService, IMonService monService, string maGv)
        {
            _service = service;
            _sinhVienService = sinhVienService;
            _monService = monService;
            _maGv = maGv;
            AddCommand = new RelayCommand(async _ => await AddAsync());
            UpdateCommand = new RelayCommand(async _ => await UpdateAsync());
            DeleteCommand = new RelayCommand(async _ => await DeleteAsync());
            RefreshCommand = new RelayCommand(async _ => await RefreshAllAsync());
            LoadCommand = new RelayCommand(async _ => await LoadDiemAsync());
            _ = LoadDanhSachSinhVienAsync();
            _ = LoadDanhSachMonAsync();

            if (SelectedDiem == null)
                SelectedDiem = new KetQua();
        }
        private async Task LoadDanhSachSinhVienAsync()
        {
            var svs = await _sinhVienService.GetAllAsync();
            DanhSachSinhVien.Clear();
            foreach (var sv in svs)
                DanhSachSinhVien.Add(sv.MaSo);
        }
        private async Task LoadDanhSachMonAsync()
        {
            var mons = await _monService.GetAllAsync();
            DanhSachMon.Clear();
            int count = 0;
            foreach (var m in mons)
                if (m.MaGv == _maGv)
                {
                    DanhSachMon.Add(m);
                    count++;
                }
            MessageBox.Show($"Có {count} môn thuộc về giảng viên {_maGv}");
        }
        public async Task LoadDiemAsync()
        {
            DanhSachDiem.Clear();
            var all = await _service.GetAllAsync();
            var maMons = DanhSachMon.Select(m => m.MaMh).ToList();
            if (!string.IsNullOrWhiteSpace(SelectedSinhVien) && SelectedMon != null)
            {
                // Chọn cả sinh viên và môn: chỉ hiển thị điểm của sinh viên đó cho môn đó
                foreach (var d in all)
                    if (d.MaSo == SelectedSinhVien && d.MaMh == SelectedMon.MaMh)
                        DanhSachDiem.Add(d);
            }
            else if (!string.IsNullOrWhiteSpace(SelectedSinhVien))
            {
                // Chỉ chọn sinh viên: hiển thị tất cả điểm của sinh viên đó cho các môn mà giảng viên phụ trách
                foreach (var d in all)
                    if (d.MaSo == SelectedSinhVien && maMons.Contains(d.MaMh))
                        DanhSachDiem.Add(d);
            }
            else if (SelectedMon != null)
            {
                // Chỉ chọn môn: hiển thị tất cả điểm của các sinh viên cho môn đó
                foreach (var d in all)
                    if (d.MaMh == SelectedMon.MaMh)
                        DanhSachDiem.Add(d);
            }
            else
            {
                // Không chọn gì: hiển thị tất cả điểm của các môn mà giảng viên này phụ trách
                foreach (var d in all)
                    if (maMons.Contains(d.MaMh))
                        DanhSachDiem.Add(d);
            }
        }
        private async Task AddAsync()
        {
            MessageBox.Show($"SelectedSinhVien: {SelectedSinhVien}\nSelectedMon: {SelectedMon?.MaMh} - {SelectedMon?.TenMh}\nDiem: {Diem}");
            if (!string.IsNullOrWhiteSpace(SelectedSinhVien) && SelectedMon != null && Diem.HasValue)
            {
                if (Diem < 0 || Diem > 10)
                {
                    MessageBox.Show("Điểm phải từ 0 đến 10!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var kq = new KetQua { MaSo = SelectedSinhVien, MaMh = SelectedMon.MaMh, Diem = Diem };
                await _service.AddAsync(kq);
                await LoadDiemAsync();
                MessageBox.Show("Thêm điểm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn đủ sinh viên, môn học và nhập điểm hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private async Task UpdateAsync()
        {
            if (SelectedDiem != null && Diem.HasValue)
            {
                if (Diem < 0 || Diem > 10)
                {
                    MessageBox.Show("Điểm phải từ 0 đến 10!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                SelectedDiem.Diem = Diem;
                await _service.UpdateAsync(SelectedDiem);
                await LoadDiemAsync();
                MessageBox.Show("Cập nhật điểm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private async Task DeleteAsync()
        {
            if (SelectedDiem != null)
            {
                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa điểm của sinh viên {SelectedDiem.MaSo} cho môn {SelectedDiem.MaMh}?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    await _service.DeleteAsync(SelectedDiem.MaSo, SelectedDiem.MaMh);
                    await LoadDiemAsync();
                    MessageBox.Show("Xóa điểm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
} 