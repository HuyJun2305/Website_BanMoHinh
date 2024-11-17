using Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace View.ViewModel
{
    public class ProductViewModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public Guid IdBrand { get; set; }
        public Guid IdMaterial { get; set; }
        public Guid IdPromotion { get; set; }
        public Guid IdSize { get; set; }

        //
        public List<ProductViewModel> ProductDetails { get; set; } = new List<ProductViewModel>();
    }
}
