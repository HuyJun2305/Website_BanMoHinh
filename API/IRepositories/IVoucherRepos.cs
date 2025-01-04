using Data.Models;

namespace API.IRepositories
{
    public interface IVoucherRepos
    {
        Task<List<Voucher>> GetAll();
        Task<Voucher> GetById(Guid id);
        Task Create(Voucher voucher);
        Task Update(Voucher voucher);
        Task Delete(Guid id);
        Task SaveChanges();
    }
}
