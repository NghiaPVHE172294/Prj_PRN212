using ProjectWPF.DTO.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWPF.Service.Services
{
    public interface IKhoaService
    {
        Task<IEnumerable<Khoa>> GetAllAsync();
        Task<Khoa?> GetByIdAsync(string maKhoa);
        Task AddAsync(Khoa khoa);
        Task UpdateAsync(Khoa khoa);
        Task DeleteAsync(string maKhoa);
    }
} 