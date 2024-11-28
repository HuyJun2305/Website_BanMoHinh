using Data.DTO;
using Data.Models;

namespace View.IServices
{
    public interface IUserServices
    {
        Task Create(UserData user1);
        Task Delete(string id);
        Task<IEnumerable<string>?> GetAllRoles(string username);
        Task<IEnumerable<ApplicationUser>?> GetAllUser();
        Task<List<ApplicationUser>> GetFilteredUser(Guid? searchQuery = null, Guid? sizeId = null, Guid? imageId = null, Guid? brandId = null, Guid? materialId = null);
        Task<ApplicationUser?> GetUserById(string id);
        Task Update(UserData user, string id);
    }
}