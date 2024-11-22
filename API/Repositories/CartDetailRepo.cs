using API.Data;
using API.IRepositories;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class CartDetailRepo : ICartDetailRepo
    {
        private  readonly ApplicationDbContext _context;

        public CartDetailRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(CartDetail cartdetail)
        {
            if (await GetCartDetailById(cartdetail.Id) != null) throw new DuplicateWaitObjectException($"Product : {cartdetail.Id} is existed!");
            await _context.CartDetails.AddAsync(cartdetail);
        }

        public async Task Delete(Guid id)
        {
            var cartdetail = await GetCartDetailById(id);
            if (cartdetail == null) throw new KeyNotFoundException("Not found this Id!");
            _context.CartDetails.Remove(cartdetail);
        }

        public async Task<List<CartDetail>> GetAllCartDetail()
        {
            return await _context.CartDetails.Include(p => p.Cart).Include(p => p.Product).ToListAsync();
        }

        public async Task<CartDetail> GetCartDetailById(Guid id)
        {
            return await _context.CartDetails.Include(p => p.Cart).Include(p => p.Product).FirstOrDefaultAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(CartDetail cartDetails, Guid id)
        {
            var updateItem = await _context.CartDetails.FindAsync(id);
            //if (cartDetails != null)
            //{
            //    updateItem.Quanlity = cartDetails.Quanlity;
            //    updateItem.TotalPrice = cartDetails.TotalPrice;
            //}
            _context.CartDetails.Update(updateItem);
            await _context.SaveChangesAsync();
        }
    }
}
