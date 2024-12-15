    using Data.Models;

namespace API.IRepositories
{
    public interface ICartRepo
    {
        Task<List<Cart>> GetAllCart();
        Task<Cart> GetCartById(Guid id);
        Task<Cart> GetCartByUserId(Guid userId);
        Task Create(Cart cart);
		Task<Cart> CreateAsync(Cart cart);

		Task Update(Cart cart);
        Task SaveChanges();
    }
}
