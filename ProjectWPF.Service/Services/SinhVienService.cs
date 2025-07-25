using ProjectWPF.DTO.Models;
using ProjectWPF.Repository.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWPF.Service.Services
{
    public class SinhVienService : ISinhVienService
    {
        private readonly ISinhVienRepository _repo;

        public SinhVienService(ISinhVienRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SinhVien>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<SinhVien?> GetByIdAsync(string maSo) => await _repo.GetByIdAsync(maSo);

        public async Task AddAsync(SinhVien sinhVien)
        {
            var existing = await _repo.GetByIdAsync(sinhVien.MaSo);
            if (existing != null)
                throw new Exception("Sinh viên đã tồn tại!");

            await _repo.AddAsync(sinhVien);
        }

        public async Task UpdateAsync(SinhVien sinhVien)
        {
            var existing = await _repo.GetByIdAsync(sinhVien.MaSo);
            if (existing == null)
                throw new Exception("Sinh viên không tồn tại!");

            await _repo.UpdateAsync(sinhVien);
        }

        public async Task DeleteAsync(string maSo)
        {
            var existing = await _repo.GetByIdAsync(maSo);
            if (existing == null)
                throw new Exception("Không tìm thấy sinh viên để xóa.");

            await _repo.DeleteAsync(maSo);
        }
    }

}