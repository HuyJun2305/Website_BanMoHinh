using Data.Models;

namespace View.IServices
{
	public interface IOrderServices
	{
		Task<IEnumerable<Order>> GetAllOrder();
		Task<IEnumerable<Order>> GetAllOrderStatus0();
        Task<IEnumerable<Order>> GetOrdersByStatus(OrderStatus status);
        Task<IEnumerable<Order>> GetOrdersByCustomerId(Guid customerId);
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAndStatus(Guid customerId, OrderStatus status);
        Task<Order> GetOrderById(Guid id);
		Task<Order> CreateByStaff(Guid staffId, Guid? customerId = null, Guid? voucherid = null);
		Task Create(Order order);
		Task Update(Order order);
		Task Delete(Guid id);
	}
}
