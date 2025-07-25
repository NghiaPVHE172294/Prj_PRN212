using ProjectWPF.DTO.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWPF.Service.Services
{
    public interface IKetQuaService
    {
        Task<IEnumerable<KetQua>> GetAllAsync();
        Task AddAsync(KetQua ketQua);
        Task UpdateAsync(KetQua ketQua);
        Task DeleteAsync(string maSo, string maMh);
    }
} 