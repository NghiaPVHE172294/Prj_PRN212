using ProjectWPF.DTO.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWPF.Repository.Repositories
{
    public interface IKetQuaRepository
    {
        Task<IEnumerable<KetQua>> GetAllAsync();
        Task<KetQua?> GetByIdAsync(string maSo, string maMh);
        Task AddAsync(KetQua ketQua);
        Task UpdateAsync(KetQua ketQua);
        Task DeleteAsync(string maSo, string maMh);
    }
} 