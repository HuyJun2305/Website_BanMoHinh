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
                cartDetail.Quantity = quantity;
                cartDetail.TotalPrice = cartDetail.Quantity * product.Price; // Tính lại tổng giá dựa trên giá sản phẩm
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
				existingCartDetail.Quantity += quantity;
				existingCartDetail.TotalPrice = existingCartDetail.Quantity * existingCartDetail.Product.Price;
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
					Quantity = quantity,
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
            // Kiểm tra các tham số đầu vào
            if (cartDetailIds == null || !cartDetailIds.Any())
                throw new ArgumentException("CartDetailIds cannot be null or empty");

            if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(district) ||
                string.IsNullOrWhiteSpace(ward) || string.IsNullOrWhiteSpace(addressDetail))
                throw new ArgumentException("Address fields cannot be empty");

            if (shippingFee < 0)
                throw new ArgumentException("Shipping fee must be a positive value");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Lấy thông tin CartDetails
                var cartDetails = await _context.CartDetails
                    .Where(cd => cartDetailIds.Contains(cd.Id))
                    .Select(cd => new
                    {
                        cd.Id,
                        cd.ProductId,
                        cd.SizeId,
                        cd.Quantity,
                        cd.CartId,
                        ProductPrice = cd.Product.Price
                    })
                    .ToListAsync();

                if (!cartDetails.Any())
                    throw new KeyNotFoundException("No cart details found for the provided IDs");

                var cartId = cartDetails.First().CartId;

                // Lấy thông tin giỏ hàng
                var cart = await _context.Carts
                    .Where(c => c.Id == cartId)
                    .Select(c => new { c.AccountId, AccountName = c.Account.UserName })
                    .FirstOrDefaultAsync();

                if (cart == null)
                    throw new KeyNotFoundException("Cart not found for the selected cart details");

                // Tính toán tổng giá
                decimal totalPrice = cartDetails.Sum(cd => cd.Quantity * cd.ProductPrice);
                decimal finalTotalPrice = totalPrice + shippingFee;

                // Tạo Order mới
                var newOrder = new Order
                {
                    Id = Guid.NewGuid(),
                    AccountId = cart.AccountId,
                    CreateBy = cart.AccountId,
                    DayCreate = DateTime.Now,
                    Price = finalTotalPrice,
                    ShippingFee = shippingFee,
                    PaymentMethods = PaymentMethod.Cash,
                    Status = OrderStatus.WaitingForConfirmation,
                    CustomerName = cart.AccountName ?? "Guest"
                };
                await _context.Orders.AddAsync(newOrder);

                // Tạo OrderDetail
                var orderDetails = cartDetails.Select(cd => new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    OrderId = newOrder.Id,
                    ProductId = cd.ProductId,
                    SizeId = cd.SizeId,
                    Quantity = cd.Quantity,
                    TotalPrice = cd.Quantity * cd.ProductPrice
                }).ToList();
                await _context.OrderDetails.AddRangeAsync(orderDetails);

                // Xóa CartDetails trực tiếp bằng Id
                _context.CartDetails.RemoveRange(
                    _context.CartDetails.Where(cd => cartDetailIds.Contains(cd.Id))
                );

                // Tạo OrderAddress
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

                // Lưu thay đổi
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
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
