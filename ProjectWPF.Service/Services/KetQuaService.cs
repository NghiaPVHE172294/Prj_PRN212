using ProjectWPF.DTO.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProjectWPF.Service.Services
{
    public class KetQuaService : IKetQuaService
    {
        private readonly MyProjectContext _context;
        public KetQuaService(MyProjectContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<KetQua>> GetAllAsync()
            => await _context.KetQuas.Include(k => k.MaMhNavigation).ToListAsync();
        public async Task AddAsync(KetQua ketQua)
        {
            _context.KetQuas.Add(ketQua);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(KetQua ketQua)
        {
            _context.KetQuas.Update(ketQua);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(string maSo, string maMh)
        {
            var entity = await _context.KetQuas.FirstOrDefaultAsync(k => k.MaSo == maSo && k.MaMh == maMh);
            if (entity != null)
            {
                _context.KetQuas.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
} 