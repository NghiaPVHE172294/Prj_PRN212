using ProjectWPF.StudentManage.ViewModels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ProjectWPF.StudentManage
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                // Lấy password từ PasswordBox
                vm.Password = PasswordBox.Password;

                // Gọi trực tiếp hàm login async (đừng dùng Task.Run)
                await vm.LoginAsync();

                // Nếu login thành công -> đóng cửa sổ
                if (vm.IsLoginSuccess)
                {
                    DialogResult = true;
                    Close();
                }
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
