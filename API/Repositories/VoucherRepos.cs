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

        public Task Create(Voucher voucher)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Voucher>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Voucher> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task Update(Voucher voucher)
        {
            throw new NotImplementedException();
        }
    }
}
