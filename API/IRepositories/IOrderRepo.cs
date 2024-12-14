using Data.Models;

namespace API.IRepositories
{
    public interface IOrderRepo
    {
        Task<List<Order>> GetAllOrder();
        Task<List<Order>> GetOrderStatus();
        Task<List<Order>> GetOrderByStatus(OrderStatus status);
        Task<List<Order>> GetOrdersByCustomerIdAndStatus(Guid customerId, OrderStatus status);
        Task<Order> GetOrderById(Guid id);  
        Task<List<Order>> GetOrderByCustomerId(Guid customerId);
        Task<Order> CreateByStaff(Guid staffId, Guid? customerId = null, Guid? voucherid = null);
        Task Create(Order order);
        Task Update(Order order);   
        Task Delete(Guid id);
        Task SaveChanges();
        Task CheckOutInStore(Guid orderId, Guid staff, decimal amountGiven, PaymentMethod paymentMethod);
        Task AcceptOrder(Guid orderId);
        Task CancelOrder(Guid orderId, string? note);
        Task DeliveryOrder(Guid orderId, string? note);
        Task ConplateOrder(Guid orderId, string? note);
        Task RefundOrder(Guid orderId, string? note);
        Task ShippingError(Guid orderId, string? note);
        Task MissingInformation(Guid orderId, string? note);  
        Task LoseOrder(Guid orderId, string? note);   

    }
}
