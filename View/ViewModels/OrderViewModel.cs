using Data.Models;

namespace View.ViewModels
{
    public class OrderViewModel
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public decimal? Price { get; set; }
        public string Note { get; set; }
        public PaymentMethod PaymentMethods { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime DayCreate { get; set; }
        public OrderStatus Status { get; set; }
        public string? PhoneNumber { get; set; }
    }
    public class OrderIndexViewModel
    {
        public IDictionary<OrderStatus, IEnumerable<OrderViewModel>> OrdersByStatus { get; set; }
        public Guid? SelectedOrderId { get; set; }
    }

    public class OrderVM
    {
        public Guid? SelectedOrderId { get; set; }
        public IEnumerable<Order>? Orders { get; set; }
        public IEnumerable<OrderDetail>? OrderDetails { get; set; }
        public IDictionary<OrderStatus, IEnumerable<Order>> OrdersByStatus { get; set; }

    }



}
