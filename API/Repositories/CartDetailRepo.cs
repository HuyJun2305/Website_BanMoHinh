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

        public async Task Create(CartDetail cartDetails)
        {
            bool exists = await _context.CartDetails.AnyAsync(c => c.Id == cartDetails.Id);
            if (exists)
            {
                throw new DuplicateWaitObjectException("This cartDetails is existed!");
            }
            await _context.CartDetails.AddAsync(cartDetails);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var deleteItem = await _context.CartDetails.FindAsync(id);
            if (deleteItem == null)
            {
                throw new KeyNotFoundException($"CartDetail with Id {id} not found.");
            }
            _context.CartDetails.Remove(deleteItem);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CartDetail>> GetAllCartDetail()
        {
            var lstCartDetails = await _context.CartDetails
                                .Include(pd => pd.Product)
                            .ToListAsync();
            return lstCartDetails;
        }

        public async Task<List<CartDetail>?> GetCartDetailByCartId(Guid cartId)
        {
            return await _context.CartDetails
                            .Where(cd => cd.CartId == cartId)
                                .Include(pd => pd.Product)
                            .ToListAsync();
        }

        public async Task<CartDetail> GetCartDetailById(Guid id)
        {
            return await _context.CartDetails.FindAsync(id);
        }



        public async Task Update(CartDetail cartDetails, Guid id)
        {
            var updateItem = await _context.CartDetails.FindAsync(id);
            if (cartDetails != null)
            {
                updateItem.Quatity = cartDetails.Quatity;
                updateItem.TotalPrice = cartDetails.TotalPrice;
            }
            _context.CartDetails.Update(updateItem);
            await _context.SaveChangesAsync();
        }
    }
}
