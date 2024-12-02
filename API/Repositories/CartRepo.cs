using API.Data;
using API.IRepositories;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class CartRepo :  ICartRepo
    {
        private readonly ApplicationDbContext _context;

        public CartRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Create(Cart cart)
        {
            if (await GetCartById(cart.Id) != null) throw new DuplicateWaitObjectException($"Cart : {cart.Id} is existed!");
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            
        }

        public async Task<Cart> CreateAsync(Cart cart)
        {
			await _context.Carts.AddAsync(cart);
			await _context.SaveChangesAsync();
            return cart;

		}
        

        public async Task<List<Cart>> GetAllCart()
        {
            return await _context.Carts.Include(p => p.Account).ToListAsync();
        }

        public async Task<Cart> GetCartById(Guid id)
        {
            return await _context.Carts.Include(p => p.Account).FirstOrDefaultAsync();
        }

        public async Task<Cart> GetCartByUserId(Guid userId)
        {
            return await _context.Carts.Include(p => p.Account).Where(p => p.AccountId == userId).FirstAsync();
        }


		public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(Cart cart)
        {
            if (await GetCartById(cart.Id) == null) throw new KeyNotFoundException("Not found this Id!");
            _context.Entry(cart).State = EntityState.Modified;
        }

    }
}
