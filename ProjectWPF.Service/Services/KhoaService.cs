using ProjectWPF.DTO.Models;
using ProjectWPF.Repository.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWPF.Service.Services
{
    public class KhoaService : IKhoaService
    {
        private readonly IKhoaRepository _repository;
        public KhoaService(IKhoaRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Khoa>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Khoa?> GetByIdAsync(string maKhoa) => await _repository.GetByIdAsync(maKhoa);
        public async Task AddAsync(Khoa khoa) => await _repository.AddAsync(khoa);
        public async Task UpdateAsync(Khoa khoa) => await _repository.UpdateAsync(khoa);
        public async Task DeleteAsync(string maKhoa) => await _repository.DeleteAsync(maKhoa);
    }
} 