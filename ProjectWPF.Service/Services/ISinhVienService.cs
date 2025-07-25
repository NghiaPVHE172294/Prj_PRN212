using ProjectWPF.DTO.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWPF.Service.Services
{
    public interface ISinhVienService
    {
        Task<IEnumerable<SinhVien>> GetAllAsync();
        Task<SinhVien?> GetByIdAsync(string maSo);
        Task AddAsync(SinhVien sinhVien);
        Task UpdateAsync(SinhVien sinhVien);
        Task DeleteAsync(string maSo);
    }
} 