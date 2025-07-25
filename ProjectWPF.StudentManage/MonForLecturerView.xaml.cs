using ProjectWPF.Service.Services;
using ProjectWPF.StudentManage.ViewModels;
using System.Windows;

namespace ProjectWPF.StudentManage
{
    public partial class MonForLecturerView : Window
    {
        public MonForLecturerView(IMonService monService, string maGv)
        {
            InitializeComponent();
            DataContext = new MonForLecturerViewModel(monService, maGv);
        }
    }
}
