using Data.Models;

namespace View.ViewModels
{
    public class OrderViewModel
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public decimal? Price { get; set; }
        public PaymentMethod PaymentMethods { get; set; }
        public DateTime DayCreate { get; set; }
        public OrderStatus Status { get; set; }
    }
    public class OrderIndexViewModel
    {
        public IDictionary<OrderStatus, IEnumerable<OrderViewModel>> OrdersByStatus { get; set; }
    }

}
