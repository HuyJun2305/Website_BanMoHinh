using Data.Models;

namespace View.ViewModel
{
    public class VoucherViewModel
    {
        public Voucher Voucher { get; set; } // Đối tượng Voucher
        public int CurrentPage { get; set; } // Trang hiện tại (cho phân trang)
        public int TotalPages { get; set; } // Tổng số trang (cho phân trang)

    }
}
