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
        public decimal? Price { get; set; }
		public PaymentMethod PaymentMethods { get; set; } = PaymentMethod.Cash;
		public OrderStatus Status { get; set; } = OrderStatus.TaoDonHang;

		public Guid CreateBy { get; set; }
		public string CustomerName { get; set; }

        public Guid? AccountId { get; set; }
        public Guid? VoucherId { get; set; }
        [JsonIgnore]
        public virtual Voucher? Voucher { get; set; }
        public virtual ApplicationUser? Account { get; set; }
    }

	public enum PaymentMethod
	{
		[Display(Name = "Cash")]
		Cash = 0,
		[Display(Name = "MoMo")]
		MoMo = 1,
		[Display(Name = "VNPay")]
		VNPay = 2

	}

	public enum OrderStatus
	{
		[Display(Name = "Tạo đơn hàng")]
		TaoDonHang = 0,
		[Display(Name = "Chờ xác nhận")]
		ChoXacNhan = 1,
		[Display(Name = "Chuẩn bị đơn hàng")]
		ChuanBiDonHang = 2,
		[Display(Name = "Đang giao hàng")]
		DangGiaoHang = 3,
		[Display(Name = "Đã giao hàng")]
		DaGiaoHang = 4,
		[Display(Name = "Hoàn thành")]
		HoanThanh = 5,

		// Trong trường hợp xảy ra thất thoát
		[Display(Name = "Đã huỷ")]
		DaHuy = 6,
		[Display(Name = "Mất hàng")]
		MatHang = 7,
		[Display(Name = "Hoàn trả")]
		HoanTra = 8
	}
}
