using API.Data;
using API.IRepositories;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class CartDetailRepo : ICartDetailRepo
    {
        private readonly ApplicationDbContext _context;

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
        public async Task<CartDetail> GetCartDetailByProductId(Guid cartId, Guid productId)
        {
            return await _context.CartDetails.Include(p => p.Product)
                .FirstOrDefaultAsync(c => c.CartId == cartId && c.ProductId == productId);
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

		public async Task AddToCart(Guid cartId, Guid productId, int quantity)
		{
			// Tìm Cart
			var cart = await _context.Carts.FindAsync(cartId);
			if (cart == null)
			{
				throw new KeyNotFoundException("Cart not found.");
			}

			// Tìm Product
			var product = await _context.Products.FindAsync(productId);
			if (product == null)
			{
				throw new KeyNotFoundException("Product not found.");
			}

			// Kiểm tra CartDetail đã tồn tại hay chưa
			var cartDetail = await _context.CartDetails
				.FirstOrDefaultAsync(cd => cd.CartId == cartId && cd.ProductId == productId);

			if (cartDetail != null)
			{
				// Nếu đã có, cập nhật số lượng và tổng giá
				cartDetail.Quatity += quantity;
				cartDetail.TotalPrice = cartDetail.Quatity * product.Price;
				_context.CartDetails.Update(cartDetail);
			}
			else
			{
				// Nếu chưa có, tạo mới CartDetail
				var newCartDetail = new CartDetail
				{
					Id = Guid.NewGuid(),
					CartId = cartId,
					ProductId = productId,
					Quatity = quantity,
					TotalPrice = quantity * product.Price
				};

				await _context.CartDetails.AddAsync(newCartDetail);
			}

			// Cập nhật tổng giá trị giỏ hàng
			cart.TotalPrice = await _context.CartDetails
				.Where(cd => cd.CartId == cartId)
				.SumAsync(cd => cd.TotalPrice);

			// Lưu thay đổi
			await _context.SaveChangesAsync();
		}




	}
}
