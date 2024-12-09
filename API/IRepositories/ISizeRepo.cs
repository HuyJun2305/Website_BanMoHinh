using Data.Models;

namespace API.IRepositories
{
    public interface ISizeRepo
    {
        Task<List<Size>> GetAllSize();
        Task<List<Size>> GetSizeByStatus();
        Task<List<Size>> GetSizeByProductId(Guid productId);
        Task<Size> GetSizeById(Guid id);
        Task Create(Size size);
        Task Update(Size size);
        Task Delete(Guid id);
        Task SaveChanges();
    }
}
