using ProjectWPF.DTO.Models;
using ProjectWPF.Repository.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWPF.Service.Services
{
    public class GiangVienService : IGiangVienService
    {
        private readonly IGiangVienRepository _repository;
        public GiangVienService(IGiangVienRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<GiangVien>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<GiangVien?> GetByIdAsync(string maSo) => await _repository.GetByIdAsync(maSo);
        public async Task AddAsync(GiangVien giangVien) => await _repository.AddAsync(giangVien);
        public async Task UpdateAsync(GiangVien giangVien) => await _repository.UpdateAsync(giangVien);
        public async Task DeleteAsync(string maSo) => await _repository.DeleteAsync(maSo);
    }
} 