using Data.Models;

namespace API.IRepositories
{
    public interface IImageRepo
    {
        Task<List<Image>> GetAllImage();
        Task<Image> GetImageById(Guid id);
        Task Create(Image image);
        Task Update(Image image);
        Task Delete(Guid id);
        Task SaveChanges();
    }
}
