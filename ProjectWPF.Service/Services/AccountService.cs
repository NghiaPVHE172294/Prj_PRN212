using ProjectWPF.DTO.Models;
using ProjectWPF.Repository.Repositories;
using System.Threading.Tasks;

namespace ProjectWPF.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }
        public async Task<Account?> LoginAsync(string username, string password)
            => await _repository.LoginAsync(username, password);
        public async Task<string?> GetRoleAsync(string username)
            => await _repository.GetRoleAsync(username);
        public async Task<IEnumerable<Account>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Account?> GetByIdAsync(string maSo) => await _repository.GetByIdAsync(maSo);
        public async Task AddAsync(Account account) => await _repository.AddAsync(account);
        public async Task UpdateAsync(Account account) => await _repository.UpdateAsync(account);
        public async Task DeleteAsync(string maSo) => await _repository.DeleteAsync(maSo);
    }
} 