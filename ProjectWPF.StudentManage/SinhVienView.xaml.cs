using ProjectWPF.StudentManage.ViewModels;
using ProjectWPF.Service.Services;
using System.Windows;
using System;

namespace ProjectWPF.StudentManage
{
    public partial class SinhVienView : Window
    {
        public SinhVienView(ISinhVienService sinhVienService, IKhoaService khoaService, IAccountService accountService)
        {
            try
            {
                InitializeComponent();
                DataContext = new SinhVienViewModel(sinhVienService, khoaService, accountService);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo SinhVienView: " + ex.ToString());
                throw;
            }
        }
    }
} 