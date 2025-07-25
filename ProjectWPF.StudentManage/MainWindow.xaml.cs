using Microsoft.Extensions.DependencyInjection;
using ProjectWPF.Service.Services;
using System.Windows;

namespace ProjectWPF.StudentManage
{
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider _provider;
        public string? UserRole { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            _provider = App.AppHost.Services;
        }

        private void SinhVien_Click(object sender, RoutedEventArgs e)
        {
            var win = _provider.GetRequiredService<SinhVienView>();
            win.Show();
        }

        private void GiangVien_Click(object sender, RoutedEventArgs e)
        {
            var win = _provider.GetRequiredService<GiangVienView>();
            win.Show();
        }

        private void Khoa_Click(object sender, RoutedEventArgs e)
        {
            var win = _provider.GetRequiredService<KhoaView>();
            win.Show();
        }

        private void Mon_Click(object sender, RoutedEventArgs e)
        {
            var win = _provider.GetRequiredService<MonView>();
            win.Show();
        }

        private void about_Click(object sender, RoutedEventArgs e)
        {
            var win = _provider.GetRequiredService<AboutView>();
            win.Show();
        }

        private void Account_Click(object sender, RoutedEventArgs e)
        {
            var accountService = _provider.GetRequiredService<IAccountService>();
            var win = new AccountView(accountService);
            win.Show();
        }

        private void DangXuat_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.Close(); // chỉ cần đóng window hiện tại
            }
        }
    }
}
