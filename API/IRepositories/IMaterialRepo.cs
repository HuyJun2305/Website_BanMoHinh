using Data.Models;

namespace API.IRepositories
{
    public interface IMaterialRepo
    {
        Task<List<Material>> GetAllMaterials();
        Task<Material> GetMaterialById(Guid id);
        Task Create(Material material);
        Task Update(Material material);
        Task Delete(Guid id);
        Task SaveChanges();
    }
}
