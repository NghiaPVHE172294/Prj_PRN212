using ProjectWPF.DTO.Models;
using ProjectWPF.Service.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using System;
using ProjectWPF.StudentManage.Services;
using Microsoft.Win32;
using System.Windows;

namespace ProjectWPF.StudentManage.ViewModels
{
    public class SinhVienViewModel : INotifyPropertyChanged
    {
        private readonly ISinhVienService _service;
        private readonly IAccountService _accountService;

        public ObservableCollection<SinhVien> SinhViens { get; set; } = new();

        private SinhVien? _selectedSinhVien;
        public SinhVien? SelectedSinhVien
        {
            get => _selectedSinhVien;
            set
            {
                _selectedSinhVien = value;
                // Đảm bảo mã số đang chọn luôn có trong danh sách để ComboBox hiển thị đúng
                if (_selectedSinhVien != null && !string.IsNullOrEmpty(_selectedSinhVien.MaSo) && !DanhSachMaSoAccount.Contains(_selectedSinhVien.MaSo))
                {
                    DanhSachMaSoAccount.Add(_selectedSinhVien.MaSo);
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(NgaySinhDateTime));
            }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); }
        }

        private string? _selectedKhoa;
        public string? SelectedKhoa
        {
            get => _selectedKhoa;
            set { _selectedKhoa = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> DanhSachKhoa { get; set; } = new();
        public ObservableCollection<string> DanhSachMaSoAccount { get; set; } = new();

        private DateTime? _ngaySinhDateTime;
        public DateTime? NgaySinhDateTime
        {
            get => SelectedSinhVien?.NgaySinh != null ? SelectedSinhVien.NgaySinh.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null;
            set
            {
                if (SelectedSinhVien != null)
                {
                    SelectedSinhVien.NgaySinh = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ExportExcelCommand { get; }

        private readonly ExcelExportService _excelExportService = new();

        public SinhVienViewModel(ISinhVienService service, IKhoaService khoaService, IAccountService accountService)
        {
            _service = service;
            _accountService = accountService;

            AddCommand = new RelayCommand(async _ => await AddAsync());
            UpdateCommand = new RelayCommand(async _ => await UpdateAsync());
            DeleteCommand = new RelayCommand(async _ => await DeleteAsync());
            RefreshCommand = new RelayCommand(async _ => await LoadDataAsync());
            SearchCommand = new RelayCommand(async _ => await SearchAsync());
            ExportExcelCommand = new RelayCommand(_ => ExportExcel());

            _ = LoadDataAsync();
            _ = LoadDanhSachKhoaAsync(khoaService);
            _ = LoadDanhSachMaSoAccountAsync();

            SelectedSinhVien = new SinhVien(); // Gán mặc định khi khởi tạo
        }

        public async Task LoadDataAsync()
        {
            var list = await _service.GetAllAsync();
            SinhViens.Clear();
            foreach (var sv in list)
                SinhViens.Add(sv);
            await LoadDanhSachMaSoAccountAsync(); // Cập nhật lại danh sách mã số khả dụng khi load lại
        }

        private async Task LoadDanhSachKhoaAsync(IKhoaService khoaService)
        {
            var khoas = await khoaService.GetAllAsync();
            DanhSachKhoa.Clear();
            DanhSachKhoa.Add(""); // Tùy chọn tất cả
            foreach (var k in khoas)
                DanhSachKhoa.Add(k.MaKhoa);
        }

        private async Task LoadDanhSachMaSoAccountAsync()
        {
            var allAccounts = await _accountService.GetAllAsync();
            var allSinhViens = await _service.GetAllAsync();
            var usedMaSo = allSinhViens.Select(sv => sv.MaSo).ToHashSet();
            DanhSachMaSoAccount.Clear();
            foreach (var acc in allAccounts)
            {
                if (acc.Role == "SV" && !usedMaSo.Contains(acc.MaSo))
                    DanhSachMaSoAccount.Add(acc.MaSo);
            }
        }

        private async Task SearchAsync()
        {
            var all = await _service.GetAllAsync();
            var filtered = all;

            if (!string.IsNullOrWhiteSpace(SearchText))
                filtered = filtered.Where(sv => sv.HoTen != null && sv.HoTen.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(SelectedKhoa))
                filtered = filtered.Where(sv => sv.MaKhoa == SelectedKhoa);

            SinhViens.Clear();
            foreach (var sv in filtered)
                SinhViens.Add(sv);
        }

        private async Task AddAsync()
        {
            if (SelectedSinhVien != null)
            {
                // Kiểm tra các trường bắt buộc
                if (string.IsNullOrWhiteSpace(SelectedSinhVien.MaSo))
                {
                    MessageBox.Show("Mã số sinh viên không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!DanhSachMaSoAccount.Contains(SelectedSinhVien.MaSo))
                {
                    MessageBox.Show("Mã số này không hợp lệ hoặc đã có sinh viên!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(SelectedSinhVien.HoTen))
                {
                    MessageBox.Show("Tên sinh viên không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(SelectedSinhVien.MaKhoa))
                {
                    MessageBox.Show("Mã khoa không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Kiểm tra trùng mã số sinh viên trước khi gọi Service
                var existed = await _service.GetByIdAsync(SelectedSinhVien.MaSo);
                if (existed != null)
                {
                    MessageBox.Show($"Mã số sinh viên '{SelectedSinhVien.MaSo}' đã tồn tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    await _service.AddAsync(SelectedSinhVien);

                    // 🛠 GỌI LẠI object mới từ DB, đảm bảo đầy đủ dữ liệu
                    var svMoi = await _service.GetByIdAsync(SelectedSinhVien.MaSo);

                    // Nếu tìm được thì add thủ công
                    if (svMoi != null)
                    {
                        SinhViens.Add(svMoi);
                    }

                    // Hoặc gọi lại toàn bộ danh sách nếu thích
                    // await LoadDataAsync();

                    MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    SelectedSinhVien = new SinhVien();
                    OnPropertyChanged(nameof(SelectedSinhVien));


                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thêm sinh viên: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task UpdateAsync()
        {
            if (SelectedSinhVien != null)
            {
                if (string.IsNullOrWhiteSpace(SelectedSinhVien.HoTen))
                {
                    MessageBox.Show("Tên sinh viên không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                await _service.UpdateAsync(SelectedSinhVien);
                await LoadDataAsync();
                MessageBox.Show("Cập nhật sinh viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async Task DeleteAsync()
        {
            if (SelectedSinhVien != null)
            {
                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa sinh viên {SelectedSinhVien.HoTen}?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    await _service.DeleteAsync(SelectedSinhVien.MaSo);
                    await LoadDataAsync();
                    MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void ExportExcel()
        {
            var dialog = new SaveFileDialog { Filter = "Excel Files|*.xlsx", FileName = "DanhSachSinhVien.xlsx" };
            if (dialog.ShowDialog() == true)
            {
                _excelExportService.ExportToExcel(SinhViens, dialog.FileName);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
