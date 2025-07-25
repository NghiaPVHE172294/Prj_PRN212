using ProjectWPF.DTO.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWPF.Repository.Repositories
{
    public class MonRepository : IMonRepository
    {
        private readonly IDbContextFactory<MyProjectContext> _contextFactory;
        public MonRepository(IDbContextFactory<MyProjectContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Mon>> GetAllAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Mons
                .Include(m => m.MaGvNavigation)
                .ToListAsync();
        }
        public async Task<Mon?> GetByIdAsync(string maMh)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Mons
                .Include(m => m.MaGvNavigation)
                .FirstOrDefaultAsync(m => m.MaMh == maMh);
        }
        public async Task AddAsync(Mon mon)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Mons.Add(mon);
            await context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Mon mon)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Mons.Update(mon);
            await context.SaveChangesAsync();
        }
        public async Task DeleteAsync(string maMh)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.Mons.FindAsync(maMh);
            if (entity != null)
            {
                context.Mons.Remove(entity);
                await context.SaveChangesAsync();
            }
        }
    }
} 