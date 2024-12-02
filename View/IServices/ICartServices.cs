using Data.Models;

namespace View.IServices
{
	public interface ICartServices 
	{
		Task<Cart> GetCartAsync(Guid id);
		Task<Cart> GetCartByUserId(Guid userId);
		Task CreateCart(Cart cart);
		Task Update(Cart cart, Guid id);
		Task<List<CartDetail>> GetAllCartDetails();
		Task<List<CartDetail>> GetCartDetailByCartId(Guid cartId);
		Task<CartDetail> GetCartDetailByProductId(Guid cartId, Guid productId);
		Task<Cart> GetCartDetailById(Guid id);
		Task<CartDetail> AddToCart(Guid cartId, Guid productId, int quantity);
		Task Delete(Guid id);
	}
}
