using Data.Models;

namespace API.IRepositories
{
    public interface IAddresRepo
    {
        Task Create(Address data);
        Task Delete(Guid id);
        Task<Address> GetAddresById(Guid id);
        Task<List<Address>> GetAllAddres();
        Task SaveChanges();
        Task Update(Address data);
    }
}