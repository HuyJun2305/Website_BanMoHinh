using Data.Models;

namespace View.IServices
{
    public interface IBrandServices
    {
        Task<IEnumerable<Brand>> GetAllBrands();
        Task<Brand> GetBrandById(Guid id);
        Task Create(Brand Brand);
        Task Update(Brand Brand);
        Task Delete(Guid id);
    }
}
