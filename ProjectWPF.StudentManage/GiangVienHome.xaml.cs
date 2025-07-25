using Microsoft.Extensions.DependencyInjection;
using ProjectWPF.DTO.Models;
using ProjectWPF.Service.Services;
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
    /// Interaction logic for GiangVienHome.xaml
    /// </summary>
    public partial class GiangVienHome : Window
    {
        private readonly IServiceProvider _provider;
        private readonly string _maGv;
        public GiangVienHome(string maGv)
        {
            InitializeComponent();
            _provider = App.AppHost.Services;
            _maGv = maGv;
        }
        private void DangXuat_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.Close(); // chỉ cần đóng window hiện tại
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Lấy các service cần thiết từ DI container
            var ketQuaService = _provider.GetRequiredService<IKetQuaService>();
            var sinhVienService = _provider.GetRequiredService<ISinhVienService>();
            var monService = _provider.GetRequiredService<IMonService>();
            var win = new DiemView(ketQuaService, sinhVienService, monService, _maGv);
            win.Show();
        }

        private void XemMon_Click(object sender, RoutedEventArgs e)
        {
            var monService = _provider.GetRequiredService<IMonService>();
            var win = new MonForLecturerView(monService, _maGv);
            win.ShowDialog();
        }

        private void XemThongTin_Click(object sender, RoutedEventArgs e)
        {
            var gvService = _provider.GetRequiredService<IGiangVienService>();
            var win = new ProfileView("GV", _maGv, gvService, null);
            win.ShowDialog();
        }
    }
}
