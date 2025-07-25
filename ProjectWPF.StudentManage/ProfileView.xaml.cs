using ProjectWPF.Service.Services;
using ProjectWPF.StudentManage.ViewModels;
using System.Windows;

namespace ProjectWPF.StudentManage
{
    public partial class ProfileView : Window
    {
        public ProfileView(string role, string maSo, IGiangVienService? gvService, ISinhVienService? svService)
        {
            InitializeComponent();
            DataContext = new ProfileViewModel(role, maSo, gvService, svService);
        }
    }
}
