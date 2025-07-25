using ProjectWPF.DTO.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWPF.Repository.Repositories
{
    public class KhoaRepository : IKhoaRepository
    {
        private readonly IDbContextFactory<MyProjectContext> _contextFactory;
        public KhoaRepository(IDbContextFactory<MyProjectContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<Khoa>> GetAllAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Khoas.ToListAsync();
        }
        public async Task<Khoa?> GetByIdAsync(string maKhoa)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Khoas.FindAsync(maKhoa);
        }
        public async Task AddAsync(Khoa khoa)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Khoas.Add(khoa);
            await context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Khoa khoa)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Khoas.Update(khoa);
            await context.SaveChangesAsync();
        }
        public async Task DeleteAsync(string maKhoa)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = await context.Khoas.FindAsync(maKhoa);
            if (entity != null)
            {
                context.Khoas.Remove(entity);
                await context.SaveChangesAsync();
            }
        }
    }
} 