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

		public async Task Update(Guid cartId, Guid productId, int quantity)
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
				// Nếu CartDetail đã có, cập nhật số lượng và tổng giá
				cartDetail.Quatity += quantity;  // Cộng thêm số lượng
				cartDetail.TotalPrice = cartDetail.Quatity * product.Price;  // Tính lại tổng giá

				_context.CartDetails.Update(cartDetail);
			}
			else
			{
				// Nếu CartDetail chưa có, tạo mới CartDetail
				var newCartDetail = new CartDetail
				{
					Id = Guid.NewGuid(),
					CartId = cartId,
					ProductId = productId,
					Quatity = quantity,
					TotalPrice = quantity * product.Price  // Tính tổng giá ngay khi thêm
				};

				await _context.CartDetails.AddAsync(newCartDetail);
			}

			// Cập nhật tổng giá trị giỏ hàng
			cart.TotalPrice = await _context.CartDetails
				.Where(cd => cd.CartId == cartId)
				.SumAsync(cd => cd.TotalPrice);

			// Cập nhật giỏ hàng
			_context.Carts.Update(cart);

			// Lưu thay đổi vào cơ sở dữ liệu
			await _context.SaveChangesAsync();
		}


        public async Task AddToCart(Guid cartId, Guid productId, Guid sizeId, int quantity)
        {
			//// Kiểm tra xem giỏ hàng có tồn tại không
			//var cart = await _context.Carts
			//	.FirstOrDefaultAsync(c => c.Id == cartId);

			//if (cart == null)
			//{
			//	throw new Exception("Giỏ hàng không tồn tại");
			//}

			//// Kiểm tra xem sản phẩm có tồn tại không
			//var product = await _context.Products
			//	.FirstOrDefaultAsync(p => p.Id == productId);

			//if (product == null)
			//{
			//	throw new Exception("Sản phẩm không tồn tại");
			//}

			//// Kiểm tra xem kích thước có tồn tại trong sản phẩm không
			//var size = product.Sizes.FirstOrDefault(s => s.Id == sizeId);
			//if (size == null)
			//{
			//	throw new Exception("Kích thước không hợp lệ");
			//}

			//// Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
			//var existingCartDetail = await _context.CartDetails
			//	.FirstOrDefaultAsync(cd => cd.CartId == cartId && cd.ProductId == productId && cd.SizeId == sizeId);

			//if (existingCartDetail != null)
			//{
			//	// Cập nhật số lượng nếu sản phẩm đã có trong giỏ
			//	existingCartDetail.Quatity += quantity;
			//	existingCartDetail.TotalPrice = existingCartDetail.Quatity * product.Price;

			//	_context.CartDetails.Update(existingCartDetail);
			//}
			//else
			//{
			//	// Thêm sản phẩm mới vào giỏ hàng
			//	var cartDetail = new CartDetail
			//	{
			//		Id = Guid.NewGuid(),
			//		CartId = cartId,
			//		ProductId = productId,
			//		SizeId = sizeId,
			//		Quatity = quantity,
			//		TotalPrice = quantity * product.Price
			//	};

			//	await _context.CartDetails.AddAsync(cartDetail);
			//}

			//// Lưu thay đổi vào cơ sở dữ liệu
			//await _context.SaveChangesAsync();
		}

		public async Task CheckOut(Guid cartId)
		{
			var cartDetails = await _context.CartDetails
				.Include(cd => cd.Product) 
				.Where(cd => cd.CartId == cartId)
				.ToListAsync();

			if (cartDetails == null || !cartDetails.Any())
			{
				throw new KeyNotFoundException("Không có dữ liệu trong giỏ hàng chi tiết");
			}

			// Kiểm tra thông tin giỏ hàng
			var cart = await _context.Carts
				.Include(c => c.Account)
				.AsNoTracking() // Tránh theo dõi Cart
				.FirstOrDefaultAsync(c => c.Id == cartId);

			if (cart == null)
			{
				throw new KeyNotFoundException("Không có dữ liệu trong giỏ hàng");
			}

			foreach (var cartDetail in cartDetails)
			{

				if (cartDetail.Product == null)
				{
					throw new KeyNotFoundException($"Sản phẩm {cartDetail.ProductId} không tồn tại");
				}

				if (cartDetail.Quatity > cartDetail.Product.Stock)
				{
					await SendStockAlertToAdmin(cartDetail.Product); // Gửi thông báo cho Admin
					throw new InvalidOperationException($"Không đủ hàng để checkout cho sản phẩm {cartDetail.Product.Name}. Sản phẩm đã hết hàng hoặc số lượng không đủ.");
				}
			}

			// Tính tổng số lượng và tổng tiền từ CartDetail
			decimal totalPrice = cartDetails.Sum(cd => cd.Quatity * cd.Product.Price);
			string customerName = cart.Account?.UserName ?? "Guest";

			// Tạo Order mới
			var newOrder = new Order
			{
				Id = Guid.NewGuid(),
				AccountId = cart.AccountId,
				CreateBy = cart.AccountId,
				DayCreate = DateTime.Now,
				Price = totalPrice,
				PaymentMethods = PaymentMethod.Cash,
				Status = OrderStatus.ChoXacNhan,
				CustomerName = customerName
			};

			await _context.Orders.AddAsync(newOrder);

			// Tạo danh sách OrderDetail (gộp các sản phẩm cùng ProductId)
			var orderDetails = cartDetails
				.GroupBy(cd => cd.ProductId)
				.Select(g => new OrderDetail
				{
					Id = Guid.NewGuid(),
					OrderId = newOrder.Id,
					ProductId = g.Key,
					Quatity = g.Sum(cd => cd.Quatity),
					TotalPrice = g.Sum(cd => cd.Quatity * cd.Product.Price)
				})
				.ToList();

			await _context.OrderDetails.AddRangeAsync(orderDetails);

			// Cập nhật lại số lượng kho trong Product (giảm số lượng theo CartDetail)
			foreach (var cartDetail in cartDetails)
			{

				if (cartDetail.Product != null)
				{
					cartDetail.Product.Stock -= cartDetail.Quatity;

					// Dùng Entry để gắn trạng thái cập nhật vào Entity Framework
					_context.Entry(cartDetail.Product).State = EntityState.Modified; // Đảm bảo là cập nhật, không phải thêm mới
				}
			}

			// Xóa CartDetail sau khi checkout
			_context.CartDetails.RemoveRange(cartDetails);

			// Lưu thay đổi vào cơ sở dữ liệu
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
