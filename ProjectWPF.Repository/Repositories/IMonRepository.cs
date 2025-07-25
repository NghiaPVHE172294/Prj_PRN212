using ProjectWPF.DTO.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWPF.Repository.Repositories
{
    public interface IMonRepository
    {
        Task<IEnumerable<Mon>> GetAllAsync();
        Task<Mon?> GetByIdAsync(string maMh);
        Task AddAsync(Mon mon);
        Task UpdateAsync(Mon mon);
        Task DeleteAsync(string maMh);
    }
} 