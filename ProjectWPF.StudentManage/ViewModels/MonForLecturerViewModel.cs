using ProjectWPF.DTO.Models;
using ProjectWPF.Service.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ProjectWPF.StudentManage.ViewModels
{
    public class MonForLecturerViewModel : INotifyPropertyChanged
    {
        private readonly IMonService _monService;
        private readonly string _maGv;
        public ObservableCollection<Mon> DanhSachMon { get; set; } = new();

        public MonForLecturerViewModel(IMonService monService, string maGv)
        {
            _monService = monService;
            _maGv = maGv;
            _ = LoadMonsAsync();
        }

        private async Task LoadMonsAsync()
        {
            var mons = await _monService.GetAllAsync();
            DanhSachMon.Clear();
            foreach (var m in mons)
                if (m.MaGv == _maGv)
                    DanhSachMon.Add(m);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
