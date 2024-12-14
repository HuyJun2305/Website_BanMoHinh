using API.Data;
using API.IRepositories;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
            var cartDetails = await _context.CartDetails
                .Include(cd => cd.Product)   
                .ThenInclude(p => p.ProductSizes) 
                .Include(cd => cd.Size)     
                .Include(cd => cd.Cart)      
                .ToListAsync();

            return cartDetails;
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
        public async Task Update(Guid cartDetailId, Guid productId, Guid sizeId, int quantity)
        {
            // Tìm CartDetail dựa trên cartDetailId, productId và sizeId
            var cartDetail = await _context.CartDetails
                .FirstOrDefaultAsync(cd => cd.Id == cartDetailId && cd.ProductId == productId && cd.SizeId == sizeId);

            if (cartDetail == null)
            {
                throw new KeyNotFoundException("CartDetail not found.");
            }

            // Tìm ProductSize để kiểm tra tồn kho
            var productSize = await _context.ProductSizes
                .FirstOrDefaultAsync(ps => ps.ProductId == productId && ps.SizeId == sizeId);

            if (productSize == null)
            {
                throw new KeyNotFoundException("ProductSize not found.");
            }

            if (quantity > productSize.Stock)
            {
                quantity = productSize.Stock;
            }
            else if (quantity == 0)
            {
                _context.CartDetails.Remove(cartDetail);
                await _context.SaveChangesAsync();
            }
            else
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    throw new Exception("Product not found.");
                }
                // Cập nhật số lượng và tính lại tổng giá
                cartDetail.Quatity = quantity;
                cartDetail.TotalPrice = cartDetail.Quatity * product.Price; // Tính lại tổng giá dựa trên giá sản phẩm
                _context.CartDetails.Update(cartDetail);
            }

            // Cập nhật tổng giá trị giỏ hàng
            var cart = await _context.Carts.FindAsync(cartDetail.CartId);
            if (cart != null)
            {
                cart.TotalPrice = await _context.CartDetails
                    .Where(cd => cd.CartId == cart.Id)
                    .SumAsync(cd => cd.TotalPrice);
                _context.Carts.Update(cart);
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }
		public async Task AddToCart(Guid cartId, Guid productId, Guid sizeId, int quantity)
		{
			// Lấy giỏ hàng từ cơ sở dữ liệu
			var cart = await _context.Carts
				.Include(c => c.CartDetails) // Bao gồm CartDetails
				.ThenInclude(cd => cd.Product) // Bao gồm Product trong CartDetail
				.ThenInclude(p => p.ProductSizes) // Bao gồm ProductSizes của sản phẩm
				.Include(c => c.CartDetails)
				.ThenInclude(cd => cd.Size) // Bao gồm Size trong CartDetail
				.FirstOrDefaultAsync(c => c.Id == cartId);

			if (cart == null)
			{
				throw new Exception("Giỏ hàng không tồn tại.");
			}

			// Kiểm tra nếu sản phẩm và size đã tồn tại trong giỏ hàng
			var existingCartDetail = cart.CartDetails
				.FirstOrDefault(cd => cd.ProductId == productId && cd.SizeId == sizeId);

			if (existingCartDetail != null)
			{
				// Nếu sản phẩm đã tồn tại trong giỏ hàng, cộng thêm số lượng
				existingCartDetail.Quatity += quantity;
				existingCartDetail.TotalPrice = existingCartDetail.Quatity * existingCartDetail.Product.Price;
				_context.CartDetails.Update(existingCartDetail);
			}
			else
			{
				// Nếu sản phẩm chưa có trong giỏ hàng, thêm mới
				var product = await _context.Products
					.Include(p => p.ProductSizes)
					.FirstOrDefaultAsync(p => p.Id == productId);

				var size = await _context.Sizes
					.FirstOrDefaultAsync(s => s.Id == sizeId);

				if (product == null || size == null)
				{
					throw new Exception("Sản phẩm hoặc kích thước không tồn tại.");
				}

				if (!product.ProductSizes.Any(ps => ps.SizeId == sizeId))
				{
					throw new Exception("Sản phẩm này không có sẵn trong kích thước này.");
				}

				var newCartDetail = new CartDetail
				{
					Id = Guid.NewGuid(),
					CartId = cartId,
					ProductId = productId,
					SizeId = sizeId,
					Quatity = quantity,
					TotalPrice = product.Price * quantity
				};

				// Thêm mới CartDetail vào giỏ hàng
				_context.CartDetails.Add(newCartDetail);
			}

			// Lưu các thay đổi vào cơ sở dữ liệu
			await _context.SaveChangesAsync();
		}



        public async Task CheckOut(
    List<Guid> cartDetailIds,
    decimal shippingFee,
    string city,
    string district,
    string ward,
    string addressDetail)
        {
            // Kiểm tra danh sách ID giỏ hàng
            if (cartDetailIds == null || !cartDetailIds.Any())
                throw new ArgumentException("CartDetailIds cannot be null or empty");

            // Kiểm tra thông tin địa chỉ
            if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(district) ||
                string.IsNullOrWhiteSpace(ward) || string.IsNullOrWhiteSpace(addressDetail))
                throw new ArgumentException("Address fields cannot be empty");

            // Lấy thông tin CartDetails với sản phẩm
            var cartDetails = await _context.CartDetails
                .Where(cd => cartDetailIds.Contains(cd.Id))
                .Include(cd => cd.Product) 
                .ThenInclude(p => p.ProductSizes).AsNoTracking()
                .ToListAsync();

            if (!cartDetails.Any())
                throw new KeyNotFoundException("No cart details found for the provided IDs");

            // Lấy thông tin giỏ hàng
            var cartId = cartDetails.First().CartId;
            var cart = await _context.Carts
                .Where(c => c.Id == cartId)
                .FirstOrDefaultAsync();

            if (cart == null)
                throw new KeyNotFoundException("Cart not found for the selected cart details");

            decimal totalPrice = cartDetails.Sum(cd => cd.Quatity * cd.Product.Price);


            decimal finalTotalPrice = totalPrice + shippingFee;

            var newOrder = new Order
            {
                Id = Guid.NewGuid(),
                AccountId = cart.AccountId,
                CreateBy = cart.AccountId,
                DayCreate = DateTime.Now,
                Price = finalTotalPrice,
                ShippingFee = shippingFee,
                PaymentMethods = PaymentMethod.Cash, // Điều chỉnh theo yêu cầu
                Status = OrderStatus.ChoXacNhan,
                CustomerName = cart.Account?.UserName ?? "Guest"
            };

            await _context.Orders.AddAsync(newOrder);

            // Tạo OrderDetails
            var orderDetails = cartDetails.Select(cd => new OrderDetail
            {
                Id = Guid.NewGuid(),
                OrderId = newOrder.Id,
                ProductId = cd.Product.Id, // Sử dụng Product.Id thay vì ProductId
                SizeId = cd.SizeId,
                Quatity = cd.Quatity,
                TotalPrice = cd.Quatity * cd.Product.Price
            }).ToList();

            await _context.OrderDetails.AddRangeAsync(orderDetails);
            _context.CartDetails.RemoveRange(cartDetails);

            var orderAddress = new OrderAddress
            {
                Id = Guid.NewGuid(),
                OrderId = newOrder.Id,
                City = city,
                District = district,
                Ward = ward,
                AddressDetail = addressDetail
            };

            await _context.OrderAddresses.AddAsync(orderAddress);

            await _context.SaveChangesAsync();
        }











        // Hàm gửi thông báo cho Admin về số lượng kho không đủ
        private async Task SendStockAlertToAdmin(Product product)
		{
			// Gửi email hoặc log thông báo cho Admin
			var adminEmail = "admin@example.com"; // Email Admin
			var subject = "Thông báo: Cập nhật số lượng sản phẩm";
			var body = $"Sản phẩm {product.Name} đã hết hàng hoặc số lượng không đủ trong kho. Vui lòng kiểm tra và cập nhật số lượng sản phẩm.";

			// Ví dụ gửi email (có thể sử dụng thư viện email như SMTP hoặc dịch vụ email bên ngoài)
			//await _emailService.SendEmailAsync(adminEmail, subject, body);
		}


	}
}
