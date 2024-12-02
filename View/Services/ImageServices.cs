using Data.Models;
using Newtonsoft.Json;
using View.IServices;

namespace View.Services
{
    public class ImageServices : IImageServices
    {
        private readonly HttpClient _httpClient;
        public ImageServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task Create(Image image)
        {
            await _httpClient.PostAsJsonAsync("https://localhost:7280/api/Images", image);
        }

        public async Task Delete(Guid id)
        {
            await _httpClient.DeleteAsync($"https://localhost:7280/api/Images/{id}");
        }

        public async Task<IEnumerable<Image>?> GetAllImages()
        {
            var response = await _httpClient.GetStringAsync("https://localhost:7280/api/Images");
            var Images = JsonConvert.DeserializeObject<IEnumerable<Image>>(response);
            return Images;
        }

        public async Task<Image?> GetImageById(Guid id)
        {
            var response = await _httpClient.GetStringAsync($"https://localhost:7280/api/Images/{id}");
            var Image = JsonConvert.DeserializeObject<Image>(response);
            return Image;
        }

        public async Task<IEnumerable<Image>?> GetImagesByProductId(Guid id)
        {
            var Img = GetAllImages().Result;
            return Img != null ? Img.Where(i => i.ProductId == id) : Img;
        }

        public async Task Update(Image image)
        {
            await _httpClient.PutAsJsonAsync($"https://localhost:7280/api/Images/{image.Id}", image);
        }
    }
}
