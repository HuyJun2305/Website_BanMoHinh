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
		[Display(Name = "Cash")]
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
        [Display(Name = "Create Order")]
        CreateOrder = 0,
		[Display(Name = "Waiting for confirmation")]	
		WaitingForConfirmation = 1,
		[Display(Name = "Prepare order")]
        PrepareOrder = 2,
		[Display(Name = "On Delivery")]
        OnDelivery = 3,
		[Display(Name = "Delivered")]
        Delivered = 4,
		[Display(Name = "Complete")]
        Complete = 5,

		// Trong trường hợp xảy ra thất thoát
		[Display(Name = "Canceled")]
        Canceled = 6,
		[Display(Name = "Lost goods")]
        LostGoods = 7,
		[Display(Name = "Refund")]
        Refund = 8,
        [Display(Name = "Incorrect address information")]
        IncorrectAddress = 9,
        [Display(Name = "Delivery man had a traffic accident")]
        Accident = 10,
		[Display(Name = "Products accepted for return")]
		AcceptRefund = 11


    }

	public enum PaymentStatus
	{
		[Display(Name = "Pending")]
		Pending,
        [Display(Name = "Advance")]
        Advance,
        [Display(Name = "Paid")]
        Paid,
        [Display(Name = "Failed")]
        Failed,
        [Display(Name = "Processing")]
        Processing,
        [Display(Name = "Refunded")]
        Refunded
    }
}
