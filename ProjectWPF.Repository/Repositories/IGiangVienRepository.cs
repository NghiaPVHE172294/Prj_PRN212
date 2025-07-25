using ProjectWPF.DTO.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWPF.Repository.Repositories
{
    public interface IGiangVienRepository
    {
        Task<IEnumerable<GiangVien>> GetAllAsync();
        Task<GiangVien?> GetByIdAsync(string maSo);
        Task AddAsync(GiangVien giangVien);
        Task UpdateAsync(GiangVien giangVien);
        Task DeleteAsync(string maSo);
    }
} 