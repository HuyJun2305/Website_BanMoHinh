using Data.Models;

namespace API.IRepositories
{
    public interface ICartDetailRepo
    {
        Task<List<CartDetail>> GetAllCartDetail();
        Task<List<CartDetail>> GetCartDetailByCartId(Guid cartId);
        Task<CartDetail> GetCartDetailById(Guid id);
        Task Create(CartDetail cartDetails);
        Task Update(CartDetail cartDetails, Guid id);
        Task Delete(Guid id);
    }
}
