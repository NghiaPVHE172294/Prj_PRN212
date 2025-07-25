using Microsoft.Extensions.DependencyInjection;
using ProjectWPF.Service.Services;
using ProjectWPF.StudentManage.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectWPF.StudentManage
{
    /// <summary>
    /// Interaction logic for StuHome.xaml
    /// </summary>
    public partial class StuHome : Window
    {
        private readonly IServiceProvider _provider;
        private readonly string _maSo;
        public StuHome(string maSo)
        {
            InitializeComponent();
            _provider = App.AppHost.Services;
            _maSo = maSo;
        }
        private void DangXuat_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.Close(); // chỉ cần đóng window hiện tại
            }
        }
        private void XemThongTin_Click(object sender, RoutedEventArgs e)
        {
            var svService = _provider.GetRequiredService<ISinhVienService>();
            var win = new ProfileView("SV", _maSo, null, svService);
            win.ShowDialog();
        }
        private void XemDiem_Click(object sender, RoutedEventArgs e)
        {
            var ketQuaService = _provider.GetRequiredService<IKetQuaService>();
            var win = new StudentScoreView();
            win.DataContext = new StudentScoreViewModel(_maSo, ketQuaService);
            win.ShowDialog();
        }
    }
}
