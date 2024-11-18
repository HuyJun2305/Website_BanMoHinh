using Data.Models;

namespace API.IRepositories
{
    public interface IOrderDetailRepo
    {
        Task<List<OrderDetail>?> GetAllOrderDetails();
        Task<List<OrderDetail>?> GetOrderDetailsByOrderId(Guid id);
        Task<OrderDetail?> GetOrderDetailById(Guid id);
        Task Create(OrderDetail orderDetail);
        Task Update(OrderDetail orderDetail);
        Task Delete(Guid id);
        Task SaveChanges();
    }
}
