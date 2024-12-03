using API.Data;
using API.IRepositories;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
	public class OrderDetailRepo : IOrderDetailRepo
	{
		private readonly ApplicationDbContext _context;
		public OrderDetailRepo(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<OrderDetail>?> GetOrderDetailsByOrderIdAsync(Guid orderId)
		{
			return await _context.OrderDetails
				.Include(p => p.Product).ThenInclude(p => p.Images)
					.Where(od => od.OrderId == orderId)
					.ToListAsync();
		}

		public async Task<OrderDetail?> GetOrderDetailByIdAsync(Guid id)
		{
			return await _context.OrderDetails
				.Include(od => od.Product)
				.FirstOrDefaultAsync(od => od.Id == id);
		}

		public async Task CreateAsync(OrderDetail orderDetail)
		{
			if (orderDetail == null)
				throw new ArgumentNullException(nameof(orderDetail));

			await _context.OrderDetails.AddAsync(orderDetail);
		}

		public async Task UpdateAsync(OrderDetail orderDetail)
		{
			if (orderDetail == null)
				throw new ArgumentNullException(nameof(orderDetail));

			var existingOrderDetail = await GetOrderDetailByIdAsync(orderDetail.Id);
			if (existingOrderDetail == null)
				throw new KeyNotFoundException($"No OrderDetail found with ID: {orderDetail.Id}");

			_context.Entry(existingOrderDetail).CurrentValues.SetValues(orderDetail);
		}

		public async Task DeleteAsync(Guid id)
		{
			var orderDetail = await GetOrderDetailByIdAsync(id);
			if (orderDetail == null)
				throw new KeyNotFoundException($"No OrderDetail found with ID: {id}");

			_context.OrderDetails.Remove(orderDetail);
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}

		public async Task<OrderDetail?> GetOrderDetailByOrderAndProductIdAsync(Guid orderId, Guid productId)
		{
			return await _context.OrderDetails
				.FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId);
		}

		public async Task AddOrderDetailAsync(OrderDetail orderDetail)
		{
			await _context.OrderDetails.AddAsync(orderDetail);
		}

		public async Task UpdateOrderDetailAsync(OrderDetail orderDetail)
		{
			_context.OrderDetails.Update(orderDetail);
		}

		public async Task<decimal> GetTotalPriceByOrderIdAsync(Guid orderId)
		{
			var orderDetails = await _context.OrderDetails
				.Where(od => od.OrderId == orderId)
				.ToListAsync();

			return orderDetails.Sum(od => od.TotalPrice);
		}

		public async Task<OrderDetail?> AddOrUpdateOrderDetail(Guid orderId, Guid productId, int quantity)
		{
			// Kiểm tra Order tồn tại
			var order = await _context.Orders
				.FirstOrDefaultAsync(od => od.Id == orderId);
			if (order == null)
			{
				throw new KeyNotFoundException($"Order with ID {orderId} not found.");
			}

			// Kiểm tra Product tồn tại
			var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
			if (product == null)
			{
				throw new KeyNotFoundException($"Product with ID {productId} not found.");
			}

			// Kiểm tra OrderDetail đã tồn tại
			var existingOrderDetail = await _context.OrderDetails
				.FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId);

			OrderDetail newOrderDetail = null; // Khai báo mới cho OrderDetail

			if (existingOrderDetail != null)
			{
				// Cập nhật số lượng và tổng giá
				existingOrderDetail.Quatity += quantity; // Sửa chính tả từ Quatity => Quantity
				existingOrderDetail.TotalPrice = existingOrderDetail.Quatity * product.Price;

				// Cập nhật OrderDetail trong cơ sở dữ liệu
				_context.Entry(existingOrderDetail).State = EntityState.Modified;
			}
			else
			{
				// Tạo mới OrderDetail
				newOrderDetail = new OrderDetail
				{
					Id = Guid.NewGuid(),
					OrderId = orderId,
					ProductId = productId,
					Quatity = quantity, // Sửa chính tả từ Quatity => Quantity
					TotalPrice = quantity * product.Price
				};

				// Thêm OrderDetail mới vào cơ sở dữ liệu
				await _context.OrderDetails.AddAsync(newOrderDetail);
			}

			// Cập nhật tổng giá của Order
			order.Price = await _context.OrderDetails
				.Where(od => od.OrderId == orderId)
				.SumAsync(od => od.TotalPrice);

			// Cập nhật Order trong cơ sở dữ liệu
			_context.Entry(order).State = EntityState.Modified;

			// Lưu thay đổi vào cơ sở dữ liệu
			await _context.SaveChangesAsync();

			// Trả về OrderDetail vừa được thêm hoặc cập nhật
			return existingOrderDetail ?? newOrderDetail;
		}

		public async Task<bool> RemoveOrderDetail(Guid orderId, Guid productId, int quantityToRemove)
		{
			// Kiểm tra Order tồn tại
			var order = await _context.Orders
				.FirstOrDefaultAsync(od => od.Id == orderId);
			if (order == null)
			{
				throw new KeyNotFoundException($"Order with ID {orderId} not found.");
			}

			// Kiểm tra Product tồn tại trong OrderDetail
			var orderDetail = await _context.OrderDetails.Include(od => od.Product)
				.FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId);
			if (orderDetail == null)
			{
				throw new KeyNotFoundException($"OrderDetail for Product ID {productId} not found in Order ID {orderId}.");
			}

			// Kiểm tra số lượng muốn xóa có hợp lệ không
			if (quantityToRemove > orderDetail.Quatity)
			{
				throw new InvalidOperationException($"Cannot remove more quantity than available. Available quantity: {orderDetail.Quatity}");
			}

			// Cập nhật số lượng nếu còn lại sau khi xóa
			if (quantityToRemove < orderDetail.Quatity)
			{
				orderDetail.Quatity -= quantityToRemove;
				orderDetail.TotalPrice = orderDetail.Quatity * orderDetail.Product.Price;

				// Cập nhật OrderDetail trong cơ sở dữ liệu
				_context.Entry(orderDetail).State = EntityState.Modified;
			}
			else
			{
				// Nếu số lượng còn lại là 0, xóa OrderDetail
				_context.OrderDetails.Remove(orderDetail);
			}

			// Cập nhật tổng giá của Order sau khi xóa
			order.Price = await _context.OrderDetails
				.Where(od => od.OrderId == orderId)
				.SumAsync(od => od.TotalPrice);

			// Cập nhật Order trong cơ sở dữ liệu
			_context.Entry(order).State = EntityState.Modified;

			// Lưu thay đổi vào cơ sở dữ liệu
			await _context.SaveChangesAsync();

			// Trả về true nếu thao tác thành công
			return true;
		}
	}

}
