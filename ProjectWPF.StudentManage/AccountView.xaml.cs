using ProjectWPF.StudentManage.ViewModels;
using ProjectWPF.Service.Services;
using System.Windows;

namespace ProjectWPF.StudentManage
{
    public partial class AccountView : Window
    {
        public AccountView(IAccountService accountService)
        {
            InitializeComponent();
            var vm = new AccountViewModel(accountService);
            DataContext = vm;
            PasswordBox.PasswordChanged += (s, e) =>
            {
                if (vm.SelectedAccount != null)
                    vm.SelectedAccount.Password = PasswordBox.Password;
            };
        }
    }
} 