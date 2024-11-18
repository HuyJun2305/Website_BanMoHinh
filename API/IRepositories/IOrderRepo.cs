using Data.Models;

namespace API.IRepositories
{
    public interface IOrderRepo
    {
        Task<List<Order>> GetAllOrder();
        Task<Order> GetOrderById(Guid id);
        Task Create(Order order);
        Task Update(Order order);
        Task Delete(Guid id);
        Task SaveChanges();
    }
}
