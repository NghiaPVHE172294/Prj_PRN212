using ProjectWPF.DTO.Models;
using ProjectWPF.Service.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using System;
using System.Windows;

namespace ProjectWPF.StudentManage.ViewModels
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        private readonly IAccountService _service;
        public ObservableCollection<Account> Accounts { get; set; } = new();
        private Account? _selectedAccount;
        public Account? SelectedAccount
        {
            get => _selectedAccount;
            set { _selectedAccount = value; OnPropertyChanged(); }
        }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RefreshCommand { get; }
        public AccountViewModel(IAccountService service)
        {
            _service = service;
            AddCommand = new RelayCommand(async _ => await AddAsync());
            UpdateCommand = new RelayCommand(async _ => await UpdateAsync());
            DeleteCommand = new RelayCommand(async _ => await DeleteAsync());
            RefreshCommand = new RelayCommand(async _ => await LoadDataAsync());
            _ = LoadDataAsync();
            SelectedAccount = new Account();
        }
        public async Task LoadDataAsync()
        {
            var list = await _service.GetAllAsync();
            Accounts.Clear();
            foreach (var acc in list)
                Accounts.Add(acc);
        }
        private async Task AddAsync()
        {
            if (SelectedAccount != null)
            {
                if (string.IsNullOrWhiteSpace(SelectedAccount.MaSo) || string.IsNullOrWhiteSpace(SelectedAccount.Username) || string.IsNullOrWhiteSpace(SelectedAccount.Password) || string.IsNullOrWhiteSpace(SelectedAccount.Role))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var existed = Accounts.FirstOrDefault(a => a.MaSo == SelectedAccount.MaSo || a.Username == SelectedAccount.Username);
                if (existed != null)
                {
                    MessageBox.Show("Mã số hoặc Username đã tồn tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                try
                {
                    await _service.AddAsync(SelectedAccount);
                    await LoadDataAsync();
                    MessageBox.Show("Thêm tài khoản thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    SelectedAccount = new Account();
                    OnPropertyChanged(nameof(SelectedAccount));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thêm tài khoản: {ex.Message}\n{ex.InnerException?.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private async Task UpdateAsync()
        {
            if (SelectedAccount != null)
            {
                if (string.IsNullOrWhiteSpace(SelectedAccount.MaSo) || string.IsNullOrWhiteSpace(SelectedAccount.Username) || string.IsNullOrWhiteSpace(SelectedAccount.Password) || string.IsNullOrWhiteSpace(SelectedAccount.Role))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                try
                {
                    await _service.UpdateAsync(SelectedAccount);
                    await LoadDataAsync();
                    MessageBox.Show("Cập nhật tài khoản thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi cập nhật tài khoản: {ex.Message}\n{ex.InnerException?.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private async Task DeleteAsync()
        {
            if (SelectedAccount != null)
            {
                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa tài khoản {SelectedAccount.Username}?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _service.DeleteAsync(SelectedAccount.MaSo);
                        await LoadDataAsync();
                        MessageBox.Show("Xóa tài khoản thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa tài khoản: {ex.Message}\n{ex.InnerException?.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
} 