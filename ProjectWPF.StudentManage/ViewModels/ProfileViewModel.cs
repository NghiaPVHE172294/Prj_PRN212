using ProjectWPF.DTO.Models;
using ProjectWPF.Service.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ProjectWPF.StudentManage.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        public GiangVien? GiangVien { get; private set; }
        public SinhVien? SinhVien { get; private set; }
        public string Role { get; }
        private readonly IGiangVienService? _gvService;
        private readonly ISinhVienService? _svService;

        public ProfileViewModel(string role, string maSo, IGiangVienService? gvService, ISinhVienService? svService)
        {
            Role = role;
            _gvService = gvService;
            _svService = svService;
            _ = LoadProfileAsync(maSo);
        }

        private async Task LoadProfileAsync(string maSo)
        {
            if (Role == "GV" && _gvService != null)
            {
                GiangVien = await _gvService.GetByIdAsync(maSo);
            }
            else if (Role == "SV" && _svService != null)
            {
                SinhVien = await _svService.GetByIdAsync(maSo);
            }
            OnPropertyChanged(nameof(GiangVien));
            OnPropertyChanged(nameof(SinhVien));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
