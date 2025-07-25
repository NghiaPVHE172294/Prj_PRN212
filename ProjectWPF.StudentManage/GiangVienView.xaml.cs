using ProjectWPF.StudentManage.ViewModels;
using ProjectWPF.Service.Services;
using System.Windows;
using System;

namespace ProjectWPF.StudentManage
{
    public partial class GiangVienView : Window
    {
        public GiangVienView(IGiangVienService giangVienService, IKhoaService khoaService, IMonService monService, IAccountService accountService)
        {
            try
            {
                InitializeComponent();
                DataContext = new GiangVienViewModel(giangVienService, khoaService, monService, accountService);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo GiangVienView: " + ex.ToString());
                throw;
            }
        }
    }
} 