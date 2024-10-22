using API.Data;
using API.IRepositories;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class VoucherRepos : IVoucherRepos
    {
        private readonly ApplicationDbContext _context;

        public VoucherRepos(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task create(Voucher voucher)
        {
            if (await GetById(voucher.Id) != null) throw new DuplicateWaitObjectException($"Voucher : {voucher.Id} is existed!");
            await _context.Vouchers.AddAsync(voucher);
            await _context.SaveChangesAsync();
        }

        public async Task delete(Guid id)
        {
            var voucher = await GetById(id);
            if (voucher == null) throw new KeyNotFoundException("Not found this voucher!");
            if (_context.Orders.Any(o => o.VoucherId == id))
                throw new Exception("This voucher is applied to one or more invoices and cannot be deleted!");
            _context.Vouchers.Remove(voucher);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Voucher>> GetAll()
        {
            return _context.Vouchers.ToList();
        }

        public async Task<Voucher> GetById(Guid id)
        {
            return await _context.Vouchers.FindAsync(id);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
        public async Task update(Voucher voucher)
        {
            if (await GetById(voucher.Id) == null)
                throw new KeyNotFoundException("Not found this voucher!");
            _context.Entry(voucher).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
