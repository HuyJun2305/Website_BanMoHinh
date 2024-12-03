using Data.Models;

namespace API.IRepositories
{
    public interface IPromotionRepos
    {
        Task<List<Promotion>> GetAllPromotion();
        Task<Promotion> GetPromotionById(Guid id);
        Task Create(Promotion promotion);
        Task Update(Promotion promotion);

        Task applyToProduct(List<Guid> pros, Guid promId);
        Task Delete(Guid id);
        Task SaveChanges();
    }
}
