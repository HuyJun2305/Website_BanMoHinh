using Data.Models;

namespace View.ViewModels
{
	public class CounterSalesViewModel
	{
		public Guid OrderId { get; set; }
		public IEnumerable<Order>? orders { get; set; }
		public IEnumerable<Image>? images { get; set; }
		public IEnumerable<Product>? products { get; set; }
		public IEnumerable<OrderDetail>? orderDetails { get; set; }
	}
}
