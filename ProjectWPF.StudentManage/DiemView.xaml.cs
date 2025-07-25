using ProjectWPF.StudentManage.ViewModels;
using ProjectWPF.Service.Services;
using System.Windows;

namespace ProjectWPF.StudentManage
{
    public partial class DiemView : Window
    {
        public DiemView(IKetQuaService ketQuaService, ISinhVienService sinhVienService, IMonService monService, string maGv)
{
    InitializeComponent();
    DataContext = new DiemViewModel(ketQuaService, sinhVienService, monService, maGv);
}
    }
} 