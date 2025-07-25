using ProjectWPF.StudentManage.ViewModels;
using ProjectWPF.Service.Services;
using System.Windows;
using System;

namespace ProjectWPF.StudentManage
{
    public partial class MonView : Window
    {
        public MonView(IMonService monService, IGiangVienService giangVienService)
        {
            try
            {
                InitializeComponent();
                DataContext = new MonViewModel(monService, giangVienService);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo MonView: " + ex.ToString());
                throw;
            }
        }
    }
} 