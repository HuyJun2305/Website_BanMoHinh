using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.IRepositories
{
    public interface IOrderDetailRepo
    {
		Task<List<OrderDetail>?> GetOrderDetailsByOrderIdAsync(Guid? orderId);
		Task<OrderDetail?> GetOrderDetailByIdAsync(Guid id);
		Task<OrderDetail?> GetOrderDetailByOrderAndProductIdAsync(Guid orderId, Guid productId);
		Task<OrderDetail?> AddOrUpdateOrderDetail(Guid orderId, Guid productId, int quantity);
		Task<OrderDetail?> UpdateOrderDetail(Guid orderId, Guid productId, int quantity);
        Task<bool> RemoveOrderDetail(Guid orderId, Guid productId, int quantityToRemove);
		Task AddOrderDetailAsync(OrderDetail orderDetail);
		Task UpdateOrderDetailAsync(OrderDetail orderDetail);
		Task<decimal> GetTotalPriceByOrderIdAsync(Guid orderId);
		Task CreateAsync(OrderDetail orderDetail);
		Task SaveChangesAsync();
	}
}
