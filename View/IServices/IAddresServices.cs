using Data.Models;

namespace View.IServices
{
    public interface IAddresServices
    {
        Task Create(Address address);
        Task Delete(Guid id);
        Task<IEnumerable<Address>> GetAllBrands();
        Task<Address> GetBrandById(Guid id);
        Task Update(Address address);
    }
}