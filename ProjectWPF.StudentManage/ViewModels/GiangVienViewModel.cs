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
    public class GiangVienViewModel : INotifyPropertyChanged
    {
        private readonly IGiangVienService _service;
        private readonly IAccountService _accountService;

        public ObservableCollection<GiangVien> GiangViens { get; set; } = new();
        public ObservableCollection<string> DanhSachKhoa { get; set; } = new();
        // Danh sách môn không còn cần thiết ở ViewModel này vì GiangVien không còn property MaMon
        // Nếu muốn hiển thị các môn mà giảng viên dạy, có thể dùng property dưới đây:
        public ObservableCollection<Mon> MonsOfSelectedGiangVien { get; set; } = new();
        public ObservableCollection<string> DanhSachMaSoAccount { get; set; } = new();

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand SearchCommand { get; }

        public GiangVienViewModel(IGiangVienService service, IKhoaService khoaService, IMonService monService, IAccountService accountService)
        {
            _service = service;
            _accountService = accountService;

            AddCommand = new RelayCommand(async _ => await AddAsync());
            UpdateCommand = new RelayCommand(async _ => await UpdateAsync());
            DeleteCommand = new RelayCommand(async _ => await DeleteAsync());
            RefreshCommand = new RelayCommand(async _ => await LoadDataAsync());
            SearchCommand = new RelayCommand(async _ => await SearchAsync());

            _ = LoadDataAsync();
            _ = LoadDanhSachKhoaAsync(khoaService);
            _ = LoadDanhSachMaSoAccountAsync();

            // Khởi tạo object mới để có thể nhập và add ngay khi mở form
            if (SelectedGiangVien == null)
                SelectedGiangVien = new GiangVien();
        }

        private GiangVien? _selectedGiangVien;
        public GiangVien? SelectedGiangVien
        {
            get => _selectedGiangVien;
            set
            {
                _selectedGiangVien = value;
                // Đảm bảo mã số đang chọn luôn có trong danh sách để ComboBox hiển thị đúng
                if (_selectedGiangVien != null && !string.IsNullOrEmpty(_selectedGiangVien.MaSo) && !DanhSachMaSoAccount.Contains(_selectedGiangVien.MaSo))
                {
                    DanhSachMaSoAccount.Add(_selectedGiangVien.MaSo);
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(NgaySinhDateTime));
            }
        }

        // Đồng bộ DatePicker ⇄ DateOnly
        public DateTime? NgaySinhDateTime
        {
            get => SelectedGiangVien?.NgaySinh?.ToDateTime(TimeOnly.MinValue);
            set
            {
                if (SelectedGiangVien != null)
                {
                    SelectedGiangVien.NgaySinh = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(NgaySinhDateTime));
                }
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

        // Đã loại bỏ SelectedMon vì model GiangVien không còn property MaMon

        public async Task LoadDataAsync()
        {
            var list = await _service.GetAllAsync();
            GiangViens.Clear();
            foreach (var gv in list)
                GiangViens.Add(gv);

            await LoadDanhSachMaSoAccountAsync();
        }

        private async Task AddAsync()
        {
            if (SelectedGiangVien != null)
            {
                if (string.IsNullOrWhiteSpace(SelectedGiangVien.MaSo))
                {
                    MessageBox.Show("Mã số giảng viên không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!DanhSachMaSoAccount.Contains(SelectedGiangVien.MaSo))
                {
                    MessageBox.Show("Mã số này không hợp lệ hoặc đã có giảng viên!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(SelectedGiangVien.HoTen))
                {
                    MessageBox.Show("Tên giảng viên không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(SelectedGiangVien.MaKhoa))
                {
                    MessageBox.Show("Mã khoa không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    await _service.AddAsync(SelectedGiangVien);
                    await LoadDataAsync();
                    MessageBox.Show("Thêm giảng viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    SelectedGiangVien = new GiangVien();
                    OnPropertyChanged(nameof(SelectedGiangVien));
                    await LoadDanhSachMaSoAccountAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thêm giảng viên: {ex.Message}\n{ex.InnerException?.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task UpdateAsync()
        {
            if (SelectedGiangVien != null)
            {
                if (string.IsNullOrWhiteSpace(SelectedGiangVien.HoTen))
                {
                    MessageBox.Show("Tên giảng viên không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    await _service.UpdateAsync(SelectedGiangVien);
                    await LoadDataAsync();
                    MessageBox.Show("Cập nhật giảng viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi cập nhật giảng viên: {ex.Message}\n{ex.InnerException?.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task DeleteAsync()
        {
            if (SelectedGiangVien != null)
            {
                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa giảng viên {SelectedGiangVien.HoTen}?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _service.DeleteAsync(SelectedGiangVien.MaSo);
                        await LoadDataAsync();
                        MessageBox.Show("Xóa giảng viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        SelectedGiangVien = new GiangVien();
                        OnPropertyChanged(nameof(SelectedGiangVien));
                        await LoadDanhSachMaSoAccountAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa giảng viên: {ex.Message}\n{ex.InnerException?.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private async Task LoadDanhSachKhoaAsync(IKhoaService khoaService)
        {
            var khoas = await khoaService.GetAllAsync();
            DanhSachKhoa.Clear();
            DanhSachKhoa.Add("");
            foreach (var k in khoas)
                DanhSachKhoa.Add(k.MaKhoa);
        }

        // Đã loại bỏ hàm load danh sách môn vì không còn property MaMon

        private async Task LoadDanhSachMaSoAccountAsync()
        {
            var allAccounts = await _accountService.GetAllAsync();
            var allGiangViens = await _service.GetAllAsync();
            var usedMaSo = allGiangViens.Select(gv => gv.MaSo).ToHashSet();

            DanhSachMaSoAccount.Clear();
            foreach (var acc in allAccounts)
            {
                if (acc.Role == "GV" && !usedMaSo.Contains(acc.MaSo))
                    DanhSachMaSoAccount.Add(acc.MaSo);
            }
        }

        private async Task SearchAsync()
        {
            var all = await _service.GetAllAsync();
            var filtered = all.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
                filtered = filtered.Where(gv => gv.HoTen != null && gv.HoTen.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(SelectedKhoa))
                filtered = filtered.Where(gv => gv.MaKhoa == SelectedKhoa);
            // Đã loại bỏ filter theo SelectedMon vì model GiangVien không còn property MaMon

            GiangViens.Clear();
            foreach (var gv in filtered)
                GiangViens.Add(gv);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
