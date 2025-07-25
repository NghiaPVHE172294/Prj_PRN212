using ProjectWPF.DTO.Models;
using ProjectWPF.Repository.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWPF.Service.Services
{
    public class MonService : IMonService
    {
        private readonly IMonRepository _repository;
        public MonService(IMonRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Mon>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Mon?> GetByIdAsync(string maMh) => await _repository.GetByIdAsync(maMh);
        public async Task AddAsync(Mon mon) => await _repository.AddAsync(mon);
        public async Task UpdateAsync(Mon mon) => await _repository.UpdateAsync(mon);
        public async Task DeleteAsync(string maMh) => await _repository.DeleteAsync(maMh);
    }
} 