using API.Data;
using API.IRepositories;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class AddresRepo : IAddresRepo
    {
        private readonly ApplicationDbContext _context;
        public AddresRepo(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
        //
        public async Task Create(Address data)
        {
            if (await GetAddresById(data.Id) != null) throw new DuplicateWaitObjectException($"Address : {data.Id} is existed!");
            await _context.Addresses.AddAsync(data);
        }
        //
        public async Task Delete(Guid id)
        {
            var data = await GetAddresById(id);
            if (data == null) throw new KeyNotFoundException("Not found this brand!");
            if (_context.Products.Where(p => p.IdBrand == data.Id).Any()) throw new Exception("This brand has used for some product!");
            _context.Addresses.Remove(data);
        }
        //
        public async Task<List<Address>> GetAllAddres()
        {
            return await _context.Addresses.ToListAsync();
        }
        //
        public async Task<Address> GetAddresById(Guid id)
        {
            return await _context.Addresses.FindAsync(id);
        }
        //
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
        //
        public async Task Update(Address data)
        {
            if (await GetAddresById(data.Id) == null) throw new KeyNotFoundException("Not found this brand!");
            _context.Entry(data).State = EntityState.Modified;
        }
    }
}
