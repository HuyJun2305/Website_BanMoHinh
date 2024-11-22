using Data.Models;

namespace View.IServices
{
    public interface IMaterialServices
    {
        Task<IEnumerable<Material>> GetAllMaterials();
        Task<Material> GetMaterialById(Guid id);
        Task Create(Material Material);
        Task Update(Material Material);
        Task Delete(Guid id);
    }
}
