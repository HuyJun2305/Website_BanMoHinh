using Data.Models;

namespace View.IServices
{
	public interface IOrderDetailServices
	{
		Task<List<OrderDetail>?> GetOrderDetailsByOrderIdAsync(Guid orderId);
		Task<OrderDetail?> GetOrderDetailByIdAsync(Guid id);
		Task<OrderDetail?> GetOrderDetailByOrderAndProductIdAsync(Guid orderId, Guid productId);
		Task<OrderDetail?> AddOrUpdateOrderDetail(Guid orderId, Guid productId, int quantity);
		Task<bool> RemoveOrderDetail(Guid orderId, Guid productId, int quantityToRemove);
		Task<decimal> GetTotalPriceByOrderIdAsync(Guid orderId);
		Task CreateAsync(OrderDetail orderDetail);
		Task SaveChangesAsync();
	}
}
