using API.Data;
using API.IRepositories;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class CartRepo :  ICartRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartRepo(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
            return await _context.Carts.Include(p => p.Account)
                .Where(c => c.AccountId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Cart> GetCartByUserId(Guid userId)
        {
            // Lấy thông tin người dùng
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return null;

            // Kiểm tra vai trò người dùng
            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Customer")) return null;

            // Nếu là "Customer", lấy giỏ hàng
            return await _context.Carts
                .Include(c => c.CartDetails).ThenInclude(x => x.Product).ThenInclude(i => i.Images)
                .Include(c => c.Account)
                .Where(c => c.AccountId == userId)
                .FirstOrDefaultAsync();
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
