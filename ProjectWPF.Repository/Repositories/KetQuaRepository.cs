using Microsoft.EntityFrameworkCore;
using ProjectWPF.DTO.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWPF.Repository.Repositories
{
    public class KetQuaRepository : IKetQuaRepository
    {
        private readonly IDbContextFactory<MyProjectContext> _contextFactory;
        public KetQuaRepository(IDbContextFactory<MyProjectContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<KetQua>> GetAllAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.KetQuas.Include(k => k.MaMhNavigation).ToListAsync();
        }
        public async Task<KetQua?> GetByIdAsync(string maSo, string maMh)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.KetQuas.FindAsync(maSo, maMh);
        }
        public async Task AddAsync(KetQua ketQua)
        {
            using var context = _contextFactory.CreateDbContext();
            context.KetQuas.Add(ketQua);
            await context.SaveChangesAsync();
        }
        public async Task UpdateAsync(KetQua ketQua)
        {
            using var context = _contextFactory.CreateDbContext();
            context.KetQuas.Update(ketQua);
            await context.SaveChangesAsync();
        }
        public async Task DeleteAsync(string maSo, string maMh)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.KetQuas.FindAsync(maSo, maMh);
            if (entity != null)
            {
                context.KetQuas.Remove(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}