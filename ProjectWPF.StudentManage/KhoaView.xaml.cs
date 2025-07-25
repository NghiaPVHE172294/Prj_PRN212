using ProjectWPF.StudentManage.ViewModels;
using ProjectWPF.Service.Services;
using System.Windows;
using System;

namespace ProjectWPF.StudentManage
{
    public partial class KhoaView : Window
    {
        public KhoaView(IKhoaService khoaService)
        {
            try
            {
                InitializeComponent();
                DataContext = new KhoaViewModel(khoaService);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo KhoaView: " + ex.ToString());
                throw;
            }
        }
    }
} 