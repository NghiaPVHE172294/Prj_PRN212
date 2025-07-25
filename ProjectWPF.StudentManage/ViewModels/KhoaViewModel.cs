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
    public class KhoaViewModel : INotifyPropertyChanged
    {
        private readonly IKhoaService _service;
        public ObservableCollection<Khoa> Khoas { get; set; } = new();
        private Khoa? _selectedKhoa;
        public Khoa? SelectedKhoa
        {
            get => _selectedKhoa;
            set { _selectedKhoa = value; OnPropertyChanged(); }
        }
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
        public KhoaViewModel(IKhoaService service)
        {
            _service = service;
            AddCommand = new RelayCommand(async _ => await AddAsync());
            UpdateCommand = new RelayCommand(async _ => await UpdateAsync());
            DeleteCommand = new RelayCommand(async _ => await DeleteAsync());
            RefreshCommand = new RelayCommand(async _ => await LoadDataAsync());
            SearchCommand = new RelayCommand(async _ => await SearchAsync());
            _ = LoadDataAsync();

            // Đảm bảo có thể add mới ngay khi mở form
            if (SelectedKhoa == null)
                SelectedKhoa = new Khoa();
        }
        public async Task LoadDataAsync()
        {
            var list = await _service.GetAllAsync();
            Khoas.Clear();
            foreach (var k in list)
                Khoas.Add(k);
        }
        private async Task AddAsync()
        {
            if (SelectedKhoa != null)
            {
                if (string.IsNullOrWhiteSpace(SelectedKhoa.TenKhoa))
                {
                    MessageBox.Show("Tên khoa không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                await _service.AddAsync(SelectedKhoa);
                await LoadDataAsync();
                MessageBox.Show("Thêm khoa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                SelectedKhoa = new Khoa();
                OnPropertyChanged(nameof(SelectedKhoa));
            }
        }
        private async Task UpdateAsync()
        {
            if (SelectedKhoa != null)
            {
                if (string.IsNullOrWhiteSpace(SelectedKhoa.TenKhoa))
                {
                    MessageBox.Show("Tên khoa không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                await _service.UpdateAsync(SelectedKhoa);
                await LoadDataAsync();
                MessageBox.Show("Cập nhật khoa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private async Task DeleteAsync()
        {
            if (SelectedKhoa != null)
            {
                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa khoa {SelectedKhoa.TenKhoa}?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    await _service.DeleteAsync(SelectedKhoa.MaKhoa);
                    await LoadDataAsync();
                    MessageBox.Show("Xóa khoa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private async Task SearchAsync()
        {
            var all = await _service.GetAllAsync();
            var filtered = all;
            if (!string.IsNullOrWhiteSpace(SearchText))
                filtered = filtered.Where(k => k.TenKhoa != null && k.TenKhoa.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            Khoas.Clear();
            foreach (var k in filtered)
                Khoas.Add(k);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
} 