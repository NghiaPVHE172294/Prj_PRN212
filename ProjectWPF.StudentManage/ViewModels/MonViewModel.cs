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

    public class MonViewModel : INotifyPropertyChanged
    {
        private readonly IMonService _service;
        private readonly IGiangVienService _giangVienService;
        public ObservableCollection<GiangVien> DanhSachGiangVien { get; set; } = new();
        public ObservableCollection<Mon> Mons { get; set; } = new();
        private Mon? _selectedMon;
        public Mon? SelectedMon
        {
            get => _selectedMon;
            set { _selectedMon = value; OnPropertyChanged(); OnPropertyChanged(nameof(TenGiangVien)); }
        }

        // Property hỗ trợ hiển thị tên giảng viên dạy môn
        public string? TenGiangVien => SelectedMon?.MaGvNavigation?.HoTen;

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); }
        }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand SearchCommand { get; }
        public MonViewModel(IMonService service, IGiangVienService giangVienService)
        {
            _service = service;
            _giangVienService = giangVienService;
            AddCommand = new RelayCommand(async _ => await AddAsync());
            UpdateCommand = new RelayCommand(async _ => await UpdateAsync());
            DeleteCommand = new RelayCommand(async _ => await DeleteAsync());
            RefreshCommand = new RelayCommand(async _ => await LoadDataAsync());
            SearchCommand = new RelayCommand(async _ => await SearchAsync());
            _ = LoadDataAsync();
            _ = LoadDanhSachGiangVienAsync();

            if (SelectedMon == null)
                SelectedMon = new Mon();
        }
        public async Task LoadDataAsync()
        {
            var list = await _service.GetAllAsync();
            Mons.Clear();
            foreach (var m in list)
                Mons.Add(m);
        }

        public async Task LoadDanhSachGiangVienAsync()
        {
            var list = await _giangVienService.GetAllAsync();
            DanhSachGiangVien.Clear();
            foreach (var gv in list)
                DanhSachGiangVien.Add(gv);
        }
        private async Task AddAsync()
        {
            if (SelectedMon != null)
            {
                if (string.IsNullOrWhiteSpace(SelectedMon.TenMh))
                {
                    MessageBox.Show("Tên môn học không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                await _service.AddAsync(SelectedMon);
                await LoadDataAsync();
                MessageBox.Show("Thêm môn học thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private async Task UpdateAsync()
        {
            if (SelectedMon != null)
            {
                if (string.IsNullOrWhiteSpace(SelectedMon.TenMh))
                {
                    MessageBox.Show("Tên môn học không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                await _service.UpdateAsync(SelectedMon);
                await LoadDataAsync();
                MessageBox.Show("Cập nhật môn học thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private async Task DeleteAsync()
        {
            if (SelectedMon != null)
            {
                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa môn học {SelectedMon.TenMh}?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    await _service.DeleteAsync(SelectedMon.MaMh);
                    await LoadDataAsync();
                    MessageBox.Show("Xóa môn học thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private async Task SearchAsync()
        {
            var all = await _service.GetAllAsync();
            var filtered = all;
            if (!string.IsNullOrWhiteSpace(SearchText))
                filtered = filtered.Where(m => m.TenMh != null && m.TenMh.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            Mons.Clear();
            foreach (var m in filtered)
                Mons.Add(m);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
} 