using Data.Models;

namespace API.IRepositories
{
    public interface IBrandRepo
    {
        Task<List<Brand>> GetAllBrands();
        Task<Brand> GetBrandById(Guid id);
        Task Create(Brand brand);
        Task Update(Brand brand);
        Task Delete(Guid id);
        Task SaveChanges();
    }
}
