using ProjectWPF.Service.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace ProjectWPF.StudentManage.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IAccountService _accountService;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string? _role;
        private bool _isLoginSuccess;
        private string? _maSo;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }
        public string? Role
        {
            get => _role;
            set { _role = value; OnPropertyChanged(); }
        }
        public bool IsLoginSuccess
        {
            get => _isLoginSuccess;
            set { _isLoginSuccess = value; OnPropertyChanged(); }
        }
        public string? MaSo
        {
            get => _maSo;
            set { _maSo = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(IAccountService accountService)
        {
            _accountService = accountService;
            LoginCommand = new RelayCommand(async _ => await LoginAsync());
        }

        public async Task LoginAsync()
        {
            var account = await _accountService.LoginAsync(Username, Password);
            if (account != null)
            {
                Role = account.Role;
                MaSo = account.MaSo;
                IsLoginSuccess = true;
            }
            else
            {
                IsLoginSuccess = false;
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
} 