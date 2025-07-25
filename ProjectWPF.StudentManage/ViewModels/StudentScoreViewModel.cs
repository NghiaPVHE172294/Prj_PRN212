using ProjectWPF.DTO.Models;
using ProjectWPF.Service.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Linq;

namespace ProjectWPF.StudentManage.ViewModels
{
    public class StudentScoreViewModel : INotifyPropertyChanged
    {
        private readonly IKetQuaService _ketQuaService;
        public ObservableCollection<KetQua> DanhSachDiem { get; set; } = new();

        public StudentScoreViewModel(string maSo, IKetQuaService ketQuaService)
        {
            _ketQuaService = ketQuaService;
            _ = LoadScoresAsync(maSo);
        }

        private async Task LoadScoresAsync(string maSo)
        {
            var all = await _ketQuaService.GetAllAsync();
            DanhSachDiem.Clear();
            foreach (var kq in all.Where(x => x.MaSo == maSo))
            {
                DanhSachDiem.Add(kq);
            }
            OnPropertyChanged(nameof(DanhSachDiem));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
