using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime DayCreate { get; set; }
        public decimal Price { get; set; }
		public PaymentMethod PaymentMethods { get; set; } = PaymentMethod.Cash;
		public OrderStatus Status { get; set; } = OrderStatus.CreateOrder;
		public PaymentStatus PaymentStatus {  get; set; }	

		public string? Note { get; set; }

		public Guid CreateBy { get; set; }
        public DateTime? DayPayment { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }

        public decimal? AmountPaid { get; set; }             
        public decimal? Change { get; set; }
        public decimal? ShippingFee { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? VoucherId { get; set; }
        [JsonIgnore]
        public virtual Voucher? Voucher { get; set; }
        public virtual ApplicationUser? Account { get; set; }
		public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
		public virtual OrderAddress? OrderAddresses { get; set; }


    }

	public enum PaymentMethod
	{
		[Display(Name = "Tiền mặt")]
		Cash = 0,
		[Display(Name = "MoMo")]
		MoMo = 1,
		[Display(Name = "VNPay")]
		VNPay = 2,
		[Display(Name = "ZaloPay")]
		Zalo = 3
		

	}

	public enum OrderStatus
	{
        [Display(Name = "Tạo đơn hàng")]
        CreateOrder = 0,
		[Display(Name = "Chờ xác nhận")]	
		WaitingForConfirmation = 1,
		[Display(Name = "Chuẩn bị đơn hàng")]
        PrepareOrder = 2,
		[Display(Name = "Giao hàng")]
        OnDelivery = 3,
		[Display(Name = "Đã giao")]
        Delivered = 4,
		[Display(Name = "Hoàn thành")]
        Complete = 5,

		// Trong trường hợp xảy ra thất thoát
		[Display(Name = "Đã Hủy")]
        Canceled = 6,
		[Display(Name = "Mất đơn")]
        LostGoods = 7,
		[Display(Name = "Hoàn trả")]
        Refund = 8,
        [Display(Name = "Sai thông tin địa chỉ")]
        IncorrectAddress = 9,
        [Display(Name = "Tai nạn giao thông")]
        Accident = 10,
		[Display(Name = "Xác nhận hoàn trả")]
		AcceptRefund = 11


    }

	public enum PaymentStatus
	{
		[Display(Name = "Chờ thanh toán")]
		Pending,
        [Display(Name = "Tạm ứng")]
        Advance,
        [Display(Name = "Đã thanh toán")]
        Paid,
        [Display(Name = "Thất bại")]
        Failed,
        [Display(Name = "Đang xử lý")]
        Processing,
        [Display(Name = "Đã hoàn trả")]
        Refunded
    }
}
