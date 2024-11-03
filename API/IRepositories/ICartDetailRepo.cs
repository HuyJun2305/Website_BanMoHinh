using Data.Models;

namespace API.IRepositories
{
    public interface ICartDetailRepo
    {
        Task<List<CartDetail>> GetAllCartDetail();
        Task<CartDetail> GetCartDetailById(Guid id);
        Task Create(CartDetail cartdetail);
        Task Update(CartDetail cartdetail);
        Task Delete(Guid id);
        Task SaveChanges();
    }
}
