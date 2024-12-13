using Data.Models;

namespace API.IRepositories
{
    public interface IOrderAddressRepo
    {
        Task<List<OrderAddress>> GetAllOrderAddress();
        Task<OrderAddress> GetOrderAddressById(Guid id);
        Task<OrderAddress> GetOrderAddressByOrderId(Guid orderId);
        Task Create(OrderAddress orderAddress);
        Task Update(OrderAddress orderAddress);
        Task Delete(Guid id);
        Task SaveChanges();
    }
}
