using Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace View.ViewModels
{

    public class ProductIndex
    {
        public IEnumerable<Product>? Products { get; set; }
        public IEnumerable<Image>? Images { get; set; }
        public Guid CartId { get; set; }

    }
    public class ProductViewModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public Guid BrandId { get; set; }
        public Guid MaterialId { get; set; }
        public Guid? PromotionId { get; set; }
        public Guid SizeId { get; set; }
        public Guid CategoryId { get; set; }


        public Guid Id { get; set; }
        public IEnumerable<Image>? Images { get; set; }
        public IEnumerable<string>? SelectedImageIds { get; set; }

        public List<ProductViewModel> ProductDetails { get; set; } = new List<ProductViewModel>();
    }
    public class ImageSP
    {
        public string Url { get; set; }
        public Guid ProductId { get; set; }
    }

}
