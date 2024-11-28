using Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace View.ViewModel
{
    
    public class ProductIndex
    {
        public IEnumerable<Product>? Products { get; set; }
        public IEnumerable<Image>? Images { get; set; }
    }
    public class ProductViewModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public Guid BrandId { get; set; }
        public Guid MaterialId { get; set; }
        public Guid? PromotionId { get; set; }
        public Guid SizeId { get; set; }

        //
        public IEnumerable<Image>? Images { get; set; }

        //
        public List<ProductViewModel> ProductDetails { get; set; } = new List<ProductViewModel>();
    }
    public class ImageSP
        {
            public string Url { get; set; }
            public Guid ProductId { get; set; }
        }
    
}
