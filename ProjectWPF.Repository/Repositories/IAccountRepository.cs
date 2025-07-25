using ProjectWPF.DTO.Models;
using System.Threading.Tasks;

namespace ProjectWPF.Repository.Repositories
{
    public interface IAccountRepository
    {
        Task<Account?> LoginAsync(string username, string password);
        Task<string?> GetRoleAsync(string username);
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account?> GetByIdAsync(string maSo);
        Task AddAsync(Account account);
        Task UpdateAsync(Account account);
        Task DeleteAsync(string maSo);
    }
} 