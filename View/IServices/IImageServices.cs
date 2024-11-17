using Data.Models;

namespace View.IServices
{
    public interface IImageServices
    {
        Task<IEnumerable<Image>?> GetAllImages();
        Task<IEnumerable<Image>?> GetImagesByProductId(Guid id);
        Task<Image?> GetImageById(Guid id);
        Task Create(Image image);
        Task Update(Image image);
        Task Delete(Guid id);
    }
}
