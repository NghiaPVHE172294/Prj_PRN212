using ProjectWPF.DTO.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWPF.Repository.Repositories
{
    public class GiangVienRepository : IGiangVienRepository
    {
        private readonly IDbContextFactory<MyProjectContext> _contextFactory;
        public GiangVienRepository(IDbContextFactory<MyProjectContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<GiangVien>> GetAllAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.GiangViens
                .Include(gv => gv.MaKhoaNavigation)
                .Include(gv => gv.Mons)
                .ToListAsync();
        }
        public async Task<GiangVien?> GetByIdAsync(string maSo)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.GiangViens
                .Include(gv => gv.MaKhoaNavigation)
                .Include(gv => gv.Mons)
                .FirstOrDefaultAsync(gv => gv.MaSo == maSo);
        }
        public async Task AddAsync(GiangVien giangVien)
        {
            using var context = _contextFactory.CreateDbContext();
            context.GiangViens.Add(giangVien);
            await context.SaveChangesAsync();
        }
        public async Task UpdateAsync(GiangVien giangVien)
        {
            using var context = _contextFactory.CreateDbContext();
            context.GiangViens.Update(giangVien);
            await context.SaveChangesAsync();
        }
        public async Task DeleteAsync(string maSo)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.GiangViens.FindAsync(maSo);
            if (entity != null)
            {
                context.GiangViens.Remove(entity);
                await context.SaveChangesAsync();
            }
        }
    }
} 