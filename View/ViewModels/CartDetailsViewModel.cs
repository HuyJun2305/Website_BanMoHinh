using Data.Models;

namespace View.ViewModels
{
    public class CartDetailViewModel
    {
        public IEnumerable<CartDetailVM>? CartDetails { get; set; }
        public IEnumerable<ProductSize>? ProductSizes { get; set; }
        public IEnumerable<Image>? Images { get; set; }
    }

    public class CartDetailVM
    {
        public Guid? Id { get; set; }
        public int? Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid? CartId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? SizeId { get; set; }
        public string ProductName { get; set; }
        public string SizeName { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int Weight { get; set; }

    }


}
