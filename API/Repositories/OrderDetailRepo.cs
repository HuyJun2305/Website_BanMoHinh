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
		//sửa
		public async Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(Guid? orderId)
		{
			return await _context.OrderDetails
		.Include(od => od.Product)
			.ThenInclude(p => p.Brand)
		.Include(od => od.Product)
			.ThenInclude(p => p.Category)
		.Include(od => od.Product)
			.ThenInclude(p => p.Material)
		.Include(od => od.Product)
			.ThenInclude(p => p.Images)
		.Include(od => od.Product)
			.ThenInclude(p => p.Promotions)
            .Include(od => od.Product)
                .ThenInclude(od => od.ProductSizes).ThenInclude(od => od.Size)
        .Where(od => od.OrderId == orderId)
		.ToListAsync();
		}
        //sửa
        public async Task<OrderDetail> GetOrderDetailByIdAsync(Guid id)
        {
            return await _context.OrderDetails
                .Include(od => od.Product)
                    .ThenInclude(p => p.Brand)
                .Include(od => od.Product)
                    .ThenInclude(p => p.Category)
                .Include(od => od.Product)
                    .ThenInclude(p => p.Material)
                .Include(od => od.Product)
                    .ThenInclude(p => p.Images)
                .Include(od => od.Product)
                    .ThenInclude(p => p.Promotions)
                .Include(od => od.Product)
                    .ThenInclude(p => p.ProductSizes)
                        .ThenInclude(ps => ps.Size)
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

        public async Task<OrderDetail?> AddOrUpdateOrderDetail(Guid orderId, Guid productId, Guid sizeId, int quantity)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(od => od.Id == orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }

            var productSize = await _context.ProductSizes
                .FirstOrDefaultAsync(ps => ps.ProductId == productId && ps.SizeId == sizeId);

            if (productSize == null)
            {
                throw new KeyNotFoundException($"Product size with SizeId {sizeId} not found for ProductId {productId}.");
            }

            if (productSize.Stock < quantity)
            {
                throw new InvalidOperationException($"Insufficient stock for size {sizeId}. Only {productSize.Stock} items available.");
            }

            // Kiểm tra OrderDetail đã tồn tại
            var existingOrderDetail = await _context.OrderDetails
                .FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId && od.SizeId == sizeId);

            OrderDetail newOrderDetail = null; // Khai báo mới cho OrderDetail

            if (existingOrderDetail != null)
            {
                // Cập nhật số lượng và tổng giá
                existingOrderDetail.Quantity += quantity; 
                existingOrderDetail.TotalPrice = existingOrderDetail.Quantity * product.Price;

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
                    SizeId = sizeId, // Lưu sizeId vào OrderDetail
                    Quantity = quantity, 
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


        public async Task<bool> RemoveOrderDetail(Guid orderId, Guid productId, Guid sizeId)
        {
            // Kiểm tra Order tồn tại
            var order = await _context.Orders
                .FirstOrDefaultAsync(od => od.Id == orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");
            }

            // Kiểm tra OrderDetail có tồn tại với productId và sizeId
            var orderDetail = await _context.OrderDetails.Include(od => od.Product)
                .FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId && od.SizeId == sizeId);
            if (orderDetail == null)
            {
                throw new KeyNotFoundException($"OrderDetail for Product ID {productId} and Size ID {sizeId} not found in Order ID {orderId}.");
            }

            // Xóa OrderDetail
            _context.OrderDetails.Remove(orderDetail);

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


        public async Task<OrderDetail> UpdateOrderDetail(Guid orderId, Guid productId, Guid sizeId, int quantity)
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
                .FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId && od.SizeId == sizeId);

            if (existingOrderDetail == null)
            {
                throw new KeyNotFoundException($"OrderDetail with OrderId {orderId}, ProductId {productId}, and SizeId {sizeId} not found.");
            }

            // Kiểm tra stock trong ProductSize
            var productSize = await _context.ProductSizes
                .FirstOrDefaultAsync(ps => ps.ProductId == productId && ps.SizeId == sizeId);
            if (productSize == null)
            {
                throw new KeyNotFoundException($"ProductSize with ProductId {productId} and SizeId {sizeId} not found.");
            }

            if (productSize.Stock < quantity)
            {
                // Nếu stock không đủ, chỉ cập nhật với số lượng còn lại nhưng không thay đổi stock của sản phẩm
                existingOrderDetail.Quantity = productSize.Stock; // Sử dụng số lượng có sẵn
                existingOrderDetail.TotalPrice = productSize.Stock * product.Price;

                // Cập nhật giá của Order
                order.Price = await _context.OrderDetails
                    .Where(od => od.OrderId == orderId)
                    .SumAsync(od => od.TotalPrice);

                // Không thay đổi stock của ProductSize hay Product

                // Cập nhật vào cơ sở dữ liệu
                _context.Entry(existingOrderDetail).State = EntityState.Modified;
                _context.Entry(order).State = EntityState.Modified;

                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();

                throw new InvalidOperationException($"Stock for ProductId {productId} and SizeId {sizeId} is not sufficient. Updated with available stock: {productSize.Stock}.");
            }

            // Nếu stock đủ, cập nhật bình thường
            existingOrderDetail.Quantity = quantity;
            existingOrderDetail.TotalPrice = quantity * product.Price;

            // Cập nhật giá của Order
            order.Price = await _context.OrderDetails
                .Where(od => od.OrderId == orderId)
                .SumAsync(od => od.TotalPrice);

            // Cập nhật vào cơ sở dữ liệu
            _context.Entry(existingOrderDetail).State = EntityState.Modified;
            _context.Entry(order).State = EntityState.Modified;

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            return existingOrderDetail; // Trả về OrderDetail đã cập nhật
        }


    }

}


