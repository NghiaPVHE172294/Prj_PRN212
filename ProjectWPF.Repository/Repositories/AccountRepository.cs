using ProjectWPF.DTO.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ProjectWPF.Repository.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbContextFactory<MyProjectContext> _contextFactory;
        public AccountRepository(IDbContextFactory<MyProjectContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<Account?> LoginAsync(string username, string password)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Accounts.FirstOrDefaultAsync(a =>
                a.Username.Trim() == username.Trim() &&
                a.Password.Trim() == password.Trim());
        }
        public async Task<string?> GetRoleAsync(string username)
        {
            using var context = _contextFactory.CreateDbContext();
            var account = await context.Accounts.FirstOrDefaultAsync(a => a.Username == username);
            return account?.Role;
        }
        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Accounts.ToListAsync();
        }
        public async Task<Account?> GetByIdAsync(string maSo)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Accounts.FindAsync(maSo);
        }
        public async Task AddAsync(Account account)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Accounts.Add(account);
            await context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Account account)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Accounts.Update(account);
            await context.SaveChangesAsync();
        }
        public async Task DeleteAsync(string maSo)
        {
            using var context = _contextFactory.CreateDbContext();
            var acc = await context.Accounts.FindAsync(maSo);
            if (acc != null)
            {
                context.Accounts.Remove(acc);
                await context.SaveChangesAsync();
            }
        }
    }
} 