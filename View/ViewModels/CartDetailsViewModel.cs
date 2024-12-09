using Data.Models;

namespace View.ViewModels
{
	public class CartDetailsViewModel
	{

        public Guid? CartId { get; set; }
        public IEnumerable<Product>? Products { get; set; }
        public IEnumerable<CartDetail>? CartDetails { get; set; }
        public IEnumerable<Image>? Images { get; set; }
    }
}
