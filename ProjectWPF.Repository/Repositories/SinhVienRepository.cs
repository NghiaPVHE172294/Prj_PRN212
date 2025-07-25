using ProjectWPF.DTO.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWPF.Repository.Repositories
{
    public class SinhVienRepository : ISinhVienRepository
    {
        private readonly MyProjectContext _context;

        public SinhVienRepository(MyProjectContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SinhVien>> GetAllAsync()
            => await _context.SinhViens.ToListAsync();

        public async Task<SinhVien?> GetByIdAsync(string maSo)
            => await _context.SinhViens
                .Include(sv => sv.MaKhoaNavigation)
                .FirstOrDefaultAsync(sv => sv.MaSo == maSo);

        public async Task AddAsync(SinhVien sinhVien)
        {
            _context.SinhViens.Add(sinhVien);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SinhVien sinhVien)
        {
            _context.SinhViens.Update(sinhVien);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string maSo)
        {
            var sv = await _context.SinhViens.FindAsync(maSo);
            if (sv != null)
            {
                _context.SinhViens.Remove(sv);
                await _context.SaveChangesAsync();
            }
        }
    }

}