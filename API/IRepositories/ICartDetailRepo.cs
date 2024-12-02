using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.IRepositories
{
    public interface ICartDetailRepo
    {
        Task<List<CartDetail>> GetAllCartDetail();
        Task<List<CartDetail>> GetCartDetailByCartId(Guid cartId);
        Task<CartDetail> GetCartDetailByProductId(Guid cartId, Guid productId);
        Task<CartDetail> GetCartDetailById(Guid id);
        Task Create(CartDetail cartDetails);
        Task Update(CartDetail cartDetails, Guid id);   
        Task Delete(Guid id);

        Task AddToCart(Guid cartId, Guid productId, int quantity);

	}
}

