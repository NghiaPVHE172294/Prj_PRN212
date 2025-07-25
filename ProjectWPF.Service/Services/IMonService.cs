using ProjectWPF.DTO.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWPF.Service.Services
{
    public interface IMonService
    {
        Task<IEnumerable<Mon>> GetAllAsync();
        Task<Mon?> GetByIdAsync(string maMh);
        Task AddAsync(Mon mon);
        Task UpdateAsync(Mon mon);
        Task DeleteAsync(string maMh);
    }
} 