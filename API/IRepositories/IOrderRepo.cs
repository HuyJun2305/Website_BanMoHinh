using Data.Models;

namespace API.IRepositories
{
    public interface IOrderRepo
    {
        Task<List<Order>> GetAllOrder();
        Task<List<Order>> GetOrderStatus();
        Task<List<Order>> GetOrderByStatus(OrderStatus status);
        Task<Order> GetOrderById(Guid id);  
        Task<Order> CreateByStaff(Guid staffId, Guid? customerId = null, Guid? voucherid = null);
        Task Create(Order order);
        Task Update(Order order);   
        Task Delete(Guid id);
        Task SaveChanges();
        Task CheckOutInStore(Guid orderId, Guid staff, decimal amountGiven, PaymentMethod paymentMethod);
    }
}
